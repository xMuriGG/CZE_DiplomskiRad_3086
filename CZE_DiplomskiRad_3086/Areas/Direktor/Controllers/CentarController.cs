using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.Direktor.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Areas.Direktor.Controllers
{
    [Area(nameof(Areas.Direktor))]
    [Autorizacija(UlogeKorisnika.Predavac,UlogeKorisnika.AdministrativniRadnik,UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
    public class CentarController : Controller
    {
        private readonly CZEContext _db;

        public CentarController(CZEContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _db.Centri.Select(s => new CentarVMs.CentarListItemVM()
            {
                CentarId = s.CentarId,
                Naziv = s.Naziv,
                Adresa = s.Adresa,
                BrojTelefona = s.BrojTelefona,
                BrojMobitela = s.BrojMobitela,
                Email = s.Email,
                Longitude = s.Longitude,
                Latitude = s.Latitude,
                Grad = s.Grad.Naziv,
                GradId = s.GradId
            }).ToListAsync();     
                return View(model);           
        }
        [Autorizacija(UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]

        public async Task<IActionResult> Create(int? id)
        {
            Centar c = null;
            if (id != null && id != 0)
            {
                c = await _db.Centri.FindAsync(id);
            }
            int gradId = c?.GradId ?? 0;
            var model = new CentarVMs.CentarCreateVM()
            {
                CentarVM = null,
                Gradovi = await _db.Gradovi.Select(s => new SelectListItem()
                {
                    Text = s.Naziv,
                    Value = s.GradId.ToString(),
                    Selected = s.GradId == gradId
                }).ToListAsync()
            };
            if (c != null)
            {
                model.CentarVM = new CentarVMs.CentarVM()
                {
                    CentarId = c.CentarId,
                    Naziv = c.Naziv,
                    Email = c.Email,
                    Adresa = c.Adresa,
                    BrojTelefona = c.BrojTelefona,
                    BrojMobitela = c.BrojMobitela,
                    GradId = c.GradId,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude

                };
            }

            return PartialView(model);
        }

        [Autorizacija(UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        [HttpPost]
        public async Task<IActionResult> Create(CentarVMs.CentarCreateVM model)
        {
            ModelState.Remove("CentarVM.CentarId");
            if (!ModelState.IsValid)
            {
                model.Gradovi = await _db.Gradovi.Select(s => new SelectListItem()
                {
                    Text = s.Naziv,
                    Value = s.GradId.ToString(),
                    Selected = s.GradId == model.CentarVM.GradId
                }).ToListAsync();
                return PartialView(model);
            }

            Centar c = new Centar()
            {
                CentarId = model.CentarVM.CentarId,
                Naziv = model.CentarVM.Naziv,
                Adresa = model.CentarVM.Adresa,
                BrojTelefona = model.CentarVM.BrojTelefona,
                BrojMobitela = model.CentarVM.BrojMobitela,
                Email = model.CentarVM.Email,
                Longitude = model.CentarVM.Longitude,
                Latitude = model.CentarVM.Latitude,
                GradId = model.CentarVM.GradId
            };
            try
            {
                if (c.CentarId != 0)
                {
                    _db.Centri.Attach(c);
                    _db.Entry(c).State = EntityState.Modified;
                }
                else
                {
                    _db.Centri.Add(c);
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        [Autorizacija(UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Delete(int id)
        {
            var c =await _db.Centri.FindAsync(id);
            if (c==null)
            {
                return NotFound("Centar nije pronađen u bazi.");
            }
            try
            {
                _db.Centri.Remove(c);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Centar nije izbrisan(moguće da centar ima poveznicu)." + e.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}