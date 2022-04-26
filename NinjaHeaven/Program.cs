using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
