using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data.Models;

namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class OsobaListItemVM
    {
        public int OsobaId { get; set; }
        [Display(Name = "Ime i prezime")]
        public string ImePrezime { get; set; }
        [Display(Name = "Datum rođenja")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public string DatumRodjenja { get; set; }
        public Osoba.eSpol Spol { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Grad { get; set; }
        public string Adresa { get; set; }
        public string Permisija { get; set; }
        public bool IsZaposlenik { get; set; }
        public bool IsKandidat { get; set; }
    }
}
