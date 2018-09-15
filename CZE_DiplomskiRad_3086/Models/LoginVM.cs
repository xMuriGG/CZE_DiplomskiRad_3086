using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CZE.Web.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email je obavezno polje.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lozinka je obavezno polje.")]
        [StringLength(100, ErrorMessage = "Password mora sadržavati mininalno 4 karaktera.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }
        [Display(Name = "Zapamti me")]
        public bool ZapamtiMe { get; set; }
    }
}
