using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.ViewComponents
{
    public class RegistracijaViewComponent:ViewComponent
    {

        private readonly CZEContext _db;

        public RegistracijaViewComponent(CZEContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id, bool klijentskiPortalCall=true)
        {
            ViewData["kpCall"] = klijentskiPortalCall;
            TempData["kpCall"] = klijentskiPortalCall;
            var model = new OsobaCreateVM
            {
                Gradovi = await _db.Gradovi.Select(s => new SelectListItem()
                {
                    Text = s.Naziv,
                    Value = s.GradId.ToString()
                }).ToListAsync()
            };
            if (id != null && id != 0)
            {
                var o = await _db.Osobe.FindAsync(id);
                if (o != null)
                {
                    model.Osoba = new OsobaCreateVM.Person()
                    {
                        OsobaId = o.OsobaId,
                        Ime = o.Ime,
                        Prezime = o.Prezime,
                        DatumRodjenja = o.DatumRodjenja,
                        Email = o.Email,
                        BrojTelefona = o.BrojTelefona,
                        Adresa = o.Adresa,
                        BrojMobitela = o.BrojMobitela,
                        MjestoBoravkaId = o.MjestoBoravkaId,
                        Spol = o.Spol,
                        KakoSteSaznaliZaNas = o.KakoSteSaznaliZaNas,
                        BrojTelefonaFirme = o.BrojTelefonaFirme,
                        AdresaFirme = o.AdresaFirme,
                        PodatciOFirmi = o.PodatciOFirmi,
                        NazivFirme = o.NazivFirme,
                        MjestoRodjenjaId = o.MjestoRodjenjaId,
                        GradFirmeId = o.GradFirmeId,
                        EmailFirme = o.EmailFirme
                    };
                }
            }
            return View(model);
        }
    }
}
