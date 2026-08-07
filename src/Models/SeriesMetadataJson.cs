using System.Text.Json.Serialization;

public class SeriesMetadataJson
{
  [JsonPropertyName("title")]
  public string? Title { get; init; }

  [JsonPropertyName("summary")]
  public string[]? Summary { get; init; }
}
