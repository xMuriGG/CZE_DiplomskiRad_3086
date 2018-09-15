using CZE.Data;
using CZE.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.Util.Helper
{
    public static class Autentifikacija
    {
        private const string LogiraniKorisnik = "logirani_korisnik";

        public static async Task SetLogiraniKorisnik(this HttpContext context, KorisnickiNalog nalog, bool snimiUCookie = false)
        {

            if (snimiUCookie)
            {
                CZEContext db = context.RequestServices.GetService<CZEContext>();

                string stariToken = context.Request.GetCookieJson<string>(LogiraniKorisnik);
                if (stariToken != null)
                {
                    AutorizacijskiToken obrisati = db.AutorizacijskiTokeni.FirstOrDefault(x => x.Vrijednost == stariToken);
                    if (obrisati != null)
                    {
                        db.AutorizacijskiTokeni.Remove(obrisati);
                        await db.SaveChangesAsync();
                    }
                }

                if (nalog != null)
                {
                    string ip = context.Connection.RemoteIpAddress.ToString();
                    string token = Guid.NewGuid().ToString();
                    db.AutorizacijskiTokeni.Add(new AutorizacijskiToken
                    {
                        Vrijednost = token,
                        KorisnickiNalogId = nalog.KorisnickiNalogId,
                        IpAdresa = ip,
                        VrijemeEvidentiranja = DateTime.Now
                    });
                    await db.SaveChangesAsync();
                    context.Response.SetCookieJson(LogiraniKorisnik, token);
                }
            }
            else
            {
                context.Session.Set(LogiraniKorisnik, nalog);
            }
        }
        public static async Task<KorisnickiNalog> GetLogiraniKorisnik(this HttpContext context)
        {
            var sesija = context.Session.Get<KorisnickiNalog>(LogiraniKorisnik);
            if (sesija!=null)
            {
                return sesija;
            }

            CZEContext db = context.RequestServices.GetService<CZEContext>();

            string token = context.Request.GetCookieJson<string>(LogiraniKorisnik);
            if (token == null)
                return null;

            return await db.AutorizacijskiTokeni
                .Where(x => x.Vrijednost == token)
                .Select(s => s.KorisnickiNalog)
                .SingleOrDefaultAsync();

        }
        public static async Task RemoveLogiraniKorisnik(this HttpContext context)
        {
            CZEContext db = context.RequestServices.GetService<CZEContext>();
            context.Session.Remove(LogiraniKorisnik);
            AutorizacijskiToken obrisati = db.AutorizacijskiTokeni.FirstOrDefault(x => x.Vrijednost == context.GetTrenutniToken());
            if (obrisati != null)
            {
                try
                {
                    db.AutorizacijskiTokeni.Remove(obrisati);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }

        public static string GenerateRandomPassword(int length=12)
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, length);
        }
        public static string GetTrenutniToken(this HttpContext context)
        {
            return context.Request.GetCookieJson<string>(LogiraniKorisnik);
        }
    }
}