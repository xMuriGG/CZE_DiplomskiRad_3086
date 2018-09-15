using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Web.Areas.AdministrativniRadnik.Models;

namespace CZE.Web.Models
{
    public class GrupaIndexVM
    {
        public List<GrupaVMs.GrupaCardListItemVM> GrupaClientListItemVM { get; set; }
        public List<KursKategorijaSF> KursKategorijeVM { get; set; }
    }
    public class KursKategorijaSF
    {
        public int KursKategorijaId { get; set; }
        public string Naziv { get; set; }
        public bool Checked { get; set; }
    }
}
