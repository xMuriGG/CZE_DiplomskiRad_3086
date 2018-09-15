using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CZE.Data.Models
{
    public class Centar
    {
        public int CentarId { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string BrojMobitela { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int GradId { get; set; }
        public Grad Grad { get; set; }
    }
}
