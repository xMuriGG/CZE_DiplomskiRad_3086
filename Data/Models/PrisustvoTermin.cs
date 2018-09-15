using System;
using System.Collections.Generic;
using System.Text;

namespace CZE.Data.Models
{
    public class PrisustvoTermin
    {
        public int PrisustvoTerminId { get; set; }
        public int Casova { get; set; }
        public DateTime DateTime { get; set; }

        public int GrupaId { get; set; }
        public Grupa Grupa { get; set; }
        public List<Prisustvo> Prisustva { get; set; }
    }
}
