using System;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WebApi
{
    public static class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

                if (env.IsDevelopment())
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly != null)
                        config.AddUserSecrets(appAssembly, optional: true);
                }

                config.AddEnvironmentVariables();

                if (args != null)
                    config.AddCommandLine(args);
            })
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>()
                    .UseKestrel((_, options) =>
                    {
                        var port = Environment.GetEnvironmentVariable("PORT");
                        if (!string.IsNullOrEmpty(port))
                            options.ListenAnyIP(int.Parse(port));

                        options.Limits.MaxRequestBodySize = 10 /* Megabytes */ * 1000 /* Kilobytes */ * 1000 /* Bytes */;
                    })
            );
    }
}
