using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.DependencyInjection;

public class PluginServiceRegistrator : IPluginServiceRegistrator
{
  public void RegisterServices(
    IServiceCollection serviceCollection,
    IServerApplicationHost applicationHost)
  {
    serviceCollection.AddTransient<SeriesMetadataProvider>();
    serviceCollection.AddTransient<SeasonMetadataProvider>();
    serviceCollection.AddTransient<EpisodeMetadataProvider>();
  }
}
