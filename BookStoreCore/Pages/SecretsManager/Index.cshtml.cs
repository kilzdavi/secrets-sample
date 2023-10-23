using Amazon.SecretsManager.Extensions.Caching;
using BookStoreCore.Classes;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreCore.Pages.SecretsManager;

public class IndexModel : PageModel
{

    private readonly ILogger<Index> _logger;
    public DemoConfiguration Configuration { get; set; }
    public string LaunchCode { get; set; }
    public string SecretPlanLocation { get; set; }
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
        LaunchCode = await _cache.GetSecretString("launchcode-secret-stg");
        SecretPlanLocation = await _cache.GetSecretString("secret-plan-location-secret-stg");
        
    }
    
}