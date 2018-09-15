using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data.Models;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CZE.Web.Areas.Predavac.Models
{
    public class PrisustvaVMs
    {
        public class PrisustvoTerminVM
        {
            public int PrisustvoTerminId { get; set; }
            [Display(Name = "Časova")]
            [Required(ErrorMessage = "Broj časova je obavezno polje.")]
            public int Casova { get; set; }
            [Required(ErrorMessage = "Datum i vrijeme termina je obavezno polje.")]
            public DateTime DateTime { get; set; }
            [Required(ErrorMessage = "Grupa termina je obavezno polje.")]
            public int GrupaId { get; set; }
            public List<Prisustvo> Prisustva { get; set; }

        }
        public class KandidatPrisustvoVM
        {
            public int KandidatId { get; set; }
            public string OsobaNaziv { get; set; }
            public int GrupaKandidatiId { get; set; }
        }
        public class PrisustvoIndexVM
        {
            public PrisustvoTerminCreateVM PrisustvoTerminCreateVM { get; set; }
            public PrisustvoTableVM PrisustvoTableVM { get; set; }
        }
        public class PrisustvoTerminCreateVM
        {
            public int GrupaId { get; set; }
            public PrisustvoTerminVM PrisustvoTerminVM { get; set; }
        }
        public class PrisustvoTableVM
        {
            public int GrupaId { get; set; }
            public List<KandidatPrisustvoVM> KandidatiPrisustvoVM { get; set; }
            public List<PrisustvoTerminVM> PrisustvoTerminiVM { get; set; }

        }
       
    }
}
