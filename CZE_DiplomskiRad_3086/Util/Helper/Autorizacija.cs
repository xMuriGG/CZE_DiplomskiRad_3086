using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;


namespace CZE.Web.Util.Helper
{

    public class AutorizacijaAttribute : TypeFilterAttribute
    {
        public AutorizacijaAttribute(params UlogeKorisnika[] ulogeKorisnika)
            : base(typeof(MyAuthorizeImpl))
        {
            Arguments = new object[] { ulogeKorisnika };
        }
    }


    public class MyAuthorizeImpl : IAsyncActionFilter
    {
        private readonly UlogeKorisnika[] _ulogeKorisnika;

        public MyAuthorizeImpl(params UlogeKorisnika[] ulogeKorisnika)
        {
            _ulogeKorisnika = ulogeKorisnika.Length > 0 ? ulogeKorisnika :
                (UlogeKorisnika[])Enum.GetValues(typeof(UlogeKorisnika));
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            var requestUrl = filterContext.HttpContext.Request.GetEncodedUrl();
            KorisnickiNalog k = await filterContext.HttpContext.GetLogiraniKorisnik();

            if (k == null)
            {
                if (filterContext.Controller is Controller controller)
                {
                    controller.TempData["error_poruka"] = "Niste logirani";
                    controller.TempData["requestUrl"] = requestUrl;
                }

                filterContext.Result = new RedirectToActionResult("Index", "Autentifikacija", new { @area = "" });
                return;
            }

            //Preuzimamo DbContext preko app services
            CZEContext db = filterContext.HttpContext.RequestServices.GetService<CZEContext>();
            if (_ulogeKorisnika.Any(a => a == k.UlogaKorisnika))
            {
                await next();
                return;
            }


            if (filterContext.Controller is Controller c1)
            {
                c1.TempData["error_poruka"] = "Nemate pravo pristupa";
                c1.TempData["requestUrl"] = requestUrl;

            }
            filterContext.Result = new RedirectToActionResult("Index", "Autentifikacija", new { @area = "" });
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // throw new NotImplementedException();
        }
    }
}

