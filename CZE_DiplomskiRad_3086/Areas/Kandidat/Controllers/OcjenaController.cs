using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.Kandidat.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Areas.Kandidat.Controllers
{
    [Area(nameof(Areas.Kandidat))]
    [Autorizacija()]
    public class OcjenaController : Controller
    {
        private readonly CZEContext _db;

        public OcjenaController(CZEContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int id)
        {
            var gk = await _db.GrupaKandidati.FindAsync(id);
            if (gk == null || !gk.Odobren)
            {
                return BadRequest();
            }

            var o = await _db.Ocjene.Where(w => w.GrupaKandidatiId == id).SingleOrDefaultAsync();
            OcjenaVMs.OcjenaVM model = new OcjenaVMs.OcjenaVM() { GrupaKandidatiId = id }; ;
            if (o != null)
            {
                model.OcjenaId = o.OcjenaId;
                model.Opis = o.Opis;
                model.GrupaKandidatiId = o.GrupaKandidatiId;
                model.Vrijednost = o.Vrijednost;
                model.Silenced = o.Silenced;
            }

            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OcjenaVMs.OcjenaVM model)
        {
            ModelState.Remove("OcjenaId");
            if (!ModelState.IsValid)
            {
                return PartialView("Index", model);
            }

            Ocjena o = new Ocjena()
            {
                OcjenaId = model.OcjenaId,
                GrupaKandidatiId = model.GrupaKandidatiId,
                Vrijednost = model.Vrijednost,
                Opis = model.Opis,
                Silenced = model.Silenced,
            };
            try
            {
                if (model.OcjenaId != 0)
                {
                    _db.Ocjene.Attach(o);
                    _db.Entry(o).State = EntityState.Modified;
                }
                else
                {
                    _db.Ocjene.Add(o);
                }
                await _db.SaveChangesAsync();
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var o =await _db.Ocjene.FindAsync(id);
            if (o==null)
            {
                return NotFound(new { alert = new Alert(Severity.info, "Nije pronađena.", $"Ocjena Id:{id}") });
            }
            return PartialView("_Details", new OcjenaVMs.OcjenaVM()
            {
                OcjenaId = o.OcjenaId,
                Vrijednost = o.Vrijednost,
                Opis = o.Opis,
                GrupaKandidatiId = o.GrupaKandidatiId,
                Silenced = o.Silenced
            });
        }
        [Autorizacija(UlogeKorisnika.Administrator,UlogeKorisnika.Direktor,UlogeKorisnika.AdministrativniRadnik)]
        public async Task<IActionResult> Silence(int id)
        {
            var o = await _db.Ocjene.FindAsync(id);
            if (o == null)
            {
                return NotFound(new { alert = new Alert(Severity.info, "Nije pronađena.", $"Ocjena Id:{id}") });
            }
            o.Silenced = !o.Silenced;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id});
        }
    }
}