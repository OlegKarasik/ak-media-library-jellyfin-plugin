using System.Text.Json.Serialization;

public class SeasonMetadataJson
{
  [JsonPropertyName("summary")]
  public string[]? Summary { get; init; }
}
