using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace CZE.Web.ViewComponents
{
    public class LoginViewComponent:ViewComponent
    {
        private readonly CZEContext _db;

        public LoginViewComponent(CZEContext db)
        {
            _db = db;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            if (TempData["error_poruka"] != null)
            {
                ViewData["error_poruka"] = TempData["error_poruka"];
            }
            return View(new LoginVM() { ZapamtiMe = true });
        }
    }
}
