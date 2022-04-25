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
            if (!File.Exists(@"public.key") || !File.Exists(@"private.key"))
            {
                // RSA Encryption
                // Generate a key pair
                ExpressEncription.RSAEncription.MakeKey(@"public.key", @"private.key");
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
