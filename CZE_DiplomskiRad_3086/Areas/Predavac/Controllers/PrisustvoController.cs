using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using CZE.Web.Areas.Predavac.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Areas.Predavac.Controllers
{
    [Area(nameof(CZE.Web.Areas.Predavac))]
    [Autorizacija()]
    public class PrisustvoController : Controller
    {
        private readonly CZEContext _db;
        public PrisustvoController(CZEContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> PrisustvaIndex(int id)
        {
            var model = await GetPrisustva(id);

            return PartialView("_PrisustvaIndex", model);
        }

        [NonAction]
        private async Task<PrisustvaVMs.PrisustvoIndexVM> GetPrisustva(int id)
        {
            return new PrisustvaVMs.PrisustvoIndexVM
            {
                PrisustvoTableVM = new PrisustvaVMs.PrisustvoTableVM
                {
                    KandidatiPrisustvoVM = await _db.GrupaKandidati.AsNoTracking()
                        .Where(w => w.GrupaId == id).Select(s => new PrisustvaVMs.KandidatPrisustvoVM()
                        {
                            KandidatId = s.KandidatId,
                            OsobaNaziv = s.Kandidat.Osoba.Ime + " " + s.Kandidat.Osoba.Prezime,
                            GrupaKandidatiId = s.GrupaKandidatiId
                        }).ToListAsync(),
                    GrupaId = id,
                    PrisustvoTerminiVM = await _db.PrisustvoTermini.AsNoTracking()
                        .Where(w => w.GrupaId == id)
                        //.OrderBy(o => o.PrisustvoTermin.DateTime)
                        //.GroupBy(g => g.PrisustvoTermin)
                        //.Select(s=>new{termin=s.Key,prisustva=s.OrderBy(o=>o.GrupaKandidati.KandidatId)});
                        .Select(s => new PrisustvaVMs.PrisustvoTerminVM()
                        {
                            PrisustvoTerminId = s.PrisustvoTerminId,
                            Casova = s.Casova,
                            DateTime = s.DateTime,
                            GrupaId = s.GrupaId,
                            Prisustva = s.Prisustva
                        })
                        .OrderByDescending(o => o.DateTime).ToListAsync()
                },
                PrisustvoTerminCreateVM = new PrisustvaVMs.PrisustvoTerminCreateVM() {GrupaId = id}
            };

            //return new PrisustvaVMs.PrisustvoIndexVM
            //{
            //    PrisustvoTableVM = new PrisustvaVMs.PrisustvoTableVM
            //    {
            //        KandidatiPrisustvoVM = await _db.GrupaKandidati.AsNoTracking()
            //            .Where(w => w.GrupaId == id).Select(s => new PrisustvaVMs.KandidatPrisustvoVM()
            //            {
            //                KandidatId = s.KandidatId,
            //                OsobaNaziv = s.Kandidat.Osoba.Ime + " " + s.Kandidat.Osoba.Prezime,
            //                GrupaKandidatiId = s.GrupaKandidatiId
            //            }).ToListAsync(),
            //        GrupaId = id,
            //        PrisustvoTerminiVM = await _db.Prisustva.AsNoTracking()
            //            .Where(w => w.PrisustvoTermin.GrupaId == id)
            //            //.OrderBy(o => o.PrisustvoTermin.DateTime)
            //            .GroupBy(g => g.PrisustvoTermin)
            //            //.Select(s=>new{termin=s.Key,prisustva=s.OrderBy(o=>o.GrupaKandidati.KandidatId)});
            //            .Select(s => new PrisustvaVMs.PrisustvoTerminVM()
            //            {
            //                PrisustvoTerminId = s.Key.PrisustvoTerminId,
            //                Casova = s.Key.Casova,
            //                DateTime = s.Key.DateTime,
            //                GrupaId = s.Key.GrupaId,
            //                Prisustva = s.ToList()
            //            })
            //            .OrderByDescending(o => o.DateTime).ToListAsync()
            //    },
            //    PrisustvoTerminCreateVM = new PrisustvaVMs.PrisustvoTerminCreateVM() { GrupaId = id }
            //};
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrisustvaVMs.PrisustvoTerminCreateVM model)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 202;
                return PartialView("_Create", model);
            }
            try
            {
                PrisustvoTermin t = new PrisustvoTermin()
                {
                    PrisustvoTerminId = model.PrisustvoTerminVM.PrisustvoTerminId,
                    DateTime = model.PrisustvoTerminVM.DateTime,
                    GrupaId = model.GrupaId,
                    Casova = model.PrisustvoTerminVM.Casova
                };
                _db.PrisustvoTermini.Add(t);

                var grupaKandidati = _db.GrupaKandidati.Where(w => w.GrupaId == model.GrupaId);

                var prisustva = grupaKandidati.Select(s => new Prisustvo()
                {
                    GrupaKandidatiId = s.GrupaKandidatiId,
                    PrisustvoTermin = t,
                    Prisutan = false
                }).ToList();
                _db.Prisustva.AddRange(prisustva);

                await _db.SaveChangesAsync();

                //return RedirectToAction(nameof(PrisustvaIndex),new{id=model.GrupaId});
                Response.StatusCode = 201;
                return PartialView("_PrisustvaIndex", await GetPrisustva(model.GrupaId));
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
        }

        public async Task<IActionResult> PrisustvoStatusChange(int prisustvoId, int grupaId)
        {
            var prisustvo = _db.Prisustva.Find(prisustvoId);
            if (prisustvo != null)
            {
                prisustvo.Prisutan = !prisustvo.Prisutan;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("PrisustvaIndex", new { id = grupaId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var prisustvo = _db.PrisustvoTermini.Find(id);

            if (prisustvo == null)
            {
                return NotFound();
            }
            int grupaId = prisustvo.GrupaId;
            try
            {
                _db.PrisustvoTermini.Remove(prisustvo);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }


            return RedirectToAction("PrisustvaIndex", new { id = grupaId });
        }
    }
}