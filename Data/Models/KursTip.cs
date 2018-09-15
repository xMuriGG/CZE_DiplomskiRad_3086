using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZE.Data.Models
{
   public class KursTip
    {
        public int KursTipId { get; set; }
        public string Naziv { get; set; }
        public int Casova { get; set; }
        public decimal Cijena { get; set; }
        public string Opis { get; set; }
        public int KursKategorijaId { get; set; }
        public KursKategorija KursKategorija { get; set; }
    }
}
