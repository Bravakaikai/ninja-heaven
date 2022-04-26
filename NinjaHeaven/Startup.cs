using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using NinjaHeaven.Services;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

namespace NinjaHeaven
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Environment = env;
            Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // To store keys on Volume instead of at the %LOCALAPPDATA% default location
            var dbPath = @"Server/Keys/DataProtection";
            var dbDirectory = new DirectoryInfo(dbPath);
            // 判斷資料夾是否為空
            if (!Directory.Exists(dbPath) || dbDirectory.GetFiles().Length == 0) {
                services.AddDataProtection().PersistKeysToFileSystem(dbDirectory);
            }

            services.AddDbContext<NinjaHeavenDbContext>(options =>
            {
                if (Environment.IsDevelopment())
                {
                    options.UseSqlite(Configuration.GetConnectionString("NinjaHeavenDb"));
                }
                else
                {
                    var database = Configuration["DbName"] ?? "NinjaHeavenDb";
                    var server = Configuration["DbServer"] ?? "localhost";
                    var port = Configuration["DbPort"] ?? "1433";
                    var user = Configuration["DbUser"] ?? "SA";
                    var password = Configuration["DbPassword"] ?? "";

                    options.UseSqlServer($"Server={server}, {port}; Initial Catalog={database};User ID={user}; Password={password};");
                }
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                DbManagementService.SeedData(app);
                app.UseDeveloperExceptionPage();
            }
            else
            {
                DbManagementService.MigrationInitialize(app);
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
