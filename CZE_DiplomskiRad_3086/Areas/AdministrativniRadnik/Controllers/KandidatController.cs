using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Areas.AdministrativniRadnik.Controllers
{
    [Area(nameof(CZE.Web.Areas.AdministrativniRadnik))]
    [Autorizacija()]
    public class KandidatController : Controller
    {
        private CZEContext _db;

        public KandidatController(CZEContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Create(int? id)
        {
            Osoba o = null;
            KandidatVMs.KandidatCreateVM model = null;
            if (id != null && id != 0)
            {
                o = await _db.Osobe.Include(i => i.Kandidat).FirstOrDefaultAsync(f => f.OsobaId == id);
            }
            if (o == null) return NotFound();

            model = new KandidatVMs.KandidatCreateVM()
            {
                OsobaId = o.OsobaId,
                OsobaNaziv = o.Ime + " " + o.Prezime
            };
            if (o.Kandidat != null)
            {
                model.KandidatVM = new KandidatVMs.KandidatVM()
                {
                    KandidatId = o.Kandidat.KandidatId,
                    Biljeske = o.Kandidat.Biljeske,
                };
            }
            return PartialView("_Create", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KandidatVMs.KandidatCreateVM model)
        {
            //string name = $"{nameof(model.KandidatVM)}.{nameof(model.KandidatVM.Biljeske)}";
            //ModelState.AddModelError("", "Error");
            ModelState.Remove("KandidatVM.KandidatId");
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }
            try
            {
                string detailsMessage = "Uspješno ";
                int id = model.KandidatVM.KandidatId != 0 ? model.KandidatVM.KandidatId : model.OsobaId;
                Data.Models.Kandidat k = new Data.Models.Kandidat() { KandidatId = id, Biljeske = model.KandidatVM.Biljeske };
                if (model.KandidatVM.KandidatId != 0)
                {
                    _db.Kandidati.Attach(k);
                    var entry = _db.Entry(k);
                    entry.State = EntityState.Modified;
                    entry.Property(p => p.DatumUpisa).IsModified = false;
                    detailsMessage += "editovana.";
                }
                else
                {
                    _db.Kandidati.Add(k);
                    detailsMessage += "promovisana u kandidata.";
                }
                _db.SaveChanges();


                return RedirectToAction("QuickDetails", "Osoba", new { id = k.KandidatId });
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }

        }

        public async Task<IActionResult> Details(int id)
        {
            var kandidat = await _db.Kandidati.FindAsync(id);
            if (kandidat == null) { return NotFound(new { alert = new Alert(Severity.warning, "Nije pronađen", $"Kandidat sa Id:{id} brojem") }); }
            var model = new KandidatVMs.KandidatVM
            {
                KandidatId = kandidat.KandidatId,
                DatumUpisa = kandidat.DatumUpisa,
                Biljeske = kandidat.Biljeske
            };
            return PartialView("_Details", model);
        }
        [HttpPost]
        public async Task<IActionResult> GetKandidateDataTable(int grupaId, int draw, int start, int length, [FromForm(Name = "search[value]")] string search)
        {
            //var query = _db.Kandidati
            //    .Select(s => new KandidatVMs.KandidatListItemVM()
            //    {
            //        KandidatId = s.KandidatId,
            //        DatumUpisa = s.DatumUpisa.ToString("M/d/yy"),
            //        ImePrezime = s.Osoba.Ime + " " + s.Osoba.Prezime,
            //        Email = s.Osoba.Email,
            //        DatumRodjenja = s.Osoba.DatumRodjenja.ToString("M/d/yy")                   
            //    });

            //novi query gdje se vračaju samo oni kandidati koji več nisu u grupi
            var query = from k in _db.Kandidati
                        join gk in _db.GrupaKandidati.Where(w => w.GrupaId == grupaId) on k.KandidatId equals gk.KandidatId into
                        tempTable
                        from tt in tempTable.DefaultIfEmpty()
                        where tt == null
                        select new KandidatVMs.KandidatListItemVM()
                        {
                            KandidatId = k.KandidatId,
                            DatumUpisa = k.DatumUpisa.ToString("M/d/yy"),
                            ImePrezime = k.Osoba.Ime + " " + k.Osoba.Prezime,
                            Email = k.Osoba.Email,
                            DatumRodjenja = k.Osoba.DatumRodjenja.ToString("M/d/yy"),
                        };

            var total = await query.CountAsync();
            var recordsFiltered = total;
            //text filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(
                    w => w.ImePrezime.Contains(search)
                );
                recordsFiltered = await query.CountAsync();
            }

            query = query.Skip(start).Take(length);

            return Json(new
            {
                draw = draw,
                recordsTotal = total,
                recordsFiltered = recordsFiltered,
                data = await query.ToListAsync()
            });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.Kandidati.Where(w => w.KandidatId == id).Select(s => new
            {
                kandidat = s,
                zaposlenik = s.Osoba.Zaposlenik != null,
                kNalog = s.Osoba.KorisnickiNalog != null
            }).SingleOrDefaultAsync();
            if (model == null)
            {
                return NotFound(new
                {
                    alert = new Alert(Severity.warning, "Nije pronađen.", $"Kandidat sa Id:{id} brojem")
                });
            }
            if (!model.zaposlenik && model.kNalog)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisan. Kandidat ima korisnički nalog.", $"Kandidat sa Id:{id} brojem")
                });
            }
            try
            {
                _db.Kandidati.Remove(model.kandidat);
                await _db.SaveChangesAsync();
                return RedirectToAction("QuickDetails", "Osoba", new { id });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisan. Kandidat je vjerovatno u nekoj grupi.", $"Kandidat sa Id:{id} brojem")
                });
            }
        }
    }
}