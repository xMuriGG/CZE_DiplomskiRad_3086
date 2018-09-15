using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CZE.Data.Models
{
    public class Zaposlenik
    {
        public int ZaposlenikId { get; set; }
        public StepeniObrazovanja StepenObrazovanja { get; set; }
        public string BrojLicneKarte { get; set; }
        public string BrojRacuna { get; set; }


        public Osoba Osoba { get; set; }

        public enum StepeniObrazovanja { SSS, VKV, VŠS, VSS, Bachelor, Magistratura, Doktorat }
    }
}
