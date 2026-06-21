using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;

public class Plugin : BasePlugin<PluginConfiguration>, IRemoteMetadataProvider<Episode, EpisodeInfo> 
{
    public override string Name => "AK Media Library Plugin";
    public override Guid Id     => Guid.Parse("22A701F7-F48D-47D9-9897-90347F8A6B6D");

    public Plugin(
      IApplicationPaths applicationPaths, 
      IXmlSerializer xmlSerializer) 

      : base(applicationPaths, xmlSerializer)
    {
    }

    public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
      EpisodeInfo searchInfo, 
      CancellationToken cancellationToken)
    {
      return Task.FromResult(Enumerable.Empty<RemoteSearchResult>());
    }

    public Task<MetadataResult<Episode>> GetMetadata(
      EpisodeInfo info, 
      CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> GetImageResponse(
      string url, 
      CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
}
