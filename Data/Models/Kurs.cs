using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZE.Data.Models
{
    public class Kurs
    {
        public int KursId { get; set; }

        public int KursTipId { get; set; }
        public KursTip KursTip { get; set; }
        public string Naziv { get; set; }    
        public string Opis { get; set; }

    }
}
