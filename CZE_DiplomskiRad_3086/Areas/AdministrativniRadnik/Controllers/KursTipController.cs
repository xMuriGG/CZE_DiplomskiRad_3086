using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Areas.AdministrativniRadnik.Controllers
{
    [Area(nameof(CZE.Web.Areas.AdministrativniRadnik))]
    [Autorizacija(UlogeKorisnika.Predavac,UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]

    public class KursTipController : Controller
    {
        private readonly CZEContext _db;

        public KursTipController(CZEContext db)
        {
            _db = db;
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Create(int? id,int? kursKategorijaId)
        {
            KursVMs.KursTipCreateVM model = new KursVMs.KursTipCreateVM()
            {
                KursKategorije = await _db.KursKategorije.AsNoTracking().Select(s=>new SelectListItem()
                {
                    Text = s.Naziv,
                    Value = s.KursKategorijaId.ToString(),
                    Selected = (kursKategorijaId ?? 0)==s.KursKategorijaId
                }).ToListAsync()
            };
            if (id!=null && _db.KursTipovi.Any(a => a.KursTipId == id))
            {
                var kt = await _db.KursTipovi.FindAsync(id);
                model.KursTipVm =new KursVMs.KursTipVM()
                {
                    KursTipId = kt.KursTipId,
                    KursKategorijaId = kt.KursKategorijaId,
                    Naziv = kt.Naziv,
                    Casova = kt.Casova,
                    Cijena = kt.Cijena,
                    Opis = kt.Opis
                };
            }

            return PartialView("_Create",model);
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursVMs.KursTipCreateVM model)
        {
            ModelState.Remove("KursTipVm.KursTipId");
            if (!ModelState.IsValid)
            {
                model.KursKategorije = await _db.KursKategorije.AsNoTracking().Select(s => new SelectListItem()
                {
                    Text = s.Naziv,
                    Value = s.KursKategorijaId.ToString()
                }).ToListAsync();
                return PartialView("_Create", model);
            }
            try
            {
                KursTip kt=new KursTip()
                {
                    KursTipId = model.KursTipVm.KursTipId,
                    Naziv = model.KursTipVm.Naziv,
                    Cijena = model.KursTipVm.Cijena,
                    Casova = model.KursTipVm.Casova,
                    Opis = model.KursTipVm.Opis,
                    KursKategorijaId = model.KursTipVm.KursKategorijaId
                };
                if (kt.KursTipId!=0)
                {
                    _db.KursTipovi.Attach(kt);
                    _db.Entry(kt).State=EntityState.Modified;
                }
                else
                {
                    _db.KursTipovi.Add(kt);
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }

            return RedirectToAction("GetKursTipTable");
        }

        public async Task<IActionResult> GetKursTipTable(int? kursKategorijaId)
        {
            var query = _db.KursTipovi.AsNoTracking();
            query = kursKategorijaId ==null ?
                query.OrderBy(o => o.KursKategorija.Naziv).ThenBy(o => o.Naziv) : 
                query.Where(w => w.KursKategorijaId == kursKategorijaId);
            query = query.Include(i => i.KursKategorija).AsNoTracking();
            return PartialView("_KursTipTable", await query.ToListAsync());
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Delete(int id)
        {
            KursTip kt = await _db.KursTipovi.FindAsync(id);
            if (kt == null)
            {
                return NotFound("Zapis nije pronađen.");
            }
            try
            {
                _db.KursTipovi.Remove(kt);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisan. Tip vjerovatno ima kurs vezan za grupu.", $"Tip kursa: {kt.Naziv}")
                });
            }
            return RedirectToAction("GetKursTipTable");
        }
    }
}