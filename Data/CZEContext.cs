using System;
using System.Collections.Generic;
using System.Text;
using CZE.Data.Migrations;
using CZE.Data.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Slika = CZE.Data.Models.Slika;

namespace CZE.Data
{
    public class CZEContext : DbContext
    {
        public DbSet<Osoba> Osobe { get; set; }
        public DbSet<Zaposlenik> Zaposlenici { get; set; }
        public DbSet<Kandidat> Kandidati { get; set; }
        public DbSet<Grad> Gradovi { get; set; }
        public DbSet<Centar> Centri { get; set; }
        public DbSet<KursKategorija> KursKategorije { get; set; }
        public DbSet<KursTip> KursTipovi { get; set; }
        public DbSet<Kurs> Kursevi { get; set; }
        public DbSet<Slika> Slike { get; set; }
        public DbSet<Grupa> Grupe { get; set; }
        public DbSet<GrupaKandidati> GrupaKandidati { get; set; }
        public DbSet<UplataKandidata> UplateKandidata { get; set; }
        public DbSet<PrisustvoTermin> PrisustvoTermini { get; set; }
        public DbSet<Prisustvo> Prisustva { get; set; }
        public DbSet<KorisnickiNalog> KorisnickiNalozi { get; set; }
        public DbSet<AutorizacijskiToken> AutorizacijskiTokeni { get; set; }
        public DbSet<Ocjena> Ocjene { get; set; }


        public CZEContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Osoba>(entity =>
            {
                entity.ToTable("Osobe");
                entity.Property(p => p.Ime).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Prezime).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Spol).IsRequired();
                entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Adresa).IsRequired().HasMaxLength(100);
                entity.Property(p => p.BrojMobitela).IsRequired().HasMaxLength(15);
                entity.Property(p => p.BrojTelefona).HasMaxLength(15);
                entity.Property(p => p.KakoSteSaznaliZaNas).HasMaxLength(500);
                entity.Property(p => p.DatumRodjenja).HasColumnType("date");
                //firma
                entity.Property(p => p.NazivFirme).HasMaxLength(100);
                entity.Property(p => p.AdresaFirme).HasMaxLength(100);
                entity.Property(p => p.BrojTelefonaFirme).HasMaxLength(15);
                entity.Property(p => p.EmailFirme).HasMaxLength(100);
                //indexes
                entity.HasIndex(i => i.Email).IsUnique();
                entity.HasIndex(i => new { i.Ime, i.Prezime });
                //relacije
                entity.HasOne(e => e.MjestoRodjenja).WithMany().HasForeignKey(f => f.MjestoRodjenjaId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.MjestoBoravka).WithMany().HasForeignKey(f => f.MjestoBoravkaId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.GradFirma).WithMany().HasForeignKey(f => f.GradFirmeId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Grad>(entity =>
            {
                entity.ToTable("Gradovi");
                entity.Property(p => p.Naziv).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<Kandidat>(entity =>
            {
                entity.ToTable("Kandidati");
                entity.Property(p => p.KandidatId).ValueGeneratedNever();
                entity.Property(p => p.DatumUpisa).HasDefaultValueSql("getdate()");
                entity.Property(p => p.Biljeske).HasMaxLength(250);
                //relacije
                entity.HasOne(e => e.Osoba).WithOne(e => e.Kandidat)
                .HasForeignKey<Kandidat>(k => k.KandidatId).OnDelete(DeleteBehavior.Cascade);

            });
            modelBuilder.Entity<Zaposlenik>(entity =>
            {
                entity.ToTable("Zaposlenici");
                entity.Property(p => p.ZaposlenikId).ValueGeneratedNever();
                entity.Property(p => p.BrojLicneKarte).HasMaxLength(9).IsRequired();
                entity.Property(p => p.BrojRacuna).HasMaxLength(20).IsRequired();//https://www.cbbh.ba/Content/Read/609?lang=bs
                //relacije
                entity.HasOne(e => e.Osoba).WithOne(e => e.Zaposlenik)
                .HasForeignKey<Zaposlenik>(z => z.ZaposlenikId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Centar>(entity =>
            {
                entity.ToTable("Centri");
                entity.Property(p => p.Naziv).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Adresa).IsRequired().HasMaxLength(100);
                entity.Property(p => p.BrojMobitela).HasMaxLength(15);
                entity.Property(p => p.BrojTelefona).HasMaxLength(15);
                entity.Property(p => p.Latitude).HasMaxLength(100);
                entity.Property(p => p.Longitude).HasMaxLength(100);
            });

            modelBuilder.Entity<KursKategorija>(entity =>
            {
                entity.ToTable("KursKategorije");
                entity.Property(p => p.Naziv).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<KursTip>(entity =>
            {
                entity.ToTable("KursTipovi");
                entity.Property(p => p.Naziv).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Opis).IsRequired().HasMaxLength(4000);
                entity.Property(p => p.Cijena).HasColumnType("decimal(7,2)");

                entity.HasOne(e => e.KursKategorija).WithMany().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Kurs>(entity =>
            {
                entity.ToTable("Kursevi");
                entity.Property(p => p.Naziv).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Opis).HasMaxLength(4000);

                entity.HasOne(e => e.KursTip).WithMany().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Grupa>(entity =>
            {
                entity.ToTable("Grupe");
                entity.Property(p => p.Naziv).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Cijena).HasColumnType("decimal(7,2)");
                entity.Property(p => p.Pocetak).HasColumnType("smalldatetime");
                entity.Property(p => p.Kraj).HasColumnType("smalldatetime");
                
                entity.HasOne(e => e.Zaposlenik).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Kurs).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Centar).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Slika).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Slika>(entity =>
            {
                entity.ToTable("Slike");
                entity.Property(p => p.Naziv).HasMaxLength(100);
                entity.Property(p => p.SlikaUrl).HasMaxLength(2100);
                entity.Property(p => p.SlikaFile).HasMaxLength(1048576);
                entity.Property(p => p.SlikaThumb).HasMaxLength(524288);

                entity.HasOne(e => e.KursKategorija).WithMany().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<GrupaKandidati>(entity =>
            {
                entity.ToTable("Grupa_Kandidati");
                entity.Property(p => p.Odobren).HasDefaultValue(true);
               
                entity.HasAlternateKey(e => new {e.KandidatId, e.GrupaId});
                entity.HasOne(e => e.Kandidat).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Grupa).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<UplataKandidata>(entity =>
            {
                entity.ToTable("UplateKandidata");
                entity.Property(p => p.Kolicina).HasColumnType("decimal(7,2)");
                entity.Property(p => p.Biljeske).HasMaxLength(1000);
                entity.Property(p => p.DatumUplate).HasColumnType("smalldatetime").HasDefaultValueSql("getdate()");

                entity.HasOne(e => e.GrupaKandidati).WithMany().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Zaposlenik).WithMany().OnDelete(DeleteBehavior.ClientSetNull);

            });
            modelBuilder.Entity<PrisustvoTermin>(entity =>
            {
                entity.ToTable("PrisustvoTermin");
                entity.Property(p=>p.DateTime).HasColumnType("smalldatetime").HasDefaultValueSql("getdate()");

                entity.HasOne(e => e.Grupa).WithMany().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Prisustva).WithOne(e => e.PrisustvoTermin).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Prisustvo>(entity =>
            {
                entity.ToTable("Prisustva");
                
            });
            modelBuilder.Entity<KorisnickiNalog>(entity =>
            {
                entity.ToTable("KorisnickiNalozi");
                entity.Property(p => p.KorisnickiNalogId).ValueGeneratedNever();
                entity.Property(p => p.LozinkaHash).IsRequired();
                entity.Property(p => p.KorisnickoIme).IsRequired().HasMaxLength(256);
                entity.Property(p => p.LozinkaSalt).IsRequired().HasMaxLength(128);

                entity.HasOne(e => e.Osoba).WithOne(e => e.KorisnickiNalog)
                .HasForeignKey<KorisnickiNalog>(k=>k.KorisnickiNalogId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<AutorizacijskiToken>(entity =>
            {
                entity.ToTable("AutorizacijskiTokeni");
                entity.Property(p => p.Vrijednost).IsRequired();
                entity.Property(p => p.VrijemeEvidentiranja).HasColumnType("smalldatetime").HasDefaultValueSql("getdate()");

                entity.HasOne(e => e.KorisnickiNalog).WithMany().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Ocjena>(entity =>
            {
                entity.ToTable("Ocjene");

                entity.Property(p => p.Opis).HasMaxLength(300);
                entity.Property(p => p.Silenced).HasDefaultValue(false);
                entity.Property(p => p.DatumOcjene).HasColumnType("smalldatetime").HasDefaultValueSql("getdate()");

                entity.HasOne(e => e.GrupaKandidati).WithMany().OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
