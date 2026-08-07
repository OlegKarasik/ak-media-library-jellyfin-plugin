using System.Globalization;
using System.Text.Json;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;

public class SeriesMetadataProvider : IRemoteMetadataProvider<Series, SeriesInfo> 
{
  public string Name => "AK-Media-Library";

  public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
    SeriesInfo searchInfo, 
    CancellationToken cancellationToken)
  {
    return Task.FromResult(Enumerable.Empty<RemoteSearchResult>());
  }

  public async Task<MetadataResult<Series>> GetMetadata(
    SeriesInfo info, 
    CancellationToken cancellationToken)
  {
    var result = new MetadataResult<Series>();
    if (string.IsNullOrEmpty(info.Name) || string.IsNullOrEmpty(info.Path))
    {
      // Without name of path it is impossible for us to get the metadata
      //
      return result;
    }

    result.Item        = new Series();
    result.HasMetadata = true;

    // Here we fill the default set of metadata properties
    //
    this.SetDefaultProperties(result, info);

    var metadataJson = await this.GetSeriesMetadataJsonAsync(info.Path);
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
    MetadataResult<Series> result,
    SeriesInfo info)
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(info);

    result.Item.Name     = info.Name;
    result.Item.SortName = info.Name;
  }

  private void SetJsonProperties(
    MetadataResult<Series> result,
    SeriesMetadataJson metadataJson)
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(metadataJson);

    if (metadataJson.Title is not null)
    {
      result.Item.Name = metadataJson.Title;
      result.Item.SortName = metadataJson.Title;
    }
    if (metadataJson.Summary is not null)
    {
      var s = $"{Environment.NewLine}-{Environment.NewLine}";
      result.Item.Overview = string.Join(s, metadataJson.Summary.Select(i => i.Trim()));
    }
  }

  private async Task<SeriesMetadataJson?> GetSeriesMetadataJsonAsync(
      string path)
  {
    ArgumentException.ThrowIfNullOrEmpty(path);

    var jsonPath = $"this.props.json";
    if (!File.Exists(jsonPath))
    {
      return null;
    }

    await using var fs = new FileStream(jsonPath, FileMode.Open, FileAccess.Read);
    return await JsonSerializer.DeserializeAsync<SeriesMetadataJson>(fs);
  }

}
