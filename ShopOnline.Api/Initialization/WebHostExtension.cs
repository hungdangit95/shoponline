using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ShopOnline.Api.Initialization
{
    public static class WebHostExtension
    {
        public static IWebHost AutoInit(this IWebHost webHost, bool always = false)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                foreach (var stage in scope.ServiceProvider.GetServices<IStage>().OrderBy(t => t.Order))
                {
                    stage.ExecuteAsync().Wait();
                }
            }

            return webHost;
        }
    }
}
