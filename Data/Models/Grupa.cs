using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CZE.Data.Models
{
    public class Grupa
    {
        public int GrupaId { get; set; }
        public string Naziv { get; set; }
        public DateTime Pocetak { get; set; }
        public DateTime? Kraj { get; set; }
        public int? Casova { get; set; }
        public decimal? Cijena { get; set; }
        public GrupaStatus Status { get; set; }
       

        //TODO:DOdati ugovore predavačima(po potrebi)
        public int KursId { get; set; }
        public Kurs Kurs { get; set; }
        public int ZaposlenikId { get; set; }
        public Zaposlenik Zaposlenik { get; set; }
        public int CentarId { get; set; }
        public Centar Centar { get; set; }
        public int SlikaId { get; set; }
        public Slika Slika { get; set; }

    }

    public enum GrupaStatus
    {
        [Display(Name = "Neutralna")]
        Neutralna,
        [Display(Name = "Aktivna")]
        Aktivna,
        [Display(Name = "U toku")]
        UToku,
        [Display(Name = "Završena")]
        Zavrsena
    }
}
