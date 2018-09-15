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
    [Autorizacija(UlogeKorisnika.AdministrativniRadnik,UlogeKorisnika.Direktor,UlogeKorisnika.Administrator)]
    public class UplataKandidataController : Controller
    {
        private readonly CZEContext _db;
        public UplataKandidataController(CZEContext db)
        {
            _db = db;
        }

        public IActionResult Create(int? id, int grupaKandidatiId)
        {
            var model = new UplataKandidataVMs.UplataKandidataCreateVM()
            {
                GrupaKandidatiId = grupaKandidatiId
            };
            if (id != null && id != 0)
            {
                var uk = _db.UplateKandidata.Find(id);
                if (uk != null)
                {
                    model.UplataKandidataVM = new UplataKandidataVMs.UplataKandidataVM()
                    {
                        UplataKandidataId = uk.UplataKandidataId,
                        ZaposlenikId = uk.ZaposlenikId,
                        Kolicina = uk.Kolicina,
                        GrupaKandidatiId = uk.GrupaKandidatiId,
                        Biljeske = uk.Biljeske,
                        DatumUplate = uk.DatumUplate
                    };
                    model.GrupaKandidatiId = uk.GrupaKandidatiId;
                }
            }
          

            return PartialView("_Create", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UplataKandidataVMs.UplataKandidataCreateVM model)
        {
            ModelState.Remove("UplataKandidataVM.UplataKandidataId");
            ModelState.Remove("UplataKandidataVM.ZaposlenikId");
            if (!ModelState.IsValid) return PartialView("_Create", model);
            try
            {
                var uk=new UplataKandidata()
                {
                    UplataKandidataId = model.UplataKandidataVM.UplataKandidataId,
                    Kolicina = model.UplataKandidataVM.Kolicina,                       
                    Biljeske = model.UplataKandidataVM.Biljeske,
                    GrupaKandidatiId = model.GrupaKandidatiId,
                    ZaposlenikId =(await HttpContext.GetLogiraniKorisnik()).KorisnickiNalogId
                };
                if (model.UplataKandidataVM.DatumUplate!=null)
                {
                    uk.DatumUplate = (DateTime) model.UplataKandidataVM.DatumUplate;
                }
                if (model.UplataKandidataVM.UplataKandidataId!=0)
                {
                    _db.UplateKandidata.Attach(uk);
                    _db.Entry(uk).State=EntityState.Modified;
                }
                else
                {
                    _db.UplateKandidata.Add(uk);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("UplateKandidataTable", new {grupaKandidatiId = model.GrupaKandidatiId});
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
        }
        public async Task<IActionResult> UplateKandidataTable(int grupaKandidatiId)
        {
            var temp = await _db.GrupaKandidati.AsNoTracking()
                .Where(w => w.GrupaKandidatiId == grupaKandidatiId)
                .Select(s => new
                {
                    grupaId = s.GrupaId,
                    grupaKandidatiInfo = s.Grupa.Kurs.KursTip.Naziv + "->" +
                                     s.Grupa.Kurs.Naziv + "->" +
                                     s.Grupa.Naziv,
                    kandidatInfo = s.Kandidat.Osoba.Ime + " " + s.Kandidat.Osoba.Prezime
                }).FirstOrDefaultAsync();


            if (temp == null)
            {
                return NotFound();
            }
            var model = new UplataKandidataVMs.UplataKandidataTableVM()
            {
                UplateKandidata = await _db.UplateKandidata.AsNoTracking()
                        .Where(w => w.GrupaKandidatiId == grupaKandidatiId)
                        .Select(s => new UplataKandidataVMs.UplataKandidataVM()
                        {
                            UplataKandidataId = s.UplataKandidataId,
                            ZaposlenikId = s.ZaposlenikId,
                            GrupaKandidatiId = s.GrupaKandidatiId,
                            DatumUplate = s.DatumUplate,
                            Kolicina = s.Kolicina,
                            Biljeske = s.Biljeske
                        })
                        .ToListAsync(),
                GrupaKandidatiId = grupaKandidatiId,
                GrupaId = temp.grupaId,
                GrupaKandidatiInfo = temp.grupaKandidatiInfo,
                KandidatInfo = temp.kandidatInfo
            };


            return PartialView("_Index", model);
        }

        [Autorizacija(UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]

        public async Task<IActionResult> Delete(int id)
        {
            var uplata =await _db.UplateKandidata.FindAsync(id);
            if (uplata == null) return NotFound();
            try
            {
                _db.UplateKandidata.Remove(uplata);
                await _db.SaveChangesAsync();
                return RedirectToAction("UplateKandidataTable", new { grupaKandidatiId = uplata.GrupaKandidatiId });
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
        }
    }
}