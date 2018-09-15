using CZE.Web.Areas.Direktor.Models;

namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class OsobaEditVM
    {
        public OsobaCreateVM OsobaCreateVM { get; set; }
        public KandidatVMs.KandidatCreateVM KandidatCreateVM { get; set; }
        public ZaposlenikVMs.ZaposlenikCreateVM ZaposlenikCreateVM { get; set; }
    }
}
