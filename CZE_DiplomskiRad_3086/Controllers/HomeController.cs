using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using CZE.Web.Areas.Direktor.Models;
using CZE.Web.Areas.Kandidat.Models;
using CZE.Web.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Controllers
{
    //[Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Administrator, UlogeKorisnika.Predavac, UlogeKorisnika.Direktor)]
    //[Autorizacija()]
    public class HomeController : Controller
    {
        private readonly CZEContext _db;
        public HomeController(CZEContext db)
        {
            _db = db;
        }

        [Autorizacija(UlogeKorisnika.AdministrativniRadnik, UlogeKorisnika.Administrator, UlogeKorisnika.Predavac, UlogeKorisnika.Direktor)]
        public async Task<IActionResult> Dashboard()
        {
            var nalog = await HttpContext.GetLogiraniKorisnik();
            var model = new DashboardVMs.DashboardVM()
            {
                NePromovisaneOcjeneCount =nalog.UlogaKorisnika!=UlogeKorisnika.Predavac? await _db.Osobe.CountAsync(w => w.Zaposlenik == null && w.Kandidat == null):0,
                AktivneGrupeCount = await _db.Grupe.CountAsync(c => c.Status == GrupaStatus.Aktivna),
                CentriCount = nalog.UlogaKorisnika != UlogeKorisnika.Predavac ? await _db.Centri.CountAsync():0,
                UplateKandidataChartVM = nalog.UlogaKorisnika != UlogeKorisnika.Predavac ? await UplateKandidataChartModel():new DashboardVMs.ChartVM<decimal>(),
                BrojNovihKandidataChartVM = nalog.UlogaKorisnika != UlogeKorisnika.Predavac ? await BrojKandidataChartModel(): new DashboardVMs.ChartVM<int>(),
                OcjeneKandidata = await OcjeneKandidataModel()
            };

            return View(model);
        }


        public async Task<JsonResult> GetUplateKandidataChartModelData(int godinaDDL = 0)
        {
            return new JsonResult(await UplateKandidataChartModel(godinaDDL));
        }
        private async Task<DashboardVMs.ChartVM<decimal>> UplateKandidataChartModel(int godinaUplate = 0)
        {
            var model = new DashboardVMs.ChartVM<decimal>
            {
                GodineDDL = await _db.UplateKandidata
                    .AsNoTracking()
                    .Select(s => new SelectListItem()
                    {
                        Value = s.DatumUplate.Year.ToString(),
                        Text = s.DatumUplate.Year.ToString(),
                        Selected = s.DatumUplate.Year == godinaUplate
                    })
                    .OrderByDescending(o => o.Text)
                    .Distinct()
                    .ToListAsync()
            };
            godinaUplate = godinaUplate == 0 ? Convert.ToInt32(model.GodineDDL.FirstOrDefault()?.Value ?? "0") : godinaUplate;

            var data = await _db.UplateKandidata
                .Where(w => w.DatumUplate.Year == (godinaUplate))
                .GroupBy(g => g.DatumUplate.Month)
                .Select(s => new
                {
                    X = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(s.Key),
                    Y = s.Sum(g => g.Kolicina)
                }).ToListAsync();


            model.Data = new DashboardVMs.ChartData<decimal>()
            {
                X = data.Select(s => s.X).ToList(),
                Y = data.Select(s => s.Y).ToList()
            };

            return model;
        }
        public async Task<JsonResult> GetBrojKandidataChartModelData(int godinaDDL = 0)
        {
            return new JsonResult(await BrojKandidataChartModel(godinaDDL));
        }
        private async Task<DashboardVMs.ChartVM<int>> BrojKandidataChartModel(int godinaUplate = 0)
        {
            var model = new DashboardVMs.ChartVM<int>
            {
                GodineDDL = await _db.Kandidati
                    .AsNoTracking()
                    .Select(s => new SelectListItem()
                    {
                        Value = s.DatumUpisa.Year.ToString(),
                        Text = s.DatumUpisa.Year.ToString(),
                        Selected = s.DatumUpisa.Year == godinaUplate
                    })
                    .OrderByDescending(o => o.Text)
                    .Distinct()
                    .ToListAsync()
            };

            godinaUplate = godinaUplate == 0 ? Convert.ToInt32(model.GodineDDL.FirstOrDefault()?.Value ?? "0") : godinaUplate;

            var data = await _db.Kandidati
                .Where(w => w.DatumUpisa.Year == (godinaUplate))
                .GroupBy(g => g.DatumUpisa.Month)
                .Select(s => new
                {
                    X = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(s.Key),
                    Y = s.Count()
                }).ToListAsync();

            model.Data = new DashboardVMs.ChartData<int>()
            {
                X = data.Select(s => s.X).ToList(),
                Y = data.Select(s => s.Y).ToList()
            };

            return model;
        }

        private async Task<List<OcjenaVMs.OcjenaDetailsVM>> OcjeneKandidataModel()
        {
            return await _db.Ocjene.Select(s => new OcjenaVMs.OcjenaDetailsVM()
            {
                OcjenaId = s.OcjenaId,
                DatumOcjene = s.DatumOcjene,
                Opis = s.Opis,
                Silenced = s.Silenced,
                Vrijednost = s.Vrijednost,
                GrupaKandidatiId = s.GrupaKandidatiId,
                GrupaId = s.GrupaKandidati.GrupaId,
                GrupaNaziv = s.GrupaKandidati.Grupa.Naziv,
                KandidatId = s.GrupaKandidati.KandidatId,
                KandidatNaziv = s.GrupaKandidati.Kandidat.Osoba.Ime + " " + s.GrupaKandidati.Kandidat.Osoba.Prezime
            })
            .Take(10)
            .ToListAsync();
        }

        public async Task<IActionResult> KlijentskiPortalIndex()
        {
            var model = new KlijentskiPortalVMs()
            {
                CentariVm = await _db.Centri.Select(s => new CentarVMs.CentarListItemVM()
                {
                    CentarId = s.CentarId,
                    Naziv = s.Naziv,
                    Grad = s.Grad.Naziv,
                    Email = s.Email,
                    Adresa = s.Adresa,
                    BrojMobitela = s.BrojMobitela,
                    BrojTelefona = s.BrojTelefona,
                    GradId = s.GradId,
                    Longitude = s.Longitude,
                    Latitude = s.Latitude
                }).ToListAsync()
            };

            return View(model);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
