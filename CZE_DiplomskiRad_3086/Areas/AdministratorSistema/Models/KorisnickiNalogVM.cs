using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data.Models;

namespace CZE.Web.Areas.AdministratorSistema.Models
{
    public class KorisnickiNalogVM
    {
        public int KorisnickiNalogId { get; set; }
        public int OsobaId { get; set; }
        [Required(ErrorMessage = "Korisničko ime je obavezno polje.")]
        [Display(Name = "Korisničko ime")]
        [StringLength(256,ErrorMessage = "Max. length 256")]
        public string KorisnickoIme { get; set; }
        [Display(Name = "Izmjena lozinke")]
        public bool IzmjenaLozinke { get; set; }
        //[DataType(DataType.Password)]
        [StringLength(256,ErrorMessage = "Lozinka mora biti između 8 i 256 karaktera.",MinimumLength = 8)]
        public string Lozinka { get; set; }

        public bool Aktivan { get; set; }
        [Display(Name = "Uloga korisnika")]
        public UlogeKorisnika UlogaKorisnika { get; set; }

        public bool SendEmail { get; set; }
        public string EmailBody { get; set; }
    }
}
