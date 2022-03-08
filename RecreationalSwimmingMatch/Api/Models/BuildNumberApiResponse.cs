using System.Text.Json.Serialization;

namespace Api.Models;

internal class BuildNumberApiResponse
{
    [JsonPropertyName("tag_name")]
    public string? TagName { get; set; }
}