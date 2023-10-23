using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BookStoreCore.Classes;

public class DemoConfiguration
{
    [JsonProperty("ssmTimeToLive")]
    public uint SsmTimeToLive { get; set; }

    [JsonProperty("ssmPath")]
    public string SsmPath { get; set; }

    [JsonProperty("secretsCacheExpiry")]
    public uint SecretsCacheExpiry { get; set; }
}
