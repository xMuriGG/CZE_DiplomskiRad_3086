using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Web.Areas.Direktor.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CZE.Web.Models
{
    public class KlijentskiPortalVMs
    {
        public List<CentarVMs.CentarListItemVM> CentariVm { get; set; }
    }
}
