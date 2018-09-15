using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using CZE.Web.Areas.AdministratorSistema.Models;
using CZE.Web.Areas.Direktor.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Areas.AdministrativniRadnik.Controllers
{
    [Area(nameof(CZE.Web.Areas.AdministrativniRadnik))]
    public class OsobaController : Controller
    {
        private readonly CZEContext _db;
        public OsobaController(CZEContext db)
        {
            _db = db;
        }
        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Administrator, UlogeKorisnika.Direktor)]
        public IActionResult Index()
        {
            OsobaIndexVM model = new OsobaIndexVM();
            return View(model);
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Administrator, UlogeKorisnika.Direktor)]
        [HttpPost]
        public async Task<IActionResult> GetOsobeDataTable(int draw, int start, int length,
            [FromForm(Name = "search[value]")] string search,
            [FromForm(Name = "order[0][column]")] int? orderColumn0,
            [FromForm(Name = "order[0][dir]")] string orderDir0,
            [FromForm(Name = "order[1][column]")] int? orderColumn1,
            [FromForm(Name = "order[1][dir]")] string orderDir1,
            [FromForm(Name = "order[2][column]")] int? orderColumn2,
            [FromForm(Name = "order[2][dir]")] string orderDir2
            )
        {
            var query = _db.Osobe.Select(s => new OsobaListItemVM()
            {
                OsobaId = s.OsobaId,
                ImePrezime = s.Ime + " " + s.Prezime,
                DatumRodjenja = s.DatumRodjenja.ToString("d"),
                Spol = s.Spol,
                Email = s.Email,
                Grad = s.MjestoBoravka.Naziv,
                Adresa = s.Adresa,
                Permisija = 0 + (s.Kandidat == null ? 0 : 2) + (s.Zaposlenik == null ? 0 : 1) + (s.KorisnickiNalog != null ? "-" + (int)s.KorisnickiNalog.UlogaKorisnika : "")
            });
            var total = await query.CountAsync();
            var recordsFiltered = total;
            //text filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(
                    w => w.ImePrezime.Contains(search) ||
                    w.Grad.StartsWith(search)
                    );
                recordsFiltered = await query.CountAsync();
            }

            //ordering
            IOrderedQueryable<OsobaListItemVM> ordering = query.OrderBy(o => 0);
            if (string.IsNullOrEmpty(orderDir0))
            {
                ordering = query.OrderByDescending(o => o.OsobaId);

            }
            if (orderColumn0 != null)
            {
                switch (orderColumn0)
                {
                    case 0:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.ImePrezime) : ordering.OrderByDescending(o => o.ImePrezime);
                        break;
                    case 3:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.Grad) : ordering.OrderByDescending(o => o.Grad);
                        break;
                    case 5:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.Permisija) : ordering.OrderByDescending(o => o.Permisija);
                        break;
                }
            }
            if (orderColumn1 != null)
            {
                switch (orderColumn1)
                {
                    case 0:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.ImePrezime) : ordering.ThenByDescending(o => o.ImePrezime);
                        break;
                    case 3:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.Grad) : ordering.ThenByDescending(o => o.Grad);
                        break;
                    case 5:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.Permisija) : ordering.ThenByDescending(o => o.Permisija);
                        break;
                }
            }
            if (orderColumn2 != null)
            {
                switch (orderColumn2)
                {
                    case 0:
                        ordering = orderDir2 == "asc" ? ordering.ThenBy(o => o.ImePrezime) : ordering.ThenByDescending(o => o.ImePrezime);
                        break;
                    case 3:
                        ordering = orderDir2 == "asc" ? ordering.ThenBy(o => o.Grad) : ordering.ThenByDescending(o => o.Grad);
                        break;
                    case 5:
                        ordering = orderDir2 == "asc" ? ordering.ThenBy(o => o.Permisija) : ordering.ThenByDescending(o => o.Permisija);
                        break;
                }
            }


            query = ordering.Skip(start).Take(length);

            return Json(new
            {
                draw = draw,
                recordsTotal = total,
                recordsFiltered = recordsFiltered,
                data = await query.ToListAsync()
            });
        }
        
        public IActionResult Create(int? id)
        {
            return ViewComponent("Registracija", new { id, klijentskiPortalCall = false });

            //ovaj dio koda je zamjenjen view komponentom Registracija
            //var model = new OsobaCreateVM
            //{
            //    Gradovi = await _db.Gradovi.Select(s => new SelectListItem()
            //    {
            //        Text = s.Naziv,
            //        Value = s.GradId.ToString()
            //    }).ToListAsync()
            //};
            //if (id != null && id != 0)
            //{
            //    var o = await _db.Osobe.FindAsync(id);
            //    if (o != null)
            //    {
            //        model.Osoba = new OsobaCreateVM.Person()
            //        {
            //            OsobaId = o.OsobaId,
            //            Ime = o.Ime,
            //            Prezime = o.Prezime,
            //            DatumRodjenja = o.DatumRodjenja,
            //            Email = o.Email,
            //            BrojTelefona = o.BrojTelefona,
            //            Adresa = o.Adresa,
            //            BrojMobitela = o.BrojMobitela,
            //            MjestoBoravkaId = o.MjestoBoravkaId,
            //            Spol = o.Spol,
            //            KakoSteSaznaliZaNas = o.KakoSteSaznaliZaNas,
            //            BrojTelefonaFirme = o.BrojTelefonaFirme,
            //            AdresaFirme = o.AdresaFirme,
            //            PodatciOFirmi = o.PodatciOFirmi,
            //            NazivFirme = o.NazivFirme,
            //            MjestoRodjenjaId = o.MjestoRodjenjaId,
            //            GradFirmeId = o.GradFirmeId,
            //            EmailFirme = o.EmailFirme
            //        };
            //    }
            //}
            //return PartialView("_Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OsobaCreateVM model)
        {
            ModelState.Remove("Osoba.OsobaId");

            if (!ModelState.IsValid)
            {
                model.Gradovi = await _db.Gradovi.Select(s => new SelectListItem()
                {
                    Text = s.Naziv,
                    Value = s.GradId.ToString()
                }).ToListAsync();

                return PartialView("\\Views\\Shared\\Components\\Registracija\\Default.cshtml", model);
            }
            string alertSummary, alertDetails = "Uspješno ";
            try
            {
                Osoba o = model.ToOsoba();
                alertSummary = $"Osoba: {o.Ime} {o.Prezime}";
                if (model.Osoba.OsobaId != 0)
                {
                    alertDetails += "editovana.";
                    _db.Osobe.Attach(o);
                    _db.Entry(o).State = EntityState.Modified;
                }
                else
                {
                    alertDetails += "registrovana na sistem. Nakon što osoblje obradi zahtjev, pristupne podatke ćete dobiti na Email.";
                    _db.Osobe.Add(o);
                }
                _db.SaveChanges();
                if ((bool?)TempData["kpCall"] ?? false)
                {
                    ViewData["Info-summary"] = alertSummary;
                    ViewData["Info-details"] = alertDetails;
                    return View("Info");
                }
                else
                {
                    return StatusCode(201, new { alert = new Alert(Severity.success, alertDetails, alertSummary) });
                }
            }
            catch (Exception e)
            {
                if (e is DbUpdateException)
                {
                    if ((e.InnerException as SqlException)?.Number == 2601)
                        ModelState.AddModelError("Osoba.Email", $"Email {model.Osoba.Email} je korišten za prijavu.");

                }
                ModelState.AddModelError(String.Empty, e.Message);
                Console.WriteLine(e);
            }

            model.Gradovi = await _db.Gradovi.Select(s => new SelectListItem()
            {
                Text = s.Naziv,
                Value = s.GradId.ToString()
            }).ToListAsync();
            return PartialView("\\Views\\Shared\\Components\\Registracija\\Default.cshtml", model);
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Administrator, UlogeKorisnika.Direktor)]
        public async Task<IActionResult> Details(int id)
        {
            string randomLozinka = Autentifikacija.GenerateRandomPassword();
            var model = await _db.Osobe.Where(w => w.OsobaId == id).Select(s => new OsobaVMs.OsobaDetailsVM()
            {
                OsobaQuickDetailsVM = GetOsobaQuickDetailsVMQuery(id).FirstOrDefault(),
                GrupeKandidataVM = _db.GrupaKandidati.Where(w => w.KandidatId == s.OsobaId).Select(gk => new GrupaVMs.GrupaKandidataVM()
                {
                    GrupaKandidatiId = gk.GrupaKandidatiId,
                    GrupaId = gk.GrupaId,
                    Naziv = gk.Grupa.Naziv,
                    Pocetak = gk.Grupa.Pocetak.ToString("d/M/yyyy H:mm"),

                    KursKategorijaId = gk.Grupa.Kurs.KursTip.KursKategorijaId,
                    KursKategorijaNaziv = gk.Grupa.Kurs.KursTip.KursKategorija.Naziv,
                    KursTipId = gk.Grupa.Kurs.KursTipId,
                    KursTipNaziv = gk.Grupa.Kurs.KursTip.Naziv,
                    KursId = gk.Grupa.KursId,
                    KursNaziv = gk.Grupa.Kurs.Naziv,

                    ZaposlenikId = gk.Grupa.ZaposlenikId,
                    ZaposlenikNaziv = gk.Grupa.Zaposlenik.Osoba.Ime + " " + gk.Grupa.Zaposlenik.Osoba.Prezime,

                    CentarId = gk.Grupa.CentarId,
                    CentarLokacija = gk.Grupa.Centar.Grad.Naziv,

                    Status = gk.Grupa.Status,
                    Cijena = (gk.Grupa.Cijena ?? gk.Grupa.Kurs.KursTip.Cijena) + "",
                    Uplaceno = _db.UplateKandidata.Where(w => w.GrupaKandidatiId == gk.GrupaKandidatiId).Sum(sum => sum.Kolicina) + "",
                    Odobren = gk.Odobren,
                    Ocjena = _db.Ocjene.FirstOrDefault(w => w.GrupaKandidatiId == gk.GrupaKandidatiId),
                    Polozio = true   //TODO: zamjeniti sa stvarom vrijednoscu
                }).ToList()
            })
                .FirstOrDefaultAsync();

            if (model == null) { return NotFound(); }

            return View(model);
        }
        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Administrator, UlogeKorisnika.Direktor)]
        public async Task<IActionResult> QuickDetails(int id)
        {
            var o =await GetOsobaQuickDetailsVMQuery(id).FirstOrDefaultAsync();
            if (o == null)
            {
                return NotFound(new
                {
                    alert = new Alert(Severity.warning, "Nije pronađena", $"Osoba sa Id:{id} brojem")
                });
            }

            return PartialView("_QuickDetails", o);
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Administrator, UlogeKorisnika.Direktor)]
        public async Task<IActionResult> Delete(int id)
        {
            var o = await _db.Osobe.Include(i => i.Kandidat).Include(i => i.Zaposlenik).FirstOrDefaultAsync(f => f.OsobaId == id);
            string alertDetails = "";
            if (o == null)
            {
                return NotFound(new
                {
                    alert = new Alert(Severity.warning, "Nije pronađena", $"Osoba sa Id:{id} brojem")
                });
            }
            if (o.Kandidat != null || o.Zaposlenik != null)
            {
                alertDetails += $"Osoba: {o.Ime} {o.Prezime} -> Je ";
                alertDetails += o.Kandidat != null ? "kandidat; " : "";
                alertDetails += o.Zaposlenik != null ? "zaposlenik; " : "";
                return StatusCode(200, new
                {
                    alert = new Alert(Severity.warning, alertDetails, "Podatak nije izbrisan!")
                });
            }
            try
            {
                _db.Osobe.Remove(o);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }

            return Ok(new { alert = new Alert(Severity.success, "Je obrisana.", $"Osoba: {o.Ime} {o.Prezime}") });
        }
        [NonAction]
        private  IQueryable<OsobaVMs.OsobaQuickDetailsVM> GetOsobaQuickDetailsVMQuery(int id)
        {
            string randomLozinka = Autentifikacija.GenerateRandomPassword();
            return  _db.Osobe.AsNoTracking().Where(w => w.OsobaId == id).Select(s => new OsobaVMs.OsobaQuickDetailsVM()
            {
                OsobaId = s.OsobaId,
                Ime = s.Ime,
                Prezime = s.Prezime,
                DatumRodjenja = s.DatumRodjenja,
                MjestoRodjenjaId = s.MjestoRodjenjaId,
                MjestoRodjenjaNaziv = s.MjestoRodjenja.Naziv,
                Spol = s.Spol,
                Email = s.Email,
                MjestoBoravkaId = s.MjestoBoravkaId,
                MjestoBoravkaNaziv = s.MjestoBoravka.Naziv,
                Adresa = s.Adresa,
                BrojMobitela = s.BrojMobitela,
                BrojTelefona = s.BrojTelefona,
                KakoSteSaznaliZaNas = s.KakoSteSaznaliZaNas,
                PodatciOFirmi = s.PodatciOFirmi,
                NazivFirme = s.NazivFirme,
                AdresaFirme = s.AdresaFirme,
                GradFirmeId = s.GradFirmeId,
                GradFirmeNaziv = s.GradFirma.Naziv,
                BrojTelefonaFirme = s.BrojTelefonaFirme,
                EmailFirme = s.EmailFirme,

                ZaposlenikCreateVM = new ZaposlenikVMs.ZaposlenikCreateVM()
                {
                    OsobaId = s.OsobaId,
                    OsobaNaziv = s.Ime + " " + s.Prezime,
                    ZaposlenikVM = _db.Zaposlenici.Where(w => w.ZaposlenikId == s.OsobaId).Select(x => new ZaposlenikVMs.ZaposlenikVM()
                    {
                        ZaposlenikId = x.ZaposlenikId,
                        StepenObrazovanja = x.StepenObrazovanja,
                        BrojRacuna = x.BrojRacuna,
                        BrojLicneKarte = x.BrojLicneKarte
                    }).FirstOrDefault()
                },
                KandidatCreateVM = new KandidatVMs.KandidatCreateVM()
                {
                    OsobaId = s.OsobaId,
                    OsobaNaziv = s.Ime + " " + s.Prezime,
                    KandidatVM = _db.Kandidati.Where(w => w.KandidatId == s.OsobaId).Select(x => new KandidatVMs.KandidatVM()
                    {
                        KandidatId = x.KandidatId,
                        Biljeske = x.Biljeske,
                        DatumUpisa = x.DatumUpisa

                    }).FirstOrDefault()
                },
                KorisnickiNalog = s.KorisnickiNalog != null,
                KorisnickiNalogVM = s.KorisnickiNalog != null ? new KorisnickiNalogVM()
                {
                    KorisnickiNalogId = s.KorisnickiNalog.KorisnickiNalogId,
                    KorisnickoIme = s.KorisnickiNalog.KorisnickoIme,
                    UlogaKorisnika = s.KorisnickiNalog.UlogaKorisnika,
                    Aktivan = s.KorisnickiNalog.Aktivan,

                } : new KorisnickiNalogVM()
                {
                    OsobaId = s.OsobaId,
                    UlogaKorisnika = s.Zaposlenik != null ? UlogeKorisnika.AdministrativniRadnik : UlogeKorisnika.Kandidat,
                    KorisnickoIme = s.Ime + " " + s.Prezime,
                    Lozinka = randomLozinka,
                    Aktivan = true,
                    SendEmail = true,
                    EmailBody = MyEmailHelper.GetKorisnickiPodatciCreateEmailBody(new Osoba()
                    {
                        Ime = s.Ime,
                        Prezime = s.Prezime,
                        Spol = s.Spol
                    })
                }

            });
        }
    }
}