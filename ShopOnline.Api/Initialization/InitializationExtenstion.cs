using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ShopOnline.Api.Initialization
{
    public static class InitializationExtenstion
    {
        internal static void AddInitializationStages(this IServiceCollection services)
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly =>
                    assembly.GetTypes()
                        .Where(t => typeof(IStage).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract))
                .ToList()
                .ForEach(stage => { services.AddTransient(typeof(IStage), stage); });
        }
    }
}
