using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;

namespace BookStoreCore.Classes;

public class  SecretsManagerService
{
    private AmazonSecretsManagerClient _client { get; set; }
    private static SecretCacheConfiguration cacheConfiguration = new SecretCacheConfiguration
    {
        CacheItemTTL = 30000 
        //3600000 1 hour
    };
    public SecretsManagerCache _cache { get; set; }

    public SecretsManagerService()
    {
        _client = new AmazonSecretsManagerClient();
        _cache = new SecretsManagerCache(_client, cacheConfiguration);
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        
        // var request = new GetSecretValueRequest { SecretId = secretName };
        // var response = await _client.GetSecretValueAsync(request);

        var mySecret = await _cache.GetSecretString(secretName);
        Console.WriteLine(_cache);
        return mySecret;
    }
    
}