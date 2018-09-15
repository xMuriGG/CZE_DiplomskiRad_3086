using System;
using System.Collections.Generic;
using System.Text;

namespace CZE.Data.Models
{
   public class Slika
    {
        public int SlikaId { get; set; }
        public string Naziv { get; set; }
        public string SlikaUrl { get; set; }
        public byte[] SlikaFile { get; set; }
        public byte[] SlikaThumb { get; set; }

        public int KursKategorijaId { get; set; }
        public KursKategorija KursKategorija { get; set; }
    }
}
