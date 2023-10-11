using Newtonsoft.Json;

namespace BookStoreCore.Classes;

public class DemoConfiguration
{
    [JsonProperty("environment")]
    public string Environment { get; set; }

    [JsonProperty("ssmTimeToLive")]
    public uint SsmTimeToLive { get; set; }

    [JsonProperty("ssmPath")]
    public string SsmPath { get; set; }

    [JsonProperty("secretsCacheExpiry")]
    public uint SecretsCacheExpiry { get; set; }
}
