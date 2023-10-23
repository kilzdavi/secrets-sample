using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Options;

namespace BookStoreCore.Classes;

public class  SecretsManagerService
{
    private AmazonSecretsManagerClient _client { get; set; }
    private  DemoConfiguration _config { get; set; }
    private SecretCacheConfiguration _secretCacheConfiguration { get; set; }
    public SecretsManagerCache _cache { get; set; }

    public SecretsManagerService(DemoConfiguration config)
    {
        _client = new AmazonSecretsManagerClient();
        _config = config;
        _secretCacheConfiguration = new SecretCacheConfiguration()
        {
            CacheItemTTL = _config.SecretsCacheExpiry
            // CacheHook = new SecretManagerCacheHook()
            //3600000 1 hour
        };
        _cache = new SecretsManagerCache(_client, _secretCacheConfiguration);
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        
        // var request = new GetSecretValueRequest { SecretId = secretName };
        // var response = await _client.GetSecretValueAsync(request);

        //reads the secret response into the cache. 
        var mySecret = await _cache.GetSecretString(secretName);
        return mySecret;
    }

    public string GetRemainingCacheTTL()
    {
        return "";
    }
    
}