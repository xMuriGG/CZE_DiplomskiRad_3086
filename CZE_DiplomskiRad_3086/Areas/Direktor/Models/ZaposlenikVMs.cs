using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data.Models;

namespace CZE.Web.Areas.Direktor.Models
{
    public class ZaposlenikVMs
    {
        public class ZaposlenikCreateVM
        {
            public int OsobaId { get; set; }
            public string OsobaNaziv { get; set; }
            public ZaposlenikVM ZaposlenikVM { get; set; }            
        }
        public class ZaposlenikVM
        {
            public int ZaposlenikId { get; set; }
            [Required]
            public Zaposlenik.StepeniObrazovanja StepenObrazovanja { get; set; }
            [Required(ErrorMessage = "Broj lične karte je obavezno polje")]

            [StringLength(9, ErrorMessage = "Broj lične karte se mora sadržavati od 9 karaktera", MinimumLength = 9)]
            public string BrojLicneKarte { get; set; }
            [Required(ErrorMessage = "Broj računa je obavezno polje")]
            [MaxLength(20, ErrorMessage = "Maximalna dužina broja računa je 20 karaktera")]
            [MinLength(16, ErrorMessage = "Minimalna dužina broja računa je 16 karaktera")]
            public string BrojRacuna { get; set; }
        }
    }
   
}
