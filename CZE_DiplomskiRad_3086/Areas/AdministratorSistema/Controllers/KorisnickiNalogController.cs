using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministratorSistema.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CZE.Web.Areas.AdministratorSistema.Controllers
{
    [Area(nameof(CZE.Web.Areas.AdministratorSistema))]
    [Autorizacija()]
    public class KorisnickiNalogController : Controller
    {
        private readonly CZEContext _db;
        private readonly IConfiguration _config; 
        public KorisnickiNalogController(CZEContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var nalog = await _db.KorisnickiNalozi.FindAsync(id);
            var pass = Autentifikacija.GenerateRandomPassword();
            var model = new KorisnickiNalogVM()
            {
                OsobaId = nalog.KorisnickiNalogId,
                KorisnickiNalogId = nalog.KorisnickiNalogId,
                UlogaKorisnika = nalog.UlogaKorisnika,
                KorisnickoIme = nalog.KorisnickoIme,
                Aktivan = nalog.Aktivan,
                Lozinka = pass,
                SendEmail = true,
                EmailBody = MyEmailHelper.GetKorisnickiPodatciEditEmailBody()};

            return PartialView("_Edit", model);
        }
        //Kreiranje KorisnickogNaloga zapocinje u OsobaController->QuickDetails
        //public async Task<IActionResult> Create(int id)
        //{
        //    UlogeKorisnika uloga = UlogeKorisnika.Kandidat;
        //    string password = Autentifikacija.GenerateRandomPassword();

        //    Osoba osoba = await _db.Osobe.Include(i => i.Kandidat).Include(i => i.Zaposlenik).SingleOrDefaultAsync(s => s.OsobaId == id);
        //    if (osoba == null) { return NotFound(); }
        //    if (osoba.Zaposlenik == null && osoba.Kandidat == null)
        //    {
        //        return BadRequest(new
        //        {
        //            Alert = new Alert(Severity.warning,
        //                "Osoba mora biti ili kandidat ili zaposlenik da bi dobila korisnički nalog.", "Korisnički nalog upozorenje")
        //        });
        //    }
        //    if (osoba.Zaposlenik != null)
        //    {
        //        uloga = UlogeKorisnika.AdministrativniRadnik;
        //    }

        //    KorisnickiNalogVM model = new KorisnickiNalogVM()
        //    {
        //        KorisnickiNalogId = osoba.OsobaId,
        //        Aktivan = true,
        //        UlogaKorisnika = uloga,
        //        KorisnickoIme = osoba.Ime + " " + osoba.Prezime,
        //        Lozinka = password

        //    };
        //    return PartialView("_Create", model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KorisnickiNalogVM model)
        {
            if (!ModelState.IsValid) { return PartialView("_Create", model); }
            var prom = await _db.Osobe.Where(w => w.OsobaId == model.OsobaId)
                .Select(s => new { osoba=s, kandidat = s.Kandidat != null, zaposlenik = s.Zaposlenik != null })
                .SingleOrDefaultAsync();
            if (!(prom.kandidat || prom.zaposlenik)) { return BadRequest("Osoba mora biti ili kandidat ili zaposlenik da bi se promovisala!"); }

            var logedNalog= await HttpContext.GetLogiraniKorisnik();
            if (model.UlogaKorisnika<UlogeKorisnika.Kandidat && logedNalog.UlogaKorisnika>UlogeKorisnika.Direktor)
            {
                return BadRequest(new {alert= new Alert(Severity.warning,
                    $"Nema pravo da dodjeli ulogu {model.UlogaKorisnika}.",logedNalog.UlogaKorisnika.ToString(),false)});
            }

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            KorisnickiNalog nalog = new KorisnickiNalog()
            {
                KorisnickiNalogId = model.OsobaId,
                KorisnickoIme = model.KorisnickoIme,
                Aktivan = model.Aktivan,
                UlogaKorisnika = model.UlogaKorisnika,
                LozinkaSalt = Convert.ToBase64String(salt),
                LozinkaHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Lozinka,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8))
            };

            try
            {
                Severity status=Severity.success;
                string detailsMessage = "Uspješno ";
                if (model.KorisnickiNalogId != 0)
                {
                    nalog.KorisnickiNalogId = model.KorisnickiNalogId;

                    _db.KorisnickiNalozi.Attach(nalog);

                    var entry = _db.Entry(nalog);
                    entry.State = EntityState.Modified;
                    entry.Property(p => p.LozinkaHash).IsModified = model.IzmjenaLozinke;
                    entry.Property(p => p.LozinkaSalt).IsModified = model.IzmjenaLozinke;

                    detailsMessage += "editovan.";
                }
                else
                {
                    _db.KorisnickiNalozi.Add(nalog);
                    detailsMessage += "dodan u bazu.";
                }
                await _db.SaveChangesAsync();
                if (model.SendEmail)
                {
                    var emailBody = model.EmailBody.Replace("{lozinkaPolje}", model.Lozinka);
                    try
                    {
                        MyEmailHelper email = new MyEmailHelper(_config)
                        {
                            Subject = "Obavjest-CZE Portala",
                            Recipient = prom.osoba,
                            Body = emailBody
                        };
                        email.SendEmailKorisnickiPodatci();
                        detailsMessage += "\n ; Email poslan korisniku.";
                    }
                    catch (Exception)
                    {
                        status = Severity.warning;
                        detailsMessage += "\n ; Email nije poslan korisniku.";
                    }
                }

                return RedirectToAction("QuickDetails", "Osoba", new { area="AdministrativniRadnik", id =model.OsobaId});
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
        }

        public async Task<IActionResult> SessionsTable()
        {
            KorisnickiNalog nalog = await HttpContext.GetLogiraniKorisnik();
            var list = await _db.AutorizacijskiTokeni.Where(w => w.KorisnickiNalogId == nalog.KorisnickiNalogId)
                .Select(s => new AutorizacijskiToken()
                {
                    AutorizacijskiTokenId = s.AutorizacijskiTokenId,
                    IpAdresa = s.IpAdresa,
                    VrijemeEvidentiranja = s.VrijemeEvidentiranja
                }).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Delete(int id)
        {
            AutorizacijskiToken token = await _db.AutorizacijskiTokeni.FindAsync(id);
            if (token == null)
            {
                return NotFound("Zapis nije pronađen.");
            }
            try
            {
                _db.AutorizacijskiTokeni.Remove(token);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }

            return RedirectToAction(nameof(KorisnickiNalogController.SessionsTable));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeKorisnickoIme(KorisnickoImeVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("_ChangeKorisnickoIme", model);
            }
            var nalog = await _db.KorisnickiNalozi.FindAsync(model.KorisnickiNalogId);
            if (nalog == null)
            {
                return BadRequest($"Korisnicki nalog sa Id:{model.KorisnickiNalogId} brojem nije prodađen.");
            }
            nalog.KorisnickoIme = model.KorisnickoIme;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
            return RedirectToAction("Index", "Postavke", new { Area = "" });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeLozinka(KorisnickaLozinkaVM model)
        {
            if (!ModelState.IsValid){return View("_ChangeLozinka", model);}

            var nalog = await _db.KorisnickiNalozi.FindAsync(model.KorisnickiNalogId);
            if (nalog == null){return BadRequest($"Korisnicki nalog sa Id:{model.KorisnickiNalogId} brojem nije prodađen.");}

            string modelPasswordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.StaraLozinka,
                salt: Convert.FromBase64String(nalog.LozinkaSalt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (!nalog.LozinkaHash.Equals(modelPasswordHash, StringComparison.Ordinal))
            {
                ModelState.AddModelError("", "Pogrešan password.");
                return View("_ChangeLozinka", model);
            }


            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            try
            {
                nalog.LozinkaSalt = Convert.ToBase64String(salt);
                nalog.LozinkaHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: model.NovaLozinka,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
            return RedirectToAction("Index", "Postavke", new { Area = "" });
        }
    }
}