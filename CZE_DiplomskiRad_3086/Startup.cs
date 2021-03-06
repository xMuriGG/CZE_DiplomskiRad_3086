﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CZE.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddDbContext<CZEContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CZEContext")));
            //services.AddDbContext<CZEContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CZEGearHost")));
            services.AddDbContext<CZEContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CZEFitHost")));
            //services.AddDbContext<CZEContext>(options => options.UseInMemoryDatabase("ImeBaze"));

            // Adds a default in-memory implementation of IDistributedCache
            services.AddDistributedMemoryCache(); 
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=KlijentskiPortalIndex}/{id?}");
            });
        }
    }
}
