using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CZE.Data.Models;
using CZE.Web.Areas.AdministratorSistema.Models;
using CZE.Web.Areas.Direktor.Models;
namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class OsobaVMs
    {
        public class OsobaQuickDetailsVM
        {
            public int OsobaId { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            [Display(Name = "Datum rođenja")]
            [Required(ErrorMessage = "Datum rođenja je obavezno polje")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime DatumRodjenja { get; set; }
            [Display(Name = "Mjesto rođenja")]
            public int MjestoRodjenjaId { get; set; }

            public string MjestoRodjenjaNaziv { get; set; }
            public Osoba.eSpol Spol { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Display(Name = "Mjesto boravka")]
            public int MjestoBoravkaId { get; set; }
            public string MjestoBoravkaNaziv { get; set; }
            [Display(Name = "Adresa stanovanja")]
            public string Adresa { get; set; }
            [Display(Name = "Broj mobitela")]
            public string BrojMobitela { get; set; }
            [Display(Name = "Broj telefona")]
            public string BrojTelefona { get; set; }
            [Display(Name = "Kako ste saznali za nas?")]
            public string KakoSteSaznaliZaNas { get; set; }

            #region Podaci o firmi
            [Display(Name = "Podaci o firmi")]
            public bool PodatciOFirmi { get; set; }
            [Display(Name = "Naziv firme")]
            public string NazivFirme { get; set; }
            [Display(Name = "Adresa firme")]
            public string AdresaFirme { get; set; }
            [Display(Name = "Sjedište firme")]
            public int? GradFirmeId { get; set; }
            [Display(Name = "Broj telefona firme")]
            public string GradFirmeNaziv { get; set; }
            public string BrojTelefonaFirme { get; set; }
            [Display(Name = "Email firme")]
            [DataType(DataType.EmailAddress)]
            public string EmailFirme { get; set; }
            #endregion


            public ZaposlenikVMs.ZaposlenikCreateVM ZaposlenikCreateVM { get; set; }
            public KandidatVMs.KandidatCreateVM KandidatCreateVM { get; set; }

            public bool KorisnickiNalog { get; set; }
            public KorisnickiNalogVM KorisnickiNalogVM { get; set; }

        }
        public class OsobaDetailsVM
        {
            public OsobaQuickDetailsVM OsobaQuickDetailsVM { get; set; }
            public List<GrupaVMs.GrupaKandidataVM> GrupeKandidataVM { get; set; }
        }
    }
}
