using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace BookStoreCore.Classes;

public class  SecretsManagerService
{
    private readonly AmazonSecretsManagerClient _client = new AmazonSecretsManagerClient();
    
    public async Task<string> GetSecretAsync(string secretId)
    {
        var request = new GetSecretValueRequest { SecretId = secretId };
        var response = await _client.GetSecretValueAsync(request);
        
        return response.SecretString;
    }
    
}