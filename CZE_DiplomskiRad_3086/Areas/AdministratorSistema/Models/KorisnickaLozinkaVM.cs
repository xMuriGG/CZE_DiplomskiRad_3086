using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CZE.Web.Areas.AdministratorSistema.Models
{
    public class KorisnickaLozinkaVM
    {
        public int KorisnickiNalogId { get; set; }
        [Required(ErrorMessage = "Stara lozinka je obavezno polje.")]
        //[StringLength(256, ErrorMessage = "Lozinka mora biti između 8 i 256 karaktera.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Stara lozinka")]
        public string StaraLozinka { get; set; }
        [Required(ErrorMessage = "Nova lozinka je obavezno polje.")]
        [StringLength(256, ErrorMessage = "Lozinka mora biti između 8 i 256 karaktera.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova lozinka")]
        public string NovaLozinka { get; set; }
        [Required(ErrorMessage = "Potvrdna lozinka je obavezno polje.")]
        [Compare("NovaLozinka",ErrorMessage = "Nova i potvrdna lozinka se ne podudaraju.")]
        [StringLength(256, ErrorMessage = "Lozinka mora biti između 8 i 256 karaktera.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrdna lozinka")]
        public string PotvrdnaLozinka { get; set; }
    }
}
