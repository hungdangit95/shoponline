using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ShopOnlineApp.Initialization;

namespace ShopOnlineApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
            .AutoInit()
            .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(Configuration)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

        static void Configuration(WebHostBuilderContext context, IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.jon", optional: true,
                    reloadOnChange: true);
        }

    }
}
