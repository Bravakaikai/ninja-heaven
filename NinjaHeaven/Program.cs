using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NinjaHeaven.Services;

namespace NinjaHeaven
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var folderPath = "Server/Keys/Encryption";
            if (!File.Exists(@$"{folderPath}/public.key") || !File.Exists(@$"{folderPath}/private.key"))
            {
                if (!Directory.Exists(@$"{folderPath}"))
                {
                    Directory.CreateDirectory(@$"{folderPath}");
                }
                // RSA Encryption
                // Generate a key pair
                ExpressEncription.RSAEncription.MakeKey(@$"{folderPath}/public.key", @$"{folderPath}/private.key");
            }

            var host = CreateHostBuilder(args).Build();

            // DB Migration            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    Console.WriteLine("Sleep for 10 seconds.");
                    Thread.Sleep(10000);

                    services.GetService<NinjaHeavenDbContext>().Database.Migrate();
                    Console.WriteLine("Finish to migrate database.");

                    SeedDataService.Initialize(services);
                    Console.WriteLine("Finish to seed data.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred migrating the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
