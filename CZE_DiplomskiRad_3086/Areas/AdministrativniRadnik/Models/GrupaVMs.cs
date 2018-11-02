using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CZE.Web.Areas.AdministrativniRadnik.Models
{
    public class GrupaVMs
    {
        public class GrupaVM
        {
            public int GrupaId { get; set; }
            [Required(ErrorMessage = "Naziv grupe je obavezno polje.")]
            [MaxLength(100, ErrorMessage = "Maximalna dužina naziva je 100 karaktera.")]
            public string Naziv { get; set; }
            [Required(ErrorMessage = "Datum početka kursa je obavezno polje.")]
            [Display(Name = "Početak")]
            public DateTime Pocetak { get; set; }
            public DateTime? Kraj { get; set; }
            [Display(Name = "Časova")]
            public int? Casova { get; set; }
            public decimal? Cijena { get; set; }
            public GrupaStatus Status { get; set; }
            [Required(ErrorMessage = "Kurs grupe je obavezno polje.")]
            public int KursId { get; set; }
            [Required(ErrorMessage = "Predavač grupe je obavezno polje.")]
            [Display(Name = "Predavač")]
            public int ZaposlenikId { get; set; }
            [Required(ErrorMessage = "Centar grupe je obavezno polje.")]
            [Display(Name = "Centar")]
            public int CentarId { get; set; }
            [Required(ErrorMessage = "Slika grupe je obavezno polje.")]
            [Display(Name = "Slika")]
            public int SlikaId { get; set; }
        }

        public class GrupaCreateVM
        {
            public GrupaVM GrupaVM { get; set; }
            public List<SelectListItem> Kursevi { get; set; }
            public List<SelectListItem> Slike { get; set; }
            public List<SelectListItem> Predavaci { get; set; }
            public List<SelectListItem> Centri { get; set; }

        }
        public class GrupaDetailsVM:GrupaVM
        {
            [Display(Name = "Predavač")]
            public string ZaposlenikNaziv { get; set; }
            [Display(Name = "Lokacija centra")]
            public string CentarGrad { get; set; }
            public string CentarNaziv { get; set; }
            //public byte[] Slika { get; set; }
            //public string SlikaUrl { get; set; }
            public bool IsLogiraniKorisnikKandidat { get; set; }
            public bool LogiraniKandidatPrijavljen { get; set; }
            public bool Odobren { get; set; }

            public KursVMs.DetailsVM KursDetails { get; set; }
            public double Ocjena { get; set; }
        }
        public class GrupaListItemVM
        {
            public int GrupaId { get; set; }
            public string Naziv { get; set; }
            [Display(Name = "Početak")]
            public string Pocetak { get; set; }
            public string Kraj { get; set; }
            [Display(Name = "Časova")]
            public string Casova { get; set; }
            public string Cijena { get; set; }

            public GrupaStatus Status { get; set; }
            [Display(Name = "Slika")]
            public byte[] SlikaThumb { get; set; }
            public string SlikaUrl { get; set; }
            public int KursId { get; set; }
            [Display(Name = "Kurs")]
            public string KursInfo { get; set; }
            
            public int ZaposlenikId { get; set; }
            [Display(Name = "Predavač")]
            public string ZaposlenikNaziv { get; set; }
            public int CentarId { get; set; }
            public string Centar { get; set; }
            public int SlikaId { get; set; }
            public string StatusText { get; set; }
        }
        public class GrupaCardListItemVM
        {
            public int GrupaId { get; set; }
            public string Naziv { get; set; }
            [Display(Name = "Početak")]
            public string Pocetak { get; set; }
            public string Kraj { get; set; }
            [Display(Name = "Časova")]
            public int Casova { get; set; }
            public string Cijena { get; set; }
            [Display(Name = "Slika")]
            public byte[] Slika { get; set; }

            public string KursKategorijaNaziv { get; set; }
            public string KursTipNaziv { get; set; }
            public string KursNaziv { get; set; }
           
            [Display(Name = "Predavač")]
            public string ZaposlenikNaziv { get; set; }
            public int CentarId { get; set; }
            public string CentarLokacija { get; set; }
            public int KandidataPrijavljeno { get; set; }
            public double Ocjena { get; set; }
            public string SlikaUrl { get; internal set; }
        }
        public class GrupaKandidataVM
        {
            public int GrupaId { get; set; }
            public string Naziv { get; set; }
            [Display(Name = "Početak")]
            public string Pocetak { get; set; }

            public int KursKategorijaId { get; set; }
            [Display(Name = "Kategorija")]
            public string KursKategorijaNaziv { get; set; }
            public int KursTipId { get; set; }
            [Display(Name = "Tip")]
            public string KursTipNaziv { get; set; }
            public int KursId { get; set; }
            [Display(Name = "Modul")]
            public string KursNaziv { get; set; }

            public int ZaposlenikId { get; set; }
            [Display(Name = "Predavač")]
            public string ZaposlenikNaziv { get; set; }

            public int CentarId { get; set; }
            [Display(Name = "Centar")]
            public string CentarLokacija { get; set; }


            public GrupaStatus Status { get; set; }
            public string Cijena { get; set; }
            public bool Odobren { get; set; }
            [Display(Name = "Uplačeno")]
            public string Uplaceno { get; set; }
            public bool Polozio { get; set; }
            public int GrupaKandidatiId { get; set; }
            public Ocjena Ocjena { get; internal set; }
        }

        public class SlikaVM
        {
            public int SlikaId { get; set; }
            [Required(ErrorMessage = "Naziv slike je obavezno polje.")]
            [MaxLength(100, ErrorMessage = "Maximalna dužina naziva je 100 karaktera.")]
            public string Naziv { get; set; }
            [MaxLength(2100, ErrorMessage = "Maximalna dužina url-a je 2100 karaktera.")]
            public string SlikaUrl { get; set; }
            //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
            //[MaxLength(1048576, ErrorMessage = "Maximalna težina slike je 1MB karaktera.")]
            //[DataType(DataType.Upload)]
            //[FileExtensions(Extensions =".jpg" ,ErrorMessage = "Only Image files allowed." )]
            public IFormFile SlikaFile { get; set; }
            [Required(ErrorMessage = "Kategorija slike je obavezno polje.")]
            [Display(Name = "Kategorija")]
            public int KursKategorijaId { get; set; }
        }
        public class SlikaCreateVM
        {
            public SlikaVM SlikaVM { get; set; }
            public List<SelectListItem> KursKategorije { get; set; }
        }
        public class SlikaListItemVM
        {
            public int SlikaId { get; set; }          
            public string Naziv { get; set; }
            public string SlikaUrl { get; set; }
            [Display(Name = "Težina")]
            public float TezinaSlike { get; set; }
            [Display(Name = "Kategorija")]
            public string KursKategorijaNaziv { get; set; }
            public int KursKategorijaId { get; set; }
            public string SlikaThumb { get; set; }
        }


  
        public class GrupaKandidatiListItemVM
        {
            public int GrupaKandidatiId { get; set; }
            public int KandidatId { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Display(Name = "Broj Mobitela")]
            public string BrojMobitela { get; set; }
            [Display(Name = "Broj telefona")]
            public string BrojTelefona { get; set; }
            [Display(Name = "Uplačeno")]
            public string Uplaceno { get; set; }
            public string Cijena { get; set; }
            public bool Odobren { get; set; }
            public Ocjena Ocjena { get; set; }
        }
        public class GrupaKandidatiTableVM
        {
            public int GrupaId { get; set; }
            public string GrupaNaziv { get; set; }
            public List<GrupaKandidatiListItemVM> GrupaKandidati { get; set; }

        }
    }
}
