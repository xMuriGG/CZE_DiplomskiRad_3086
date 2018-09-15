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
    [Autorizacija(UlogeKorisnika.Administrator, UlogeKorisnika.Direktor, UlogeKorisnika.Predavac, UlogeKorisnika.AdministrativniRadnik)]
    public class GrupaKandidatiController : Controller
    {
        private readonly CZEContext _db;
        public GrupaKandidatiController(CZEContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> GrupaKandidatiTable(int id)
        {
            var grupa = await _db.Grupe.FindAsync(id);
            if (grupa == null)
            {
                return NotFound();
            }
            var model = new GrupaVMs.GrupaKandidatiTableVM()
            {
                GrupaKandidati = await _db.GrupaKandidati.AsNoTracking().Where(w => w.GrupaId == id)
                        .Select(s => new GrupaVMs.GrupaKandidatiListItemVM()
                        {
                            GrupaKandidatiId = s.GrupaKandidatiId,
                            KandidatId = s.KandidatId,
                            Ime = s.Kandidat.Osoba.Ime,
                            Prezime = s.Kandidat.Osoba.Prezime,
                            Email = s.Kandidat.Osoba.Email,
                            BrojTelefona = s.Kandidat.Osoba.BrojTelefona,
                            BrojMobitela = s.Kandidat.Osoba.BrojMobitela,
                            Odobren = s.Odobren,
                            Cijena = (s.Grupa.Cijena ?? s.Grupa.Kurs.KursTip.Cijena) + "",
                            Uplaceno = _db.UplateKandidata.Where(w => w.GrupaKandidatiId == s.GrupaKandidatiId)
                            .Sum(sum => sum.Kolicina) + "",
                            Ocjena = _db.Ocjene.AsNoTracking().FirstOrDefault(w => w.GrupaKandidatiId == s.GrupaKandidatiId),
                        })
                        .ToListAsync(),
                GrupaId = grupa.GrupaId,
                GrupaNaziv = grupa.Naziv
            };

            return PartialView("_Index", model);
        }

        public async Task<IActionResult> OdobrenStatusChange(int id)
        {
            var gK = await _db.GrupaKandidati.FindAsync(id);
            if (gK == null)
            {
                return NotFound();
            }
            gK.Odobren = !gK.Odobren;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Došlo je do greške prilikom snimanja podatka u bazu.; " + e.Message);
            }
            return RedirectToAction("GrupaKandidatiTable", new { id = gK.GrupaId });
        }

        public async Task<IActionResult> DodajKandidataUGrupu(int grupaId, int kandidatId)
        {

            Grupa grupa = await _db.Grupe.FindAsync(grupaId);
            if (grupa == null) { return BadRequest(); }

            GrupaKandidati grupaKandidata = await _db.GrupaKandidati.Where(w => w.GrupaId == grupaId && w.KandidatId == kandidatId).SingleOrDefaultAsync();

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
            await _db.SaveChangesAsync();
            var model = await _db.GrupaKandidati.AsNoTracking().Where(w => w.GrupaId == grupaId)
                .Select(s => new GrupaVMs.GrupaKandidatiListItemVM()
                {
                    GrupaKandidatiId = s.GrupaKandidatiId,
                    KandidatId = s.KandidatId,
                    Ime = s.Kandidat.Osoba.Ime,
                    Prezime = s.Kandidat.Osoba.Prezime,
                    Email = s.Kandidat.Osoba.Email,
                    BrojTelefona = s.Kandidat.Osoba.BrojTelefona,
                    BrojMobitela = s.Kandidat.Osoba.BrojMobitela,
                    Odobren = s.Odobren,
                    Cijena = (s.Grupa.Cijena ?? s.Grupa.Kurs.KursTip.Cijena) + "",
                    Uplaceno = _db.UplateKandidata.Where(w => w.GrupaKandidatiId == s.GrupaKandidatiId)
                                   .Sum(sum => sum.Kolicina) + "",
                    Ocjena = _db.Ocjene.AsNoTracking().FirstOrDefault(w => w.GrupaKandidatiId == s.GrupaKandidatiId),
                })
                .ToListAsync();

            return PartialView("_GrupaKandidatiTable", model);
        }
    }
}