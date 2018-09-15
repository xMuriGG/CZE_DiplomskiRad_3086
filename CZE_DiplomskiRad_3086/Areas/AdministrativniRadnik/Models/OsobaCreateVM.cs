using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class OsobaCreateVM
    {
        public List<SelectListItem> Gradovi { get; set; }
        public Person Osoba { get; set; }

        public Osoba ToOsoba()
        {
           Osoba o=new Osoba();
            o.OsobaId = this.Osoba.OsobaId;
            o.Ime = this.Osoba.Ime;
            o.Prezime = this.Osoba.Prezime;
            o.DatumRodjenja = this.Osoba.DatumRodjenja;
            o.MjestoRodjenjaId = this.Osoba.MjestoRodjenjaId;
            o.Spol = this.Osoba.Spol;
            o.Email = this.Osoba.Email;
            o.MjestoBoravkaId = this.Osoba.MjestoBoravkaId;
            o.Adresa = this.Osoba.Adresa;
            o.BrojMobitela = this.Osoba.BrojMobitela;
            o.BrojTelefona = this.Osoba.BrojTelefona;
            o.KakoSteSaznaliZaNas = this.Osoba.KakoSteSaznaliZaNas;
            o.PodatciOFirmi = this.Osoba.PodatciOFirmi;
            o.NazivFirme = this.Osoba.NazivFirme;
            o.AdresaFirme = this.Osoba.AdresaFirme;
            o.GradFirmeId = this.Osoba.GradFirmeId;
            o.BrojTelefonaFirme = this.Osoba.BrojTelefonaFirme;
            o.EmailFirme = this.Osoba.EmailFirme;

            return o;
        }

        public class Person
        {

            public int OsobaId { get; set; }
            [Required(ErrorMessage = "Ime je obavezno polje")]
            public string Ime { get; set; }
            [Required(ErrorMessage = "Prezime je obavezno polje")]
            public string Prezime { get; set; }
            [Display(Name = "Datum rođenja")]
            [Required(ErrorMessage = "Datum rođenja je obavezno polje")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime DatumRodjenja { get; set; }
            [Required(ErrorMessage = "Mjesto rođenja je obavezno polje")]
            [Display(Name = "Mjesto rođenja")]
            public int MjestoRodjenjaId { get; set; }
            [Required(ErrorMessage = "Spol je obavezno polje")]
            public Osoba.eSpol Spol { get; set; }
            [Required(ErrorMessage = "Email je obavezno polje")]
            [EmailAddress(ErrorMessage = "Unesite validan Email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Mjesto boravka je obavezno polje")]
            [Display(Name = "Mjesto boravka")]
            public int MjestoBoravkaId { get; set; }
            [Display(Name = "Adresa stanovanja")]
            [Required(ErrorMessage = "Adresa stanovanja je obavezno polje")]
            public string Adresa { get; set; }
            [Required(ErrorMessage = "Broj mobitela je obavezno polje.")]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"\+?387-?\d{2}-?\d{3}-?\d{3}\d?", ErrorMessage = "Nije validan broj telefona.")]
            [Display(Name = "Broj mobitela")]
            public string BrojMobitela { get; set; }
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"\+?387-?\d{2}-?\d{3}-?\d{3}\d?", ErrorMessage = "Nije validan broj telefona.")]
            [Display(Name = "Broj telefona")]
            public string BrojTelefona { get; set; }
            [DataType(DataType.MultilineText)]
            [Display(Name = "Kako ste saznali za nas?")]
            public string KakoSteSaznaliZaNas { get; set; }

            //Podaci o firmi
            [Display(Name = "Podaci o firmi")]
            public bool PodatciOFirmi { get; set; }
            [Display(Name = "Naziv")]
            public string NazivFirme { get; set; }
            [Display(Name = "Adresa")]
            public string AdresaFirme { get; set; }
            [Display(Name = "Sjedište")]
            public int? GradFirmeId { get; set; }
            [Display(Name = "Broj telefona")]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"\+?387-?\d{2}-?\d{3}-?\d{3}\d?", ErrorMessage = "Nije validan broj telefona.")]
            public string BrojTelefonaFirme { get; set; }
            [EmailAddress(ErrorMessage = "Unesi validan Email")]
            [Display(Name = "Email")]
            public string EmailFirme { get; set; }
        }
       


      

    }
}
