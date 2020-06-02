using Liquid.Feature.MapPlugin.Controllers;
using Liquid.Feature.MapPlugin.Repositories;
using Liquid.Feature.MapPlugin.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Liquid.Feature.MapPlugin.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMapPluginRepository, MapPluginRepository>();
            serviceCollection.AddTransient<MapPluginController>();
        }
    }
}