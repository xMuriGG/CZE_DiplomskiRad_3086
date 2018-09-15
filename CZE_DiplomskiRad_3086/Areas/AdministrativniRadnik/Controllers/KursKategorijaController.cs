using System;
using System.Collections.Generic;
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
    [Autorizacija(UlogeKorisnika.Predavac,UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]

    public class KursKategorijaController : Controller
    {
        private CZEContext _db;

        public KursKategorijaController(CZEContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetKursKategorijaTable(int? id)
        {
            return PartialView("_KursKategorijaTable", await _db.KursKategorije.AsNoTracking().OrderBy(o => o.Naziv).ToListAsync());
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Create(int? id)
        {
            KursKategorija kursKategorija = null;
            if (id != null && _db.KursKategorije.Any(a => a.KursKategorijaId == id))
            {
                kursKategorija = await _db.KursKategorije.FirstOrDefaultAsync(f => f.KursKategorijaId == id);
            }
            KursVMs.KursKategorijaVM kkVm = kursKategorija != null ? new KursVMs.KursKategorijaVM()
            {
                KursKategorijaId = kursKategorija.KursKategorijaId,
                Naziv = kursKategorija.Naziv
            } : null;
            return PartialView("_Create", kkVm);

        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursVMs.KursKategorijaVM model)
        {
            ModelState.Remove("KursKategorijaId");
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }
            try
            {
                KursKategorija kk = new KursKategorija() {KursKategorijaId = model.KursKategorijaId, Naziv = model.Naziv };
                if (model.KursKategorijaId != 0)
                {
                    _db.KursKategorije.Attach(kk);
                    _db.Entry(kk).State = EntityState.Modified;
                }
                else
                {
                    _db.KursKategorije.Add(kk);
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }

            return RedirectToAction("GetKursKategorijaTable");
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Delete(int id)
        {
            KursKategorija kk =await _db.KursKategorije.FindAsync(id);
            if (kk == null)
            {
                return NotFound("Zapis nije pronađen.");
            }
            try
            {
                _db.KursKategorije.Remove(kk);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisana. Kategorija vjerovatno ima kurs vezan za grupu.", $"Kategorija kursa: {kk.Naziv}")
                });
            }

            return RedirectToAction("GetKursKategorijaTable");
        }

    }
}