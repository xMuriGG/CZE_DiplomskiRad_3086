using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    [Autorizacija(UlogeKorisnika.Predavac,UlogeKorisnika.AdministrativniRadnik,UlogeKorisnika.Direktor,UlogeKorisnika.Administrator)]
    public class KursController : Controller
    {
        private readonly CZEContext _db;
        public KursController(CZEContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            KursVMs.IndexVM model =
                new KursVMs.IndexVM
                {
                    KursKategorije = await _db.KursKategorije.AsNoTracking().OrderBy(o => o.Naziv).ToListAsync(),
                    KursTipovi = await _db.KursTipovi.AsNoTracking().OrderBy(o => o.KursKategorija.Naziv).ThenBy(o => o.Naziv)
                                            .Include(i => i.KursKategorija).AsNoTracking().ToListAsync()
                };
            return View(model);
        }
        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Create(int? id, int? kursTipId)
        {
            KursVMs.KursCreateVM model = new KursVMs.KursCreateVM()
            {
                KursTipovi = await _db.KursTipovi.AsNoTracking().Select(s => new SelectListItem()
                {
                    Text = s.Naziv,
                    Value = s.KursTipId.ToString(),
                    Selected = (kursTipId ?? 0) == s.KursTipId
                }).ToListAsync()
            };

            model.KursTipId = kursTipId ?? 0;

            if (id != null && _db.Kursevi.Any(a => a.KursId == id))
            {
                var k = await _db.Kursevi.FindAsync(id);

                model.KursVM = new KursVMs.KursVM()
                {
                    KursId = k.KursId,
                    KursTipId = k.KursTipId,
                    Naziv = k.Naziv,
                    Opis = k.Opis
                };
            }

            return PartialView("_Create", model);
        }
        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursVMs.KursCreateVM model)
        {
            ModelState.Remove("KursVM.KursId");
            if (!ModelState.IsValid)
            {
                model.KursTipovi = await _db.KursTipovi.AsNoTracking().Select(s => new SelectListItem()
                {
                    Text = s.Naziv,
                    Value = s.KursTipId.ToString(),
                    Selected = model.KursVM.KursTipId == s.KursTipId
                }).ToListAsync();
                return PartialView("_Create", model);
            }
            Kurs k = new Kurs()
            {
                KursId = model.KursVM.KursId,
                KursTipId = model.KursVM.KursTipId,
                Naziv = model.KursVM.Naziv,
                Opis = model.KursVM.Opis
            };
            try
            {
               
                if (k.KursId!=0)
                {
                    _db.Kursevi.Attach(k);
                    _db.Entry(k).State=EntityState.Modified;
                }
                else
                {
                    _db.Kursevi.Add(k);          
                }
                await _db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
            return RedirectToAction("KursTable",new{ kursTipId =k.KursTipId});
        }
     
        public async Task<IActionResult> KursTable(int? kursTipId)
        {
            KursTip kursTip = null;
            if (kursTipId != null)
            {
                kursTip = await _db.KursTipovi.FirstOrDefaultAsync(f => f.KursTipId == kursTipId);
            }
            KursVMs.KursTablePartial model = null;
            if (kursTip != null)
            {
                model =
                   new KursVMs.KursTablePartial()
                   {
                       KursTipId = kursTip.KursTipId,
                       KursTipNaziv = kursTip.Naziv,
                       Kursevi = await _db.Kursevi.Where(w => w.KursTipId == kursTipId).AsNoTracking().ToListAsync()
                   };

            }
            return PartialView("_KursTable", model);
           
        }

        public async Task<IActionResult> Details(int id)
        {
            var src = await _db.Kursevi.Include(i => i.KursTip).Include(i => i.KursTip.KursKategorija)
                .FirstAsync(f => f.KursId == id);
            if (src==null)
            {
                return NotFound($"Ne postoji zapis kursa sa Id brojem{id}");
            }
            var model=new KursVMs.DetailsVM()
            {
                KursKategorijaVM =new KursVMs.KursKategorijaVM()
                {
                    KursKategorijaId = src.KursTip.KursKategorija.KursKategorijaId,
                    Naziv = src.KursTip.KursKategorija.Naziv
                },
                KursTipVM = new KursVMs.KursTipVM()
                {
                    KursTipId = src.KursTip.KursTipId,
                    KursKategorijaId = src.KursTip.KursKategorijaId,
                    Naziv = src.KursTip.Naziv,
                    Opis = src.KursTip.Opis,
                    Casova = src.KursTip.Casova,
                    Cijena = src.KursTip.Cijena
                },
                KursVM = new KursVMs.KursVM()
                {
                    KursId = src.KursId,
                    KursTipId = src.KursTipId,
                    Naziv = src.Naziv,
                    Opis = src.Opis
                }

            };

            return PartialView("_Details",model) ;
        }
        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Delete(int id)
        {
            Kurs k = await _db.Kursevi.FindAsync(id);
            if (k == null)
            {
                return NotFound("Zapis nije pronađen.");
            }
            int kursTipId = k.KursTipId;
            try
            {
                _db.Kursevi.Remove(k);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisan. Kurs je vjerovatno vezan za grupu.", $"Kurs: {k.Naziv}")
                });
            }
            return RedirectToAction("KursTable", new { kursTipId = kursTipId });
        }
    }
}