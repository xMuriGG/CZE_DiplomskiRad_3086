using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using CZE.Web.Models;
using CZE.Web.Util.Helper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace CZE.Web.Controllers
{
    public class AutentifikacijaController : Controller
    {
        private readonly CZEContext _db;

        public AutentifikacijaController(CZEContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            ViewData["error_poruka"] = TempData["error_poruka"];
           

            return View(new LoginVM() { ZapamtiMe = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {         
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            KorisnickiNalog nalog = _db.KorisnickiNalozi.FirstOrDefault(f => f.Osoba.Email == model.Email);
            if (nalog == null)
            {
                ModelState.AddModelError("", "Pogrešan email ili password.");
                return View("Index", model);
            }

            string modelPasswordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Lozinka,
                salt: Convert.FromBase64String(nalog.LozinkaSalt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (!nalog.LozinkaHash.Equals(modelPasswordHash, StringComparison.Ordinal))
            {
                ModelState.AddModelError("", "Pogrešan email ili password.");
                return View("Index", model);
            }

            await HttpContext.SetLogiraniKorisnik(nalog, model.ZapamtiMe);

            //Url koji šalje Autorizacija action filter
            var requestUrl = (string)TempData["requestUrl"];
            if (!string.IsNullOrEmpty(requestUrl))
            {
                return Redirect(requestUrl);
            }

            return RedirectToAction("KlijentskiPortalIndex", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.RemoveLogiraniKorisnik();
            return RedirectToAction("KlijentskiPortalIndex", "Home");
        }
    }
}