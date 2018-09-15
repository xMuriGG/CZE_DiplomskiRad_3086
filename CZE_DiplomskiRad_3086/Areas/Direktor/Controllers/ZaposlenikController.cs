using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.Direktor.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Areas.Direktor.Controllers
{
    [Area(nameof(CZE.Web.Areas.Direktor))]
    [Autorizacija(UlogeKorisnika.Direktor,UlogeKorisnika.Administrator)]
    public class ZaposlenikController : Controller
    {
        private readonly CZEContext _db;

        public ZaposlenikController(CZEContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Create(int? id)
        {
            Osoba o = null;
            ZaposlenikVMs.ZaposlenikCreateVM model = null;
            if (id != null && id != 0)
            {
                o = await _db.Osobe.Include(i => i.Zaposlenik).FirstOrDefaultAsync(f => f.OsobaId == id);
            }
            if (o == null) return NotFound();

            model = new ZaposlenikVMs.ZaposlenikCreateVM()
            {
                OsobaId = o.OsobaId,
                OsobaNaziv = o.Ime + " " + o.Prezime
            };
            if (o.Zaposlenik != null)
            {
                model.ZaposlenikVM = new ZaposlenikVMs.ZaposlenikVM()
                {
                    ZaposlenikId = o.Zaposlenik.ZaposlenikId,
                    BrojLicneKarte = o.Zaposlenik.BrojLicneKarte,
                    BrojRacuna = o.Zaposlenik.BrojRacuna,
                    StepenObrazovanja = o.Zaposlenik.StepenObrazovanja
                };
            }
            return PartialView("_Create", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ZaposlenikVMs.ZaposlenikCreateVM model)
        {
            //string name = $"{nameof(model.ZaposlenikVM)}.{nameof(model.ZaposlenikVM.BrojLicneKarte)}";
            //ModelState.AddModelError(name, "Error");
            ModelState.Remove("ZaposlenikVM.ZaposlenikId");
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }
            try
            {
                string detailsMessage = "Uspješno ";
                Zaposlenik z = new Zaposlenik()
                {
                    ZaposlenikId = model.OsobaId,
                    StepenObrazovanja = model.ZaposlenikVM.StepenObrazovanja,
                    BrojLicneKarte = model.ZaposlenikVM.BrojLicneKarte,
                    BrojRacuna = model.ZaposlenikVM.BrojRacuna
                };
                if (model.ZaposlenikVM.ZaposlenikId != 0)
                {
                    _db.Zaposlenici.Attach(z);
                    _db.Entry(z).State = EntityState.Modified;
                    detailsMessage += "editovana.";
                }
                else
                {
                    _db.Zaposlenici.Add(z);
                    detailsMessage += "promovisana u zaposlenika.";
                }
                _db.SaveChanges();
                return RedirectToAction("QuickDetails", "Osoba", new {area=nameof(AdministrativniRadnik), id = z.ZaposlenikId });
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var zaposlenik = await _db.Zaposlenici.FindAsync(id);
            if (zaposlenik == null){return NotFound(new{alert = new Alert(Severity.warning, "Nije pronađen", $"Zaposlenik sa Id:{id} brojem")});}
            var model = new ZaposlenikVMs.ZaposlenikVM()
            {
                ZaposlenikId = zaposlenik.ZaposlenikId,
                BrojLicneKarte = zaposlenik.BrojLicneKarte,
                StepenObrazovanja = zaposlenik.StepenObrazovanja,
                BrojRacuna = zaposlenik.BrojRacuna
                
            };
            return PartialView("_Details", model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.Zaposlenici.Where(w => w.ZaposlenikId == id).Select(s => new
            {
                zaposlenik = s,
                kandidat = s.Osoba.Kandidat != null,
                kNalog = s.Osoba.KorisnickiNalog != null
            }).SingleOrDefaultAsync();
            if (model == null)
            {
                return NotFound(new
                {
                    alert = new Alert(Severity.warning, "Nije pronađen.", $"Zaposlenik sa Id:{id} brojem")
                });
            }
            if (!model.kandidat && model.kNalog)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisan. Zaposlenik ima korisnički nalog.", $"Zaposlenik sa Id:{id} brojem")
                });
            }
            try
            {
                _db.Zaposlenici.Remove(model.zaposlenik);
                await _db.SaveChangesAsync();
                return RedirectToAction("QuickDetails", "Osoba", new { area = nameof(AdministrativniRadnik), id  });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisan. Zaposlenik je vjerovatno predavač na grupi ili ima uplate kandidata.", $"Zaposlenik sa Id:{id} brojem")
                });
            }
        }
    }
}