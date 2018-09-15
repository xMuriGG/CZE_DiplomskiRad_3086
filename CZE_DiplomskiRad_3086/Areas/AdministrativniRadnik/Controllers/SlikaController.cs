using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CZE.Web.Areas.AdministrativniRadnik.Controllers
{
    [Area(nameof(CZE.Web.Areas.AdministrativniRadnik))]
    [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
    public class SlikaController : Controller
    {
        private readonly CZEContext _db;
        private readonly IConfiguration _config;

        public SlikaController(CZEContext db, IConfiguration config1)
        {
            _db = db;
            _config = config1;
        }

        public IActionResult Index()
        {
            ////Install-Package CoreCompat.System.Drawing -Version 1.0.0-beta006
            ////https://stackoverflow.com/questions/33344200/manipulating-images-with-net-core
            return View();
        }

        public async Task<IActionResult> Create(int? id)
        {
            GrupaVMs.SlikaCreateVM model = new GrupaVMs.SlikaCreateVM();
            if (id != null && id != 0)
            {
                var s = await _db.Slike.FindAsync(id);
                if (s != null)
                {
                    model.SlikaVM = new GrupaVMs.SlikaVM()
                    {
                        SlikaId = s.SlikaId,
                        Naziv = s.Naziv,
                        KursKategorijaId = s.KursKategorijaId,
                        SlikaUrl = s.SlikaUrl
                    };
                }
            }

            model.KursKategorije = await _db.KursKategorije.AsNoTracking().Select(s => new SelectListItem()
            {
                Text = s.Naziv,
                Value = s.KursKategorijaId.ToString()

            }).ToListAsync();

            return PartialView("_Create", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GrupaVMs.SlikaCreateVM model)
        {
            ModelState.Remove("SlikaVM.SlikaId");
            if (ModelState.IsValid)
            {
                if (model.SlikaVM.SlikaId == 0 && model.SlikaVM.SlikaFile == null && string.IsNullOrEmpty(model.SlikaVM.SlikaUrl))
                {
                    ModelState.AddModelError("", "Morate unijeti ili url slike ili sliku.");
                }
                else
                {
                    int destWidth = _config.GetValue<int>("MySettings:imgWidth"), destHeight = _config.GetValue<int>("MySettings:imgHeight");
                    try
                    {
                        Slika s = new Slika()
                        {
                            SlikaId = model.SlikaVM.SlikaId,
                            Naziv = model.SlikaVM.Naziv,
                            KursKategorijaId = model.SlikaVM.KursKategorijaId,
                            SlikaUrl = model.SlikaVM.SlikaUrl,//TODO:Provjeriti da li su unesena oba urla ili ukinuti url
                        };
                        if (model.SlikaVM.SlikaFile != null)
                        {
                            if (model.SlikaVM.SlikaFile.Length < 1048576)
                            {
                                Image image = Image.FromStream(model.SlikaVM.SlikaFile.OpenReadStream(), true, true);
                                image = image.Resize(destWidth, destHeight).Crop(destWidth, destHeight);
                                s.SlikaFile = image.ToByteArray();
                                Image thumbNail = image.GetThumbnailImage(100, 100, null, new IntPtr());
                                s.SlikaThumb = thumbNail.ToByteArray();
                            }
                            else
                            {
                                ModelState.AddModelError("SlikaVM.SlikaFile", "Slika mora biti veličine do 1MB.");
                            }
                        }

                        if (ModelState.IsValid)
                        {
                            string alertDetails = "Uspješno ";
                            string alertSummary = $"Slika: {s.Naziv} ";
                            if (s.SlikaId == 0)
                            {
                                _db.Slike.Add(s);
                                alertDetails += "dodana u bazu.";
                            }
                            else
                            {
                                _db.Slike.Attach(s);
                                var entry = _db.Entry(s);
                                entry.State = EntityState.Modified;
                                if (s.SlikaFile == null)
                                {
                                    entry.Property(p => p.SlikaFile).IsModified = false;
                                    entry.Property(p => p.SlikaThumb).IsModified = false;
                                }
                                alertDetails = "editovana.";
                            }
                            _db.SaveChanges();
                            return StatusCode(201, new { Alert = new Alert(Severity.success, alertDetails, alertSummary) });
                        }                       
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
                    }
                }

            }


            model.KursKategorije = await _db.KursKategorije.AsNoTracking().Select(s => new SelectListItem()
            {
                Text = s.Naziv,
                Value = s.KursKategorijaId.ToString(),
                Selected = s.KursKategorijaId == model.SlikaVM.KursKategorijaId

            }).ToListAsync();
            return PartialView("_Create", model);
        }

        public async Task<IActionResult> GetSlikeDataTable(int draw, int start, int length,
            [FromForm(Name = "search[value]")] string search,
            [FromForm(Name = "order[0][column]")] int? orderColumn0,
            [FromForm(Name = "order[0][dir]")] string orderDir0,
            [FromForm(Name = "order[1][column]")] int? orderColumn1,
            [FromForm(Name = "order[1][dir]")] string orderDir1)
        {

            var query = _db.Slike.AsNoTracking().Select(s => new GrupaVMs.SlikaListItemVM()
            {
                SlikaId = s.SlikaId,
                Naziv = s.Naziv,
                KursKategorijaId = s.KursKategorijaId,
                KursKategorijaNaziv = s.KursKategorija.Naziv,
                SlikaUrl = s.SlikaUrl,
                TezinaSlike = s.SlikaFile != null ? s.SlikaFile.Length / 1024 : 0,
                SlikaThumb = s.SlikaThumb != null ? Convert.ToBase64String(s.SlikaThumb) : string.Empty
            });
            int total, recordsFiltered;
            total = await query.CountAsync();
            recordsFiltered = total;
            //text filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(
                    w => w.Naziv.Contains(search) ||
                         w.KursKategorijaNaziv.StartsWith(search)
                );
                recordsFiltered = await query.CountAsync();
            }

            //ordering
            IOrderedQueryable<GrupaVMs.SlikaListItemVM> ordering = query.OrderBy(o => 0);
            if (string.IsNullOrEmpty(orderDir0))
            {
                ordering = query.OrderByDescending(o => o.SlikaId);
            }
            if (orderColumn0 != null)
            {
                switch (orderColumn0)
                {
                    case 1:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.Naziv) : ordering.OrderByDescending(o => o.Naziv);
                        break;

                    case 2:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.KursKategorijaNaziv) : ordering.OrderByDescending(o => o.KursKategorijaNaziv);
                        break;

                }
            }
            if (orderColumn1 != null)
            {
                switch (orderColumn1)
                {
                    case 1:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.Naziv) : ordering.ThenByDescending(o => o.Naziv);
                        break;

                    case 2:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.KursKategorijaNaziv) : ordering.ThenByDescending(o => o.KursKategorijaNaziv);
                        break;
                }
            }


            query = ordering.Skip(start).Take(length);

            var list = await query.ToListAsync();

            return Json(new
            {
                draw = draw,
                recordsTotal = total,
                recordsFiltered = recordsFiltered,
                data = list
            });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var slika = await _db.Slike.FindAsync(id);

            if (await _db.Grupe.AnyAsync(g => g.SlikaId == id))
            {
                return BadRequest("Slika se nemože obrisati; Slika je vezana za grupu");
            }
            try
            {
                _db.Slike.Remove(slika);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
            return Ok(new { alert = new Alert(Severity.success, "Obrisana.", $"Slika {slika.Naziv}") });
        }
    }
}