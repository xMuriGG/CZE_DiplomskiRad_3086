using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class KursVMs
    {
        public class IndexVM
        {
            public List<KursKategorija> KursKategorije { get; set; }
            public List<KursTip> KursTipovi { get; set; }
            public KursKategorijaVM KursKategorijaVM { get; set; }
        }
        public class DetailsVM
        {
            public KursKategorijaVM KursKategorijaVM { get; set; }
            public KursTipVM KursTipVM { get; set; }
            public KursVM KursVM { get; set; }
        }

        public class KursKategorijaVM
        {
            public int KursKategorijaId { get; set; }
            [Required(ErrorMessage = "Naziv kategorije kursa je obavezno polje.")]
            [MaxLength(100,ErrorMessage = "Maximalna dužina naziva je 100 karaktera.")]
            public string Naziv { get; set; }
        }

        public class KursTipVM
        {
            public int KursTipId { get; set; }
            [Required(ErrorMessage = "Naziv kategorije kursa je obavezno polje.")]
            [MaxLength(100, ErrorMessage = "Maximalna dužina naziva je 100 karaktera.")]
            public string Naziv { get; set; }
            [Display(Name = "Časova")]
            [Required(ErrorMessage = "Trajanje tipa kursa je obavezno polje.")]
            [Range(1, int.MaxValue, ErrorMessage = "Trajanje tipa kursa je obavezno polje.")]
            public int Casova { get; set; }
            [Required(ErrorMessage = "Cijena tipa kursa je obavezno polje.")]
            [DataType(DataType.Currency)]
            public decimal Cijena { get; set; }
            [Required(ErrorMessage = "Opis tipa kursa je obavezno polje.")]
            [DataType(DataType.MultilineText)]
            [StringLength(1000, ErrorMessage = "Max. 1000 karaktera.")]
            public string Opis { get; set; }
            [Display(Name = "Kategorija")]
            [Required(ErrorMessage = "Kategorija tipa kursa je obavezno polje.")]
            public int KursKategorijaId { get; set; }
        }
        public class KursTipCreateVM
        {
            public KursTipVM KursTipVm { get; set; }
            public List<SelectListItem> KursKategorije { get; set; }
        }

        public class KursVM
        {
            public int KursId { get; set; }
            [Required(ErrorMessage = "Naziv kursa je obavezno polje.")]
            [MaxLength(100, ErrorMessage = "Maximalna dužina naziva je 100 karaktera.")]
            public string Naziv { get; set; }
            [DataType(DataType.MultilineText)]
            [StringLength(1000, ErrorMessage = "Max. 1000 karaktera.")]
            public string Opis { get; set; }
            [Display(Name = "Tip")]
            [Required(ErrorMessage = "Tip kursa je obavezno polje.")]
            public int KursTipId { get; set; }
        }
        public class KursCreateVM
        {
            public KursVM KursVM { get; set; }
            public List<SelectListItem> KursTipovi { get; set; }
            public int KursTipId { get; set; }
        }
        public class KursTablePartial
        {
            public int KursTipId { get; set; }
            public string KursTipNaziv { get; set; }
            public List<Kurs> Kursevi { get; set; }
        }
    }
}
