using System.Text.Json.Serialization;

public class EpisodeMetadataJson
{
  [JsonPropertyName("title")]
  public string? Title { get; init; }

  [JsonPropertyName("summary")]
  public string[]? Summary { get; init; }

  [JsonPropertyName("date")]
  public string? Date { get; init; }

  [JsonPropertyName("directors")]
  public string[]? Directors { get; init; }

  [JsonPropertyName("writers")]
  public string[]? Writers { get; init; }

  [JsonPropertyName("startPosition")]
  public int? StartPosition { get; init; }

  [JsonPropertyName("endPosition")]
  public int? EndPosition { get; init; }
}
