using System;
using System.Collections.Generic;
using System.Text;

namespace CZE.Data.Models
{
    public class Prisustvo
    {
        public int PrisustvoId { get; set; }
        public bool Prisutan { get; set; }

        public int PrisustvoTerminId { get; set; }
        public PrisustvoTermin PrisustvoTermin { get; set; }
        public int GrupaKandidatiId { get; set; }
        public GrupaKandidati GrupaKandidati { get; set; }
    }
}
