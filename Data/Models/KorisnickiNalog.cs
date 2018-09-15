using System;
using System.Collections.Generic;
using System.Text;

namespace CZE.Data.Models
{
    public class KorisnickiNalog
    {
        public int KorisnickiNalogId { get; set; }
        public string KorisnickoIme { get; set; }
        public string LozinkaHash { get; set; }
        public string LozinkaSalt { get; set; }
        public bool Aktivan { get; set; }
        public UlogeKorisnika UlogaKorisnika { get; set; }

        public Osoba Osoba { get; set; }
    }
    public enum UlogeKorisnika { Administrator, Direktor, AdministrativniRadnik, Predavac, Kandidat }
}
