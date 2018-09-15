using System;
using System.Collections.Generic;
using System.Text;

namespace CZE.Data.Models
{
    public class UplataKandidata
    {
        public int UplataKandidataId { get; set; }
        public decimal Kolicina { get; set; }
        public string Biljeske { get; set; }
        public DateTime DatumUplate { get; set; }

        public int GrupaKandidatiId { get; set; }
        public virtual GrupaKandidati GrupaKandidati { get; set; }
        public int ZaposlenikId { get; set; }
        public virtual Zaposlenik Zaposlenik { get; set; }
    }
}
