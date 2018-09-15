using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CZE.Web.Areas.Direktor.Models
{
    public class CentarVMs
    {
        public class CentarVM
        {
            public int CentarId { get; set; }
            [Required(ErrorMessage = "Naziv centra je obavezno polje.")]
            [StringLength(100,ErrorMessage = "Max. length 100 char")]
            public string Naziv { get; set; }
            [Required(ErrorMessage = "Adresa centra je obavezno polje.")]
            [StringLength(100, ErrorMessage = "Max. length 100 char")]
            public string Adresa { get; set; }
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"\+?387-?\d{2}-?\d{3}-?\d{3}\d?", ErrorMessage = "Nije validan broj telefona.")]
            [Display(Name = "Broj mobitela")]
            public string BrojMobitela { get; set; }
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"\+?387-?\d{2}-?\d{3}-?\d{3}\d?", ErrorMessage = "Nije validan broj telefona.")]
            [Display(Name = "Broj telefona")]
            public string BrojTelefona { get; set; }
            [EmailAddress(ErrorMessage = "Unesi validan Email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Latitude je obavezno polje.")]
            [Display(Name = "Latitude")]
            [StringLength(100, ErrorMessage = "Max. length 100 char")]
            public string Latitude { get; set; }
            [Required(ErrorMessage = "Longitude je obavezno polje.")]
            [Display(Name = "Longitude")]
            [StringLength(100, ErrorMessage = "Max. length 100 char")]
            public string Longitude { get; set; }
            [Display(Name = "Grad")]
            public int GradId { get; set; }
        }

        public class CentarListItemVM : CentarVM
        {
            public string Grad { get; set; }

        }
        public class CentarCreateVM
        {
            public CentarVM CentarVM { get; set; }
            public List<SelectListItem> Gradovi { get; set; }
        }
    }
}
