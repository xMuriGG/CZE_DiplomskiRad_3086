using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using CZE.Web.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Controllers
{
    public class GrupaController : Controller
    {
        private readonly CZEContext _db;

        public GrupaController(CZEContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int id = 0)
        {
            var model = new GrupaIndexVM()
            {
                GrupaClientListItemVM = await GetInitGrupaQuery(id, GrupaStatus.Aktivna, 0, 6).ToListAsync(),
                KursKategorijeVM = await _db.KursKategorije.Select(s => new KursKategorijaSF()
                {
                    KursKategorijaId = s.KursKategorijaId,
                    Naziv = s.Naziv,
                    Checked = s.KursKategorijaId == id

                }).ToListAsync()
            };

            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            GrupaVMs.GrupaDetailsVM model;
            if (await _db.Grupe.AnyAsync(a => a.GrupaId == id))
            {
                model = await _db.Grupe.Where(w => w.GrupaId == id).Select(s => new GrupaVMs.GrupaDetailsVM()
                {
                    GrupaId = s.GrupaId,
                    Naziv = s.Naziv,
                    Pocetak = s.Pocetak,
                    Kraj = s.Kraj,
                    Casova = s.Casova,
                    Cijena = s.Cijena,
                    Status = s.Status,
                    ZaposlenikId = s.ZaposlenikId,
                    ZaposlenikNaziv = s.Zaposlenik.Osoba.Ime + " " + s.Zaposlenik.Osoba.Prezime,
                    CentarId = s.CentarId,
                    CentarGrad = s.Centar.Grad.Naziv,
                    CentarNaziv = s.Centar.Naziv,
                    //Slika = s.Slika.SlikaFile,
                    //SlikaUrl=s.Slika.SlikaUrl,
                    Ocjena = _db.Ocjene.Where(w => !w.Silenced && w.GrupaKandidati.GrupaId == s.GrupaId).Select(ss => ss.Vrijednost)
                        .DefaultIfEmpty(0).Average(),
                    KursDetails = new KursVMs.DetailsVM()
                    {
                        KursKategorijaVM = new KursVMs.KursKategorijaVM()
                        {
                            KursKategorijaId = s.Kurs.KursTip.KursKategorijaId,
                            Naziv = s.Kurs.KursTip.KursKategorija.Naziv
                        },
                        KursTipVM = new KursVMs.KursTipVM()
                        {
                            KursTipId = s.Kurs.KursTipId,
                            Naziv = s.Kurs.KursTip.Naziv,
                            Casova = s.Kurs.KursTip.Casova,
                            Cijena = s.Kurs.KursTip.Cijena,
                            Opis = s.Kurs.KursTip.Opis,
                        },
                        KursVM = new KursVMs.KursVM()
                        {
                            KursTipId = s.Kurs.KursTipId,
                            Naziv = s.Kurs.Naziv,
                            Opis = s.Kurs.Opis
                        }
                    }
                }).SingleOrDefaultAsync();

                var nalog = await HttpContext.GetLogiraniKorisnik();

                if (nalog != null)
                {
                    model.IsLogiraniKorisnikKandidat = await _db.Kandidati.AnyAsync(a => a.KandidatId == nalog.KorisnickiNalogId);

                    var gk = await
                        _db.GrupaKandidati.SingleOrDefaultAsync(a => a.GrupaId == model.GrupaId && a.KandidatId == nalog.KorisnickiNalogId);
                    model.LogiraniKandidatPrijavljen = gk != null;
                    model.Odobren = gk?.Odobren ?? false;
                }
            }
            else
            {
                return NotFound();
            }
            return View(model);
        }

        public async Task<IActionResult> GrupeKandidata(int id)
        {
            var model = await _db.GrupaKandidati.Where(w => w.KandidatId == id).Select(s => new GrupaVMs.GrupaKandidataVM()
            {
                GrupaKandidatiId = s.GrupaKandidatiId,
                GrupaId = s.GrupaId,
                Naziv = s.Grupa.Naziv,
                Pocetak = s.Grupa.Pocetak.ToString("d/M/yyyy H:mm"),

                KursKategorijaId = s.Grupa.Kurs.KursTip.KursKategorijaId,
                KursKategorijaNaziv = s.Grupa.Kurs.KursTip.KursKategorija.Naziv,
                KursTipId = s.Grupa.Kurs.KursTipId,
                KursTipNaziv = s.Grupa.Kurs.KursTip.Naziv,
                KursId = s.Grupa.KursId,
                KursNaziv = s.Grupa.Kurs.Naziv,

                ZaposlenikId = s.Grupa.ZaposlenikId,
                ZaposlenikNaziv = s.Grupa.Zaposlenik.Osoba.Ime + " " + s.Grupa.Zaposlenik.Osoba.Prezime,

                CentarId = s.Grupa.CentarId,
                CentarLokacija = s.Grupa.Centar.Grad.Naziv,

                Status = s.Grupa.Status,
                Cijena = (s.Grupa.Cijena ?? s.Grupa.Kurs.KursTip.Cijena) + "",
                Uplaceno = _db.UplateKandidata.Where(w => w.GrupaKandidatiId == s.GrupaKandidatiId).Sum(sum => sum.Kolicina) + "",
                Odobren = s.Odobren,
                Ocjena = _db.Ocjene.FirstOrDefault(w => w.GrupaKandidatiId == s.GrupaKandidatiId),
                Polozio = true   //TODO: zamjeniti sa stvarom vrijednoscu


            })
                .ToListAsync();

            return View("GrupeKandidataIndex", model);
        }

        public async Task<JsonResult> GetGrupeCardListItem(int draw, int? kategorijaId,
            int start, int length = 6, GrupaStatus status = GrupaStatus.Aktivna)
        {
            var query = GetInitGrupaQuery(kategorijaId, status, start, length);

            var recordsTotal = await query.CountAsync();
            var list = await query.ToListAsync();
            var recordsFiltered = list.Count();

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data = list
            });
        }

        public async Task<JsonResult> GetPopularneGrupeCardListItem(int draw,int length)
        {
            var query = _db.Grupe.Select(s => new GrupaVMs.GrupaCardListItemVM()
            {
                GrupaId = s.GrupaId,
                Naziv = s.Naziv,
                Pocetak = s.Pocetak.ToString("d/M/yyyy H:mm"),
                Kraj = s.Kraj != null ? ((DateTime)s.Kraj).ToString("d/M/yyyy") : "",
                Casova = s.Casova ?? s.Kurs.KursTip.Casova,
                Cijena = (s.Cijena ?? s.Kurs.KursTip.Cijena) + "KM",
                Slika = s.Slika.SlikaFile,
                SlikaUrl = s.Slika.SlikaUrl,
                KursKategorijaNaziv = s.Kurs.KursTip.KursKategorija.Naziv,
                KursTipNaziv = s.Kurs.KursTip.Naziv,
                KursNaziv = s.Kurs.Naziv,
                ZaposlenikNaziv = s.Zaposlenik.Osoba.Ime + " " + s.Zaposlenik.Osoba.Prezime,
                CentarId = s.CentarId,
                CentarLokacija = s.Centar.Grad.Naziv,
                KandidataPrijavljeno = _db.GrupaKandidati.Count(w => w.GrupaId == s.GrupaId),              
                Ocjena = _db.Ocjene.Where(w => !w.Silenced && w.GrupaKandidati.GrupaId == s.GrupaId).Select(ss => ss.Vrijednost)
                    .DefaultIfEmpty(0).Average()
            })
            .OrderByDescending(o=>o.KandidataPrijavljeno)
            .ThenByDescending(o=>o.Ocjena)
            .Take(length);

            var list = await query.ToListAsync();
            var recordsTotal = query.Count();
            var recordsFiltered = 0;

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data = list
            });
        }


        [Autorizacija()]
        public async Task<IActionResult> PrijavaUGrupu(int id)
        {
            var nalog = await HttpContext.GetLogiraniKorisnik();
            var kandidatId = nalog?.KorisnickiNalogId;

            var kandidat = await _db.Kandidati.FindAsync(kandidatId ?? 0);

            if (kandidat == null)
            {
                return BadRequest("Logirani korisnik mora biti kandidat da bi se prijavio u grupu.");
            }

            Grupa grupa = await _db.Grupe.FindAsync(id);
            if (grupa == null || kandidatId == null || grupa.Status != GrupaStatus.Aktivna) { return BadRequest(); }

            GrupaKandidati grupaKandidata = await _db.GrupaKandidati.Where(w => w.GrupaId == id && w.KandidatId == kandidatId).SingleOrDefaultAsync();

            if (grupaKandidata != null)
            {
                if (grupaKandidata.Odobren)
                {
                    _db.GrupaKandidati.Remove(grupaKandidata);
                }
                else
                {
                    BadRequest("Korisnik se ne moze odjaviti sa grupe ako nije odobren.");
                }
            }
            else
            {
                var gKandidati = await _db.GrupaKandidati.AddAsync(new GrupaKandidati() { Grupa = grupa, KandidatId = (int)kandidatId });
                var prisustvaTermini = _db.PrisustvoTermini.Where(w => w.GrupaId == grupa.GrupaId);
                var prisustva = new List<Prisustvo>();
                foreach (var pt in prisustvaTermini)
                {
                    prisustva.Add(new Prisustvo()
                    {
                        PrisustvoTermin = pt,
                        GrupaKandidati = gKandidati.Entity,
                        Prisutan = false
                    });
                }
                if (prisustva.Any())
                {
                    await _db.Prisustva.AddRangeAsync(prisustva);
                }
            }
            try
            {
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        private IQueryable<GrupaVMs.GrupaCardListItemVM> GetInitGrupaQuery(int? kategorijaId, GrupaStatus status, int start, int length)
        {
            var query = _db.Grupe
                .Where(w => (kategorijaId ?? 0) == 0 || w.Kurs.KursTip.KursKategorija.KursKategorijaId == kategorijaId);
            query = query.Where(w => (w.Status == status));

            var query1 = query.Select(s => new GrupaVMs.GrupaCardListItemVM()
            {
                GrupaId = s.GrupaId,
                Naziv = s.Naziv,
                Pocetak = s.Pocetak.ToString("d/M/yyyy H:mm"),
                Kraj = s.Kraj != null ? ((DateTime)s.Kraj).ToString("d/M/yyyy") : "",
                Casova = s.Casova ?? s.Kurs.KursTip.Casova,
                Cijena = (s.Cijena ?? s.Kurs.KursTip.Cijena) + "KM",
                Slika = s.Slika.SlikaFile,
                SlikaUrl = s.Slika.SlikaUrl,
                KursKategorijaNaziv = s.Kurs.KursTip.KursKategorija.Naziv,
                KursTipNaziv = s.Kurs.KursTip.Naziv,
                KursNaziv = s.Kurs.Naziv,
                ZaposlenikNaziv = s.Zaposlenik.Osoba.Ime + " " + s.Zaposlenik.Osoba.Prezime,
                CentarId = s.CentarId,
                CentarLokacija = s.Centar.Grad.Naziv,
                KandidataPrijavljeno = _db.GrupaKandidati.Count(w => w.GrupaId == s.GrupaId),
                //Ocjena = 3.3D // _db.Ocjene.Where(w =>w.GrupaKandidati.GrupaId == s.GrupaId).Average(a=>a.Vrijednost) ovo pravi gresku 
                //Ocjena = _db.Ocjene.Where(w => w.GrupaKandidati.GrupaId == s.GrupaId).DefaultIfEmpty().Average(a=>a==null?0D:a.Vrijednost)
                Ocjena = _db.Ocjene.Where(w => !w.Silenced && w.GrupaKandidati.GrupaId == s.GrupaId).Select(ss => ss.Vrijednost)
                    .DefaultIfEmpty(0).Average()
            }).Skip(start).Take(length);
            return query1;
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.Grupe.FindAsync(id);
            if (model == null)
            {
                return NotFound(new
                {
                    alert = new Alert(Severity.warning, "Nije pronađena.", $"Grupa sa Id:{id} brojem")
                });
            }

            try
            {
                _db.Grupe.Remove(model);
                await _db.SaveChangesAsync();
                return Ok(new { alert = new Alert(Severity.success, "Je obrisana.", $"Grupa: {model.Naziv}") });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    alert = new Alert(Severity.warning, "Nije obrisana. ", $"Grupa: {model.Naziv}")
                });
            }
        }
    }
}