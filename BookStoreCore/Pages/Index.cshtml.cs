﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Exporter;



namespace BookStoreCore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
            MyActivitySource = new ActivitySource(_config.GetSection("Observability")["ServiceName"], _config.GetSection("Observability")["Version"]);
        }

        public static ActivitySource MyActivitySource { get; set; }

        public void OnGet()
        {

            using var activity = MyActivitySource.StartActivity("VisitHome", ActivityKind.Server); // this will be translated to a X-Ray Segment
            activity?.SetTag("http.method", "GET");
            activity?.SetTag("http.url", "http://www.bookstorecore.com");            
            activity?.SetTag("http.page", "Home");
            activity?.SetTag("CustomTrace", "true");
            activity?.AddEvent(new("A user visited the site!")); //_Logger is recommened for high volumes of events. 
            
        }

        public void StartTrace()
        {

        }


    }

    public class Telmetry
    {

        public Telmetry()
        {

        }
    }

}