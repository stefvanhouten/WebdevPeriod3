using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Linq;

namespace WebdevPeriod3
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
