using Newtonsoft.Json;

namespace dam.models.api;

class Capsule
{
    [JsonProperty("capsule_serial")]
    public string? CapsuleSerial { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("original_launch")]
    public string? OriginalLaunch { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("missions")]
    public Mission[]? CapsuleMissions { get; set; }
}