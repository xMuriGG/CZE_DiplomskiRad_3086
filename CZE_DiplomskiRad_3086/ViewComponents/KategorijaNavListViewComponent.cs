using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Web.Areas.AdministrativniRadnik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CZE.Web.ViewComponents
{
    public class KategorijaNavListViewComponent:ViewComponent
    {
        private readonly CZEContext _db;

        public KategorijaNavListViewComponent(CZEContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _db.KursKategorije.Select(s=>new KursVMs.KursKategorijaVM()
            {
                KursKategorijaId = s.KursKategorijaId,
                Naziv = s.Naziv
            }).ToListAsync();
            
            return View(model);
        }
    }
}
