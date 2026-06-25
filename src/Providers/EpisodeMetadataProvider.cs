using System.Globalization;
using System.Text.Json;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;

public class EpisodeMetadataProvider : IRemoteMetadataProvider<Episode, EpisodeInfo> 
{
  public string Name => "AK-Media-Library";

  public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
    EpisodeInfo searchInfo, 
    CancellationToken cancellationToken)
  {
    return Task.FromResult(Enumerable.Empty<RemoteSearchResult>());
  }

  public async Task<MetadataResult<Episode>> GetMetadata(
    EpisodeInfo info, 
    CancellationToken cancellationToken)
  {
    var result = new MetadataResult<Episode>();
    if (string.IsNullOrEmpty(info.Name) || string.IsNullOrEmpty(info.Path))
    {
      // Without name of path it is impossible for us to get the metadata
      //
      return result;
    }

    result.Item        = new Episode();
    result.HasMetadata = true;

    // Here we fill the default set of metadata properties
    //
    this.SetDefaultProperties(result, info);

    var metadataJson = await this.GetEpisodeMetadataJsonAsync(info.Path);
    if (metadataJson is null)
    {
      // If there are no metadata file, then we just return the default metadata
      //
      return result;
    }

    // Here we fill the properties from metadata json
    //
    this.SetJsonProperties(result, metadataJson);

    return result;
  }

  public Task<HttpResponseMessage> GetImageResponse(
    string url, 
    CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  private void SetDefaultProperties(
    MetadataResult<Episode> result,
    EpisodeInfo info)
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(info);

    result.Item.Name     = info.Name;
    result.Item.SortName = info.Name;
  }

  private void SetJsonProperties(
    MetadataResult<Episode> result,
    EpisodeMetadataJson metadataJson)
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(metadataJson);

    if (metadataJson.Summary is not null)
    {
      var s = $"{Environment.NewLine}-{Environment.NewLine}";
      result.Item.Overview = string.Join(s, metadataJson.Summary.Select(i => i.Trim()));
    }
    if (metadataJson.Date is not null)
    {
      var dt = DateTime.ParseExact(metadataJson.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

      result.Item.ProductionYear = dt.Year;
      result.Item.PremiereDate   = dt;
    }
  }

  private async Task<EpisodeMetadataJson?> GetEpisodeMetadataJsonAsync(
      string path)
  {
    ArgumentException.ThrowIfNullOrEmpty(path);

    var jsonPath = $"{path}.props.json";
    if (!File.Exists(jsonPath))
    {
      return null;
    }

    await using var fs = new FileStream(jsonPath, FileMode.Open, FileAccess.Read);
    return await JsonSerializer.DeserializeAsync<EpisodeMetadataJson>(fs);
  }

}
