using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi
{
    /// <summary>
    /// Main entry point for the web API
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point of the program
        /// </summary>
        /// <param name="args">Any arguments to pass to the program</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Sets up the web host
        /// </summary>
        /// <param name="args">Any arguments to pass related to hosting</param>
        /// <returns>A host builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
