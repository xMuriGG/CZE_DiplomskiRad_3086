using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class UplataKandidataVMs
    {
        public class UplataKandidataVM
        {
            public int UplataKandidataId { get; set; }
            [Required(ErrorMessage = "Količina uplate je obavezno polje.")]
            [Display(Name = "Količina")]
            public decimal Kolicina { get; set; }
            [Display(Name = "Bilješke")]
            [MaxLength(1000,ErrorMessage = "Max. length 1000")]
            public string Biljeske { get; set; }
            public DateTime? DatumUplate { get; set; }
            [Required(ErrorMessage = "Grupa uplate je obavezno polje.")]
            public int GrupaKandidatiId { get; set; }
            [Required(ErrorMessage = "Zaposlenik uplate je obavezno polje.")]
            public int ZaposlenikId { get; set; }
        }
        public class UplataKandidataCreateVM
        {
            public int GrupaKandidatiId { get; set; }
            public UplataKandidataVM UplataKandidataVM { get; set; }
        }
        public class UplataKandidataTableVM
        {
            public int GrupaId { get; set; }
            public int GrupaKandidatiId { get; set; }
            public string GrupaKandidatiInfo { get; set; }
            public string KandidatInfo { get; set; }
            public string Cijena { get; set; }
            [Display(Name = "Uplačeno")]
            public string Uplaceno { get; set; }
            public List<UplataKandidataVM> UplateKandidata { get; set; }
        }

    }
}
