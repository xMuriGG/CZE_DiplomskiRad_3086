using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data.Models;

namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class OsobaIndexVM
    {
        public OsobaCreateVM Osoba { get; set; }
        public List<OsobaListItemVM> Osobe { get; set; }
    }
}
