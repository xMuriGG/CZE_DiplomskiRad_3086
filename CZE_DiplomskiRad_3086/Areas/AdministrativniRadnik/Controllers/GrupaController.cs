using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Areas.AdministrativniRadnik.Controllers
{
    [Area(nameof(CZE.Web.Areas.AdministrativniRadnik))]
    [Autorizacija(UlogeKorisnika.Predavac, UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
    public class GrupaController : Controller
    {
        private readonly CZEContext _db;
        public GrupaController(CZEContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        private async Task<GrupaVMs.GrupaCreateVM> GetGrupaCreateListItems()
        {
            GrupaVMs.GrupaCreateVM model = new GrupaVMs.GrupaCreateVM()
            {
                Predavaci = await _db.Zaposlenici.AsNoTracking().Select(s => new SelectListItem()
                {
                    Text = s.Osoba.Ime + " " + s.Osoba.Prezime + " " + (DateTime.Now.Year - s.Osoba.DatumRodjenja.Year),
                    Value = s.ZaposlenikId.ToString()
                }).ToListAsync(),
                Slike = await _db.Slike.AsNoTracking().Select(s => new SelectListItem()
                {
                    Text = s.KursKategorija.Naziv + ": " + s.Naziv,
                    Value = s.SlikaId.ToString()
                }).ToListAsync(),
                Kursevi = await _db.Kursevi.AsNoTracking().Select(s => new SelectListItem()
                {
                    Text = s.KursTip.Naziv + ": " + s.Naziv,
                    Value = s.KursId.ToString()
                }).ToListAsync(),
                Centri = await _db.Centri.AsNoTracking().Select(s => new SelectListItem()
                {
                    Text = s.Grad.Naziv + ": " + s.Naziv,
                    Value = s.CentarId.ToString()
                }).ToListAsync()
            };
            return model;
        }

        [Autorizacija( UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Create(int? id)
        {
            GrupaVMs.GrupaCreateVM model = await GetGrupaCreateListItems();
            if (id != null && id != 0)
            {
                var g = await _db.Grupe.FindAsync(id);
                if (g != null)
                {
                    model.GrupaVM = new GrupaVMs.GrupaVM()
                    {
                        GrupaId = g.GrupaId,
                        Naziv = g.Naziv,
                        Status = g.Status,
                        Casova = g.Casova,
                        Cijena = g.Cijena,
                        Pocetak = g.Pocetak,
                        Kraj = g.Kraj,
                        KursId = g.KursId,
                        CentarId = g.CentarId,
                        ZaposlenikId = g.ZaposlenikId,
                        SlikaId = g.SlikaId,

                    };
                }
            }

            return PartialView("_Create", model);
        }

        [Autorizacija( UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GrupaVMs.GrupaCreateVM model)
        {
            ModelState.Remove("GrupaVM.GrupaId");
            if (ModelState.IsValid)
            {
                Grupa g = new Grupa()
                {
                    GrupaId = model.GrupaVM.GrupaId,
                    Naziv = model.GrupaVM.Naziv,
                    Status = model.GrupaVM.Status,
                    Casova = model.GrupaVM.Casova,
                    Cijena = model.GrupaVM.Cijena,
                    Pocetak = model.GrupaVM.Pocetak,
                    Kraj = model.GrupaVM.Kraj,
                    KursId = model.GrupaVM.KursId,
                    CentarId = model.GrupaVM.CentarId,
                    ZaposlenikId = model.GrupaVM.ZaposlenikId,
                    SlikaId = model.GrupaVM.SlikaId
                };

                try
                {
                    string alertDetails = "Uspješno ";
                    if (model.GrupaVM.GrupaId != 0)
                    {
                        _db.Grupe.Attach(g);
                        _db.Entry(g).State = EntityState.Modified;
                        alertDetails += "editovana.";
                    }
                    else
                    {
                        _db.Grupe.Add(g);
                        alertDetails += "dodana u bazu.";
                    }
                    await _db.SaveChangesAsync();
                    return StatusCode(201, new { alert = new Alert(Severity.success, alertDetails, $"Grupa: {g.Naziv}") });
                }
                catch (Exception e)
                {
                    return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
                }
            }
            var tempGrupaVM = model.GrupaVM;
            model = await GetGrupaCreateListItems();
            model.GrupaVM = tempGrupaVM;
            return PartialView("_Create", model);

        }

        public async Task<IActionResult> GetGrupeDataTable(int? kursId,
            int draw, int start, int length,
            [FromForm(Name = "search[value]")] string search,
            [FromForm(Name = "order[0][column]")] int? orderColumn0,
            [FromForm(Name = "order[0][dir]")] string orderDir0,
            [FromForm(Name = "order[1][column]")] int? orderColumn1,
            [FromForm(Name = "order[1][dir]")] string orderDir1,
            [FromForm(Name = "order[2][column]")] int? orderColumn2,
            [FromForm(Name = "order[2][dir]")] string orderDir2,
            [FromForm(Name = "order[3][column]")] int? orderColumn3,
            [FromForm(Name = "order[3[dir]")] string orderDir3,
            [FromForm(Name = "order[4][column]")] int? orderColumn4,
            [FromForm(Name = "order[4[dir]")] string orderDir4
            )
        {
            var query = _db.Grupe.AsNoTracking().Where(w => (kursId ?? 0) == 0 || w.KursId == kursId).Select(s => new GrupaVMs.GrupaListItemVM()
            {
                GrupaId = s.GrupaId,
                Naziv = s.Naziv,
                KursId = s.KursId,
                ZaposlenikId = s.ZaposlenikId,
                ZaposlenikNaziv = s.Zaposlenik.Osoba.Ime + " " + s.Zaposlenik.Osoba.Prezime,
                SlikaId = s.SlikaId,
                Casova = s.Casova != null ? "<small><del>" + s.Kurs.KursTip.Casova + "</del></small><br><b>" + s.Casova + "</b>" : s.Kurs.KursTip.Casova.ToString(),
                Cijena = s.Cijena!=null ? "<small><del>" + s.Kurs.KursTip.Cijena + "</del></small><br><b>" + s.Cijena + "</b>" : s.Kurs.KursTip.Cijena.ToString(),
                Status = s.Status,
                Pocetak = s.Pocetak.ToString("d/M/yyyy H:mm"),
                Kraj = s.Kraj != null ? ((DateTime)s.Kraj).ToString("d/M/yyyy H:mm") : "",
                CentarId = s.CentarId,
                Centar = s.Centar.Grad.Naziv,
                KursInfo = s.Kurs.KursTip.KursKategorija.Naziv + "->" + s.Kurs.KursTip.Naziv + "->" + s.Kurs.Naziv,
                SlikaThumb = s.Slika.SlikaThumb,
                SlikaUrl = s.Slika.SlikaUrl
                
            });
            var recordsTotal = await query.CountAsync();
            var recordsFiltered = recordsTotal;
            //text filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(
                    w => w.Naziv.Contains(search) ||
                         w.ZaposlenikNaziv.StartsWith(search) ||
                         w.KursInfo.Contains(search) ||
                         w.Centar.StartsWith(search)
                );
                recordsFiltered = await query.CountAsync();
            }

            #region ordering
            IOrderedQueryable<GrupaVMs.GrupaListItemVM> ordering = query.OrderBy(o => 0);
            if (orderColumn0 == null)
            {
                ordering = ordering.OrderByDescending(o => o.GrupaId);
            }
            else
            {
                switch (orderColumn0)
                {
                    case 2:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.Pocetak) : ordering.OrderByDescending(o => o.Pocetak);
                        break;
                    case 3:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.Kraj) : ordering.OrderByDescending(o => o.Kraj);
                        break;
                    case 6:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.Status) : ordering.OrderByDescending(o => o.Status);
                        break;
                    case 8:
                        ordering = orderDir0 == "asc" ? ordering.OrderBy(o => o.Centar) : ordering.OrderByDescending(o => o.Centar);
                        break;

                }
            }
            if (orderColumn1 != null)
            {
                switch (orderColumn1)
                {
                    case 2:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.Pocetak) : ordering.ThenByDescending(o => o.Pocetak);
                        break;
                    case 3:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.Kraj) : ordering.ThenByDescending(o => o.Kraj);
                        break;
                    case 6:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.Status) : ordering.ThenByDescending(o => o.Status);
                        break;
                    case 8:
                        ordering = orderDir1 == "asc" ? ordering.ThenBy(o => o.Centar) : ordering.ThenByDescending(o => o.Centar);
                        break;

                }
            }
            if (orderColumn2 != null)
            {
                switch (orderColumn2)
                {
                    case 2:
                        ordering = orderDir2 == "asc" ? ordering.ThenBy(o => o.Pocetak) : ordering.ThenByDescending(o => o.Pocetak);
                        break;
                    case 3:
                        ordering = orderDir2 == "asc" ? ordering.ThenBy(o => o.Kraj) : ordering.ThenByDescending(o => o.Kraj);
                        break;
                    case 6:
                        ordering = orderDir2 == "asc" ? ordering.ThenBy(o => o.Status) : ordering.ThenByDescending(o => o.Status);
                        break;
                    case 8:
                        ordering = orderDir2 == "asc" ? ordering.ThenBy(o => o.Centar) : ordering.ThenByDescending(o => o.Centar);
                        break;

                }
            }
            if (orderColumn3 != null)
            {
                switch (orderColumn3)
                {
                    case 2:
                        ordering = orderDir3 == "asc" ? ordering.ThenBy(o => o.Pocetak) : ordering.ThenByDescending(o => o.Pocetak);
                        break;
                    case 3:
                        ordering = orderDir3 == "asc" ? ordering.ThenBy(o => o.Kraj) : ordering.ThenByDescending(o => o.Kraj);
                        break;
                    case 6:
                        ordering = orderDir3 == "asc" ? ordering.ThenBy(o => o.Status) : ordering.ThenByDescending(o => o.Status);
                        break;
                    case 8:
                        ordering = orderDir3 == "asc" ? ordering.ThenBy(o => o.Centar) : ordering.ThenByDescending(o => o.Centar);
                        break;

                }
            }
            if (orderColumn4 != null)
            {
                switch (orderColumn4)
                {
                    case 2:
                        ordering = orderDir4 == "asc" ? ordering.ThenBy(o => o.Pocetak) : ordering.ThenByDescending(o => o.Pocetak);
                        break;
                    case 3:
                        ordering = orderDir4 == "asc" ? ordering.ThenBy(o => o.Kraj) : ordering.ThenByDescending(o => o.Kraj);
                        break;
                    case 6:
                        ordering = orderDir4 == "asc" ? ordering.ThenBy(o => o.Status) : ordering.ThenByDescending(o => o.Status);
                        break;
                    case 8:
                        ordering = orderDir4 == "asc" ? ordering.ThenBy(o => o.Centar) : ordering.ThenByDescending(o => o.Centar);
                        break;

                }
            }
            #endregion

            query = ordering.Skip(start).Take(length);
            var list = await query.ToListAsync();
            foreach (var i in list)
            {
                i.StatusText = i.Status.GetEnumDisplayName();
            }

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data = list
            });
        }

        [Autorizacija( UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> GrupaStatusChange(int id)
        {
            var g = await _db.Grupe.FindAsync(id);
            if (g != null)
            {
                if (Enum.GetNames(typeof(GrupaStatus)).Length <= (int)g.Status + 1) { return Ok(); }

                g.Status = g.Status + 1;
                try
                {
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
                }
            }
            return NotFound(new { alert = new Alert(Severity.info, "Nije pronađena.", $"Grupa Id:{id}") });
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Direktor, UlogeKorisnika.Administrator)]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.Grupe.FindAsync(id);           
            try
            {
                _db.Grupe.Remove(model);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisana. Grupa vjerovatno ima svoje kandidate.", $"Grupa: {model.Naziv}")
                });
            }
            return Ok(new { alert = new Alert(Severity.success, "Obrisana.", $"Grupa {model.Naziv}") });
        }

    }
}