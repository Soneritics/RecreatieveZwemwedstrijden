using Newtonsoft.Json;

namespace Models;

public class CosmosDocument<T>
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    public string Type { get; set; } = typeof(T).Name;

    public T Data { get; set; }
}