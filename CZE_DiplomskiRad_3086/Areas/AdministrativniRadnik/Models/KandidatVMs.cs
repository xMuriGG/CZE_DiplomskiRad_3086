using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class KandidatVMs
    {
        public class KandidatCreateVM
        {
            public int OsobaId { get; set; }
            public string OsobaNaziv { get; set; }
            public KandidatVM KandidatVM { get; set; }
        }
        public class KandidatVM
        {
            public int KandidatId { get; set; }
            [MaxLength(250, ErrorMessage = "Maximalno 250 karaktera")]
            public string Biljeske { get; set; }

            public DateTime DatumUpisa { get; set; }
        }
        public class KandidatListItemVM
        {
            public int KandidatId { get; set; }
            public string ImePrezime { get; set; }
            public string DatumRodjenja { get; set; }
            public string Email { get; set; }
            public string DatumUpisa { get; set; }

        }
    }
}
