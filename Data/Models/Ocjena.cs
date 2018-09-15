using System;
using System.Collections.Generic;
using System.Text;

namespace CZE.Data.Models
{
    public class Ocjena
    {
        public int OcjenaId { get; set; }
        public int Vrijednost { get; set; }
        public string Opis { get; set; }
        public bool Silenced { get; set; }
        public DateTime DatumOcjene { get; set; }

        public int GrupaKandidatiId { get; set; }
        public GrupaKandidati GrupaKandidati { get; set; }
    }
    
}
