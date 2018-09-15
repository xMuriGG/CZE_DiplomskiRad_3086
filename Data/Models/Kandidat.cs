using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZE.Data.Models
{
    public class Kandidat
    {
        public int KandidatId { get; set; }
        public DateTime DatumUpisa { get; set; }
        public string Biljeske { get; set; }

  
        public Osoba Osoba { get; set; }
       
    }
}
