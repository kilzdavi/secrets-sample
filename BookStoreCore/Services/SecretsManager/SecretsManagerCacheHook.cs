using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;

namespace BookStoreCore.Classes;

public class SecretsManagerCacheHook : ISecretCacheHook
{
    private ILogger<SecretsManagerCacheHook> _logger;

    public SecretsManagerCacheHook(ILogger<SecretsManagerCacheHook> logger)
    {
        _logger = logger;
    }
    
    public object Put(object o)
    {
        _logger.LogInformation($"Put - {GetSecretMessage(o)}");
        return o;
    }

    public object Get(object cachedObject)
    {
        
        _logger.LogInformation($"Get - {GetSecretMessage(cachedObject)}");
        return cachedObject;
        
    }

    public string GetSecretMessage(object o)
    {
        switch (o)
        {
            case DescribeSecretResponse dResponse:
                return $"Secret Name: {dResponse.Name}";
            
            case GetSecretValueResponse svResponse:
                return $"Secret Name: {svResponse.Name}";
            default:
                return $"Unknown Type Operation - {o.GetType().FullName}";
        }
        
    }
}