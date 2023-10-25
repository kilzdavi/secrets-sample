using Amazon.SecretsManager.Extensions.Caching;
using BookStoreCore.Classes;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BookStoreCore.Pages.SecretsManager;

public class IndexModel : PageModel
{

    private readonly ILogger<Index> _logger;
    public DemoConfiguration Configuration { get; set; }
    public string SystemUserName { get; set; }
    public string SystemPassword { get; set; }
    public string SystemApiKey { get; set; }

    private SecretsManagerCache _cache { get; set; }


    public IndexModel(ILogger<Index> logger, DemoConfiguration config, SecretsManagerCache cache)
    {
        _cache = cache;
        _logger = logger;
        Configuration = config;
        
    }
    
    public async Task OnGet()
    {
        _logger.LogInformation("Accessing Secrets Manager");

        var systemCredentials = JsonSerializer.Deserialize<JsonObject>(await _cache.GetSecretString("staging/bookstore/system-credentials"));
        
        SystemApiKey = systemCredentials["apiKey"].ToString();
        SystemUserName = systemCredentials["system-super-user"].ToString();
        SystemPassword = systemCredentials["system-password"].ToString();
        
        
    }
    
}