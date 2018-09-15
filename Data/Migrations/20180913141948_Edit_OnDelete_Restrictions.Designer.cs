﻿// <auto-generated />
using CZE.Data;
using CZE.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CZE.Data.Migrations
{
    [DbContext(typeof(CZEContext))]
    [Migration("20180913141948_Edit_OnDelete_Restrictions")]
    partial class Edit_OnDelete_Restrictions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CZE.Data.Models.AutorizacijskiToken", b =>
                {
                    b.Property<int>("AutorizacijskiTokenId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IpAdresa");

                    b.Property<int>("KorisnickiNalogId");

                    b.Property<string>("Vrijednost")
                        .IsRequired();

                    b.Property<DateTime>("VrijemeEvidentiranja")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smalldatetime")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("AutorizacijskiTokenId");

                    b.HasIndex("KorisnickiNalogId");

                    b.ToTable("AutorizacijskiTokeni");
                });

            modelBuilder.Entity("CZE.Data.Models.Centar", b =>
                {
                    b.Property<int>("CentarId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adresa")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("BrojMobitela")
                        .HasMaxLength(15);

                    b.Property<string>("BrojTelefona")
                        .HasMaxLength(15);

                    b.Property<string>("Email");

                    b.Property<int>("GradId");

                    b.Property<string>("Latitude")
                        .HasMaxLength(100);

                    b.Property<string>("Longitude")
                        .HasMaxLength(100);

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("CentarId");

                    b.HasIndex("GradId");

                    b.ToTable("Centri");
                });

            modelBuilder.Entity("CZE.Data.Models.Grad", b =>
                {
                    b.Property<int>("GradId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("GradId");

                    b.ToTable("Gradovi");
                });

            modelBuilder.Entity("CZE.Data.Models.Grupa", b =>
                {
                    b.Property<int>("GrupaId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Casova");

                    b.Property<int>("CentarId");

                    b.Property<decimal?>("Cijena")
                        .HasColumnType("decimal(7,2)");

                    b.Property<DateTime?>("Kraj")
                        .HasColumnType("smalldatetime");

                    b.Property<int>("KursId");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("Pocetak")
                        .HasColumnType("smalldatetime");

                    b.Property<int>("SlikaId");

                    b.Property<int>("Status");

                    b.Property<int>("ZaposlenikId");

                    b.HasKey("GrupaId");

                    b.HasIndex("CentarId");

                    b.HasIndex("KursId");

                    b.HasIndex("SlikaId");

                    b.HasIndex("ZaposlenikId");

                    b.ToTable("Grupe");
                });

            modelBuilder.Entity("CZE.Data.Models.GrupaKandidati", b =>
                {
                    b.Property<int>("GrupaKandidatiId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GrupaId");

                    b.Property<int>("KandidatId");

                    b.Property<bool>("Odobren")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.HasKey("GrupaKandidatiId");

                    b.HasAlternateKey("KandidatId", "GrupaId");

                    b.HasIndex("GrupaId");

                    b.ToTable("Grupa_Kandidati");
                });

            modelBuilder.Entity("CZE.Data.Models.Kandidat", b =>
                {
                    b.Property<int>("KandidatId");

                    b.Property<string>("Biljeske")
                        .HasMaxLength(250);

                    b.Property<DateTime>("DatumUpisa")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("KandidatId");

                    b.ToTable("Kandidati");
                });

            modelBuilder.Entity("CZE.Data.Models.KorisnickiNalog", b =>
                {
                    b.Property<int>("KorisnickiNalogId");

                    b.Property<bool>("Aktivan");

                    b.Property<string>("KorisnickoIme")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("LozinkaHash")
                        .IsRequired();

                    b.Property<string>("LozinkaSalt")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int>("UlogaKorisnika");

                    b.HasKey("KorisnickiNalogId");

                    b.ToTable("KorisnickiNalozi");
                });

            modelBuilder.Entity("CZE.Data.Models.Kurs", b =>
                {
                    b.Property<int>("KursId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("KursTipId");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Opis")
                        .HasMaxLength(1000);

                    b.HasKey("KursId");

                    b.HasIndex("KursTipId");

                    b.ToTable("Kursevi");
                });

            modelBuilder.Entity("CZE.Data.Models.KursKategorija", b =>
                {
                    b.Property<int>("KursKategorijaId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("KursKategorijaId");

                    b.ToTable("KursKategorije");
                });

            modelBuilder.Entity("CZE.Data.Models.KursTip", b =>
                {
                    b.Property<int>("KursTipId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Casova");

                    b.Property<decimal>("Cijena")
                        .HasColumnType("decimal(7,2)");

                    b.Property<int>("KursKategorijaId");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("KursTipId");

                    b.HasIndex("KursKategorijaId");

                    b.ToTable("KursTipovi");
                });

            modelBuilder.Entity("CZE.Data.Models.Ocjena", b =>
                {
                    b.Property<int>("OcjenaId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DatumOcjene")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smalldatetime")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("GrupaKandidatiId");

                    b.Property<string>("Opis")
                        .HasMaxLength(300);

                    b.Property<bool>("Silenced")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<int>("Vrijednost");

                    b.HasKey("OcjenaId");

                    b.HasIndex("GrupaKandidatiId");

                    b.ToTable("Ocjene");
                });

            modelBuilder.Entity("CZE.Data.Models.Osoba", b =>
                {
                    b.Property<int>("OsobaId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adresa")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("AdresaFirme")
                        .HasMaxLength(100);

                    b.Property<string>("BrojMobitela")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("BrojTelefona")
                        .HasMaxLength(15);

                    b.Property<string>("BrojTelefonaFirme")
                        .HasMaxLength(15);

                    b.Property<DateTime>("DatumRodjenja")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("EmailFirme")
                        .HasMaxLength(100);

                    b.Property<int?>("GradFirmeId");

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("KakoSteSaznaliZaNas")
                        .HasMaxLength(500);

                    b.Property<int>("MjestoBoravkaId");

                    b.Property<int>("MjestoRodjenjaId");

                    b.Property<string>("NazivFirme")
                        .HasMaxLength(100);

                    b.Property<bool>("PodatciOFirmi");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Spol");

                    b.HasKey("OsobaId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("GradFirmeId");

                    b.HasIndex("MjestoBoravkaId");

                    b.HasIndex("MjestoRodjenjaId");

                    b.HasIndex("Ime", "Prezime");

                    b.ToTable("Osobe");
                });

            modelBuilder.Entity("CZE.Data.Models.Prisustvo", b =>
                {
                    b.Property<int>("PrisustvoId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GrupaKandidatiId");

                    b.Property<int>("PrisustvoTerminId");

                    b.Property<bool>("Prisutan");

                    b.HasKey("PrisustvoId");

                    b.HasIndex("GrupaKandidatiId");

                    b.HasIndex("PrisustvoTerminId");

                    b.ToTable("Prisustva");
                });

            modelBuilder.Entity("CZE.Data.Models.PrisustvoTermin", b =>
                {
                    b.Property<int>("PrisustvoTerminId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Casova");

                    b.Property<DateTime>("DateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smalldatetime")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("GrupaId");

                    b.HasKey("PrisustvoTerminId");

                    b.HasIndex("GrupaId");

                    b.ToTable("PrisustvoTermin");
                });

            modelBuilder.Entity("CZE.Data.Models.Slika", b =>
                {
                    b.Property<int>("SlikaId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("KursKategorijaId");

                    b.Property<string>("Naziv")
                        .HasMaxLength(100);

                    b.Property<byte[]>("SlikaFile")
                        .HasMaxLength(1048576);

                    b.Property<byte[]>("SlikaThumb")
                        .HasMaxLength(524288);

                    b.Property<string>("SlikaUrl")
                        .HasMaxLength(2100);

                    b.HasKey("SlikaId");

                    b.HasIndex("KursKategorijaId");

                    b.ToTable("Slike");
                });

            modelBuilder.Entity("CZE.Data.Models.UplataKandidata", b =>
                {
                    b.Property<int>("UplataKandidataId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Biljeske")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("DatumUplate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smalldatetime")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("GrupaKandidatiId");

                    b.Property<decimal>("Kolicina")
                        .HasColumnType("decimal(7,2)");

                    b.Property<int?>("ZaposlenikId");

                    b.HasKey("UplataKandidataId");

                    b.HasIndex("GrupaKandidatiId");

                    b.HasIndex("ZaposlenikId");

                    b.ToTable("UplateKandidata");
                });

            modelBuilder.Entity("CZE.Data.Models.Zaposlenik", b =>
                {
                    b.Property<int>("ZaposlenikId");

                    b.Property<string>("BrojLicneKarte")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.Property<string>("BrojRacuna")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("StepenObrazovanja");

                    b.HasKey("ZaposlenikId");

                    b.ToTable("Zaposlenici");
                });

            modelBuilder.Entity("CZE.Data.Models.AutorizacijskiToken", b =>
                {
                    b.HasOne("CZE.Data.Models.KorisnickiNalog", "KorisnickiNalog")
                        .WithMany()
                        .HasForeignKey("KorisnickiNalogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.Centar", b =>
                {
                    b.HasOne("CZE.Data.Models.Grad", "Grad")
                        .WithMany()
                        .HasForeignKey("GradId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.Grupa", b =>
                {
                    b.HasOne("CZE.Data.Models.Centar", "Centar")
                        .WithMany()
                        .HasForeignKey("CentarId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CZE.Data.Models.Kurs", "Kurs")
                        .WithMany()
                        .HasForeignKey("KursId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CZE.Data.Models.Slika", "Slika")
                        .WithMany()
                        .HasForeignKey("SlikaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CZE.Data.Models.Zaposlenik", "Zaposlenik")
                        .WithMany()
                        .HasForeignKey("ZaposlenikId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CZE.Data.Models.GrupaKandidati", b =>
                {
                    b.HasOne("CZE.Data.Models.Grupa", "Grupa")
                        .WithMany()
                        .HasForeignKey("GrupaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CZE.Data.Models.Kandidat", "Kandidat")
                        .WithMany()
                        .HasForeignKey("KandidatId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CZE.Data.Models.Kandidat", b =>
                {
                    b.HasOne("CZE.Data.Models.Osoba", "Osoba")
                        .WithOne("Kandidat")
                        .HasForeignKey("CZE.Data.Models.Kandidat", "KandidatId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.KorisnickiNalog", b =>
                {
                    b.HasOne("CZE.Data.Models.Osoba", "Osoba")
                        .WithOne("KorisnickiNalog")
                        .HasForeignKey("CZE.Data.Models.KorisnickiNalog", "KorisnickiNalogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.Kurs", b =>
                {
                    b.HasOne("CZE.Data.Models.KursTip", "KursTip")
                        .WithMany()
                        .HasForeignKey("KursTipId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.KursTip", b =>
                {
                    b.HasOne("CZE.Data.Models.KursKategorija", "KursKategorija")
                        .WithMany()
                        .HasForeignKey("KursKategorijaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.Ocjena", b =>
                {
                    b.HasOne("CZE.Data.Models.GrupaKandidati", "GrupaKandidati")
                        .WithMany()
                        .HasForeignKey("GrupaKandidatiId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.Osoba", b =>
                {
                    b.HasOne("CZE.Data.Models.Grad", "GradFirma")
                        .WithMany()
                        .HasForeignKey("GradFirmeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CZE.Data.Models.Grad", "MjestoBoravka")
                        .WithMany()
                        .HasForeignKey("MjestoBoravkaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CZE.Data.Models.Grad", "MjestoRodjenja")
                        .WithMany()
                        .HasForeignKey("MjestoRodjenjaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CZE.Data.Models.Prisustvo", b =>
                {
                    b.HasOne("CZE.Data.Models.GrupaKandidati", "GrupaKandidati")
                        .WithMany()
                        .HasForeignKey("GrupaKandidatiId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CZE.Data.Models.PrisustvoTermin", "PrisustvoTermin")
                        .WithMany("Prisustva")
                        .HasForeignKey("PrisustvoTerminId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.PrisustvoTermin", b =>
                {
                    b.HasOne("CZE.Data.Models.Grupa", "Grupa")
                        .WithMany()
                        .HasForeignKey("GrupaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.Slika", b =>
                {
                    b.HasOne("CZE.Data.Models.KursKategorija", "KursKategorija")
                        .WithMany()
                        .HasForeignKey("KursKategorijaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CZE.Data.Models.UplataKandidata", b =>
                {
                    b.HasOne("CZE.Data.Models.GrupaKandidati", "GrupaKandidati")
                        .WithMany()
                        .HasForeignKey("GrupaKandidatiId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CZE.Data.Models.Zaposlenik", "Zaposlenik")
                        .WithMany()
                        .HasForeignKey("ZaposlenikId");
                });

            modelBuilder.Entity("CZE.Data.Models.Zaposlenik", b =>
                {
                    b.HasOne("CZE.Data.Models.Osoba", "Osoba")
                        .WithOne("Zaposlenik")
                        .HasForeignKey("CZE.Data.Models.Zaposlenik", "ZaposlenikId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}