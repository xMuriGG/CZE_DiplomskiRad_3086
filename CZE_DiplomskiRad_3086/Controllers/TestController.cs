using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using CZE.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CZE.Web.Controllers
{
    public class TestController : Controller
    {
        private CZEContext _db;

        public TestController(CZEContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        //[Route("AdministrativniRadnik/Osoba/GetOsobe/{id}/{draw}")]
        public IActionResult GetOsobe(int draw, int start, int length,[FromForm(Name = "search[value]")] string search)
        {
            
            var query = _db.Gradovi.AsQueryable();
            int total = query.Count();

            var data = JsonConvert.SerializeObject(query.ToList());

            //string jsonString = "{\"draw\": 1,\"recordsTotal\": 2,\"recordsFiltered\": 2,\"data\":[[1,\"Sarajevo\"],[2,\"Gorazde\"]]}";
            //string jsonString = "{\"draw\": 1,\"recordsTotal\": 2,\"recordsFiltered\": 2,\"data\": [{\"gradId\": \"1\",\"naziv\": \"Sarajevo\"},{\"gradId\": \"2\",\"naziv\": \"Gorazde\"}]}";
            //string jsonString = "{\"draw\": 1,\"recordsTotal\": 2,\"recordsFiltered\": 2,\"data\":";
            //string j=JsonConvert.SerializeObject(query.ToList());

            //ovo radi ako stavimo velika slova u DataTable data:"GradId" i data:"Naziv"
            //string jsonString = JsonConvert.SerializeObject(new
            //{
            //    draw = draw,
            //    recordsTotal = total,
            //    recordsFiltered = total,
            //    data = query.ToList()
            //});
            if (!String.IsNullOrEmpty(search))
            {
                query= query.Where(w => w.Naziv.StartsWith(search) || w.GradId.ToString().StartsWith(search));
               
            }
            int recordsFiltered = query.Count();
            query = query.Skip(start).Take(length);


            return Json(new
            {
                draw = draw,
                recordsTotal = total,
                recordsFiltered = recordsFiltered,
                data = query.ToList()
            });
            //return Ok(jsonString);
        }
    }
}