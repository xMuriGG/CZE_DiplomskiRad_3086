using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CZE.Web.Areas.Kandidat.Models
{
    public class OcjenaVMs
    {
        public class OcjenaVM
        {
            public int OcjenaId { get; set; }
            [Required(ErrorMessage = "Vrijednost ocjene je obavezno polje.")]
            [Range(1,5,ErrorMessage = "Ocjena mora biti broj od 1-5")]
            [Display(Name = "Ocjena")]
            public int Vrijednost { get; set; }
            [StringLength(300,ErrorMessage = "Dužina opisa mora biti između 20 i 300 karaktera.")]
            public string Opis { get; set; }
            public bool Silenced { get; set; }

            [Display(Name = "Grupa kandidata")]
            public int GrupaKandidatiId { get; set; }
        }
        public class OcjenaDetailsVM
        {
            public int OcjenaId { get; set; }
           
            [Display(Name = "Ocjena")]
            public int Vrijednost { get; set; }          
            public string Opis { get; set; }
            public bool Silenced { get; set; }
            public DateTime DatumOcjene { get; set; }

            [Display(Name = "Grupa kandidata")]
            public int GrupaKandidatiId { get; set; }

            public int GrupaId { get; set; }
            public string GrupaNaziv { get; set; }

            public int KandidatId { get; set; }
            public string KandidatNaziv { get; set; }
        }
    }
}
