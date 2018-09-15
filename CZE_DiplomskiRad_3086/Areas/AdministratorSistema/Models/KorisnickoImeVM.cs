using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CZE.Web.Areas.AdministratorSistema.Models
{
    public class KorisnickoImeVM
    {
        public int KorisnickiNalogId { get; set; }      
        [Required(ErrorMessage = "Korisničko ime je obavezno polje.")]
        [Display(Name = "Korisničko ime")]
        [StringLength(256, ErrorMessage = "Max. length 256")]
        public string KorisnickoIme { get; set; }
    }
}
