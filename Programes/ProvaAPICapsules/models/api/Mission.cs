using Newtonsoft.Json;

namespace dam.models.api;

class Mission
{
    [JsonProperty("name")]
    public string? MissionName { get; set; }

    [JsonProperty("flight")]
    public string? Flight { get; set; }
}