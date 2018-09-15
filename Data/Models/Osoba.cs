using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CZE.Data.Models
{
    public class Osoba
    {
        public int OsobaId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public int MjestoRodjenjaId { get; set; }
        public eSpol Spol { get; set; }
        public string Email { get; set; }
        public int MjestoBoravkaId { get; set; }
        public string Adresa { get; set; }
        public string BrojMobitela { get; set; }
        public string BrojTelefona { get; set; }
        public string KakoSteSaznaliZaNas { get; set; }

        //Podaci o firmi
        public bool PodatciOFirmi { get; set; }
        public string NazivFirme { get; set; }
        public string AdresaFirme { get; set; }
        public int? GradFirmeId { get; set; }
        public string BrojTelefonaFirme { get; set; }
        public string EmailFirme { get; set; }
            


        #region Navigation properties
        public Grad MjestoRodjenja { get; set; }     
        public Grad MjestoBoravka { get; set; }     
        public Grad GradFirma { get; set; }     
        public Kandidat Kandidat{ get; set; }
        public Zaposlenik Zaposlenik { get; set; }
        public KorisnickiNalog KorisnickiNalog { get; set; }

        #endregion


        public enum eSpol { M, Z };
    }
}
