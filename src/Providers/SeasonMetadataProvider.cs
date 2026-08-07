using System.Globalization;
using System.Text.Json;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;

public class SeasonMetadataProvider : IRemoteMetadataProvider<Season, SeasonInfo> 
{
  public string Name => "AK-Media-Library";

  public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
    SeasonInfo searchInfo, 
    CancellationToken cancellationToken)
  {
    return Task.FromResult(Enumerable.Empty<RemoteSearchResult>());
  }

  public async Task<MetadataResult<Season>> GetMetadata(
    SeasonInfo info, 
    CancellationToken cancellationToken)
  {
    var result = new MetadataResult<Season>();
    if (string.IsNullOrEmpty(info.Name) || string.IsNullOrEmpty(info.Path))
    {
      // Without name of path it is impossible for us to get the metadata
      //
      return result;
    }

    result.Item        = new Season();
    result.HasMetadata = true;

    // Here we fill the default set of metadata properties
    //
    this.SetDefaultProperties(result, info);

    var metadataJson = await this.GetSeasonMetadataJsonAsync(info.Path);
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
    MetadataResult<Season> result,
    SeasonInfo info)
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(info);

    result.Item.Name     = info.Name;
    result.Item.SortName = info.Name;
  }

  private void SetJsonProperties(
    MetadataResult<Season> result,
    SeasonMetadataJson metadataJson)
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(metadataJson);

    if (metadataJson.Summary is not null)
    {
      var s = $"{Environment.NewLine}-{Environment.NewLine}";
      result.Item.Overview = string.Join(s, metadataJson.Summary.Select(i => i.Trim()));
    }
  }

  private async Task<SeasonMetadataJson?> GetSeasonMetadataJsonAsync(
      string path)
  {
    ArgumentException.ThrowIfNullOrEmpty(path);

    var jsonPath = $"this.props.json";
    if (!File.Exists(jsonPath))
    {
      return null;
    }

    await using var fs = new FileStream(jsonPath, FileMode.Open, FileAccess.Read);
    return await JsonSerializer.DeserializeAsync<SeasonMetadataJson>(fs);
  }

}
