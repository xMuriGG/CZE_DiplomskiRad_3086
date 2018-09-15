using System;
using System.Linq;
using CZE.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace CZE.Data
{
    public static class SeedData
    {
        public static void Initialize(CZEContext context)
        {
            if (context.Osobe.Any())
            {
                return;   // DB has been seeded
            }
            var osobe = new Osoba []
            {
                new Osoba(){}
            };
        }
    }
}
