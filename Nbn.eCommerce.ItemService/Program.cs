using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nbn.eCommerce.ItemService.Utility;

namespace Nbn.eCommerce.ItemService
{
    public class Program
    {
        /// <summary>
        /// Log handler.
        /// </summary>
        private ILogger<Program> logger;

        public static void Main(string[] args)
        {
            DapperExtension.Init(retryCount: 5, 1000, (exception, timespan, attempt) => Debug.WriteLine($"Exception: '{exception}',Database Retry Attempt: {attempt} Delay: {timespan}')"));

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
