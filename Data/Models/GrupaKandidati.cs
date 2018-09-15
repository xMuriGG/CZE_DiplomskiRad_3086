using System;
using System.Collections.Generic;
using System.Text;

namespace CZE.Data.Models
{
    public class GrupaKandidati
    {
        public int GrupaKandidatiId { get; set; }
        public int KandidatId { get; set; }
        public  Kandidat Kandidat { get; set; }
        public int GrupaId { get; set; }
        public  Grupa Grupa { get; set; }
        public bool Odobren { get; set; }
       
    }
}
