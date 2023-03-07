using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry;

using BookStoreCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OpenTelemetry.Exporter;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;

// Define some important constants to initialize tracing with

var builder = WebApplication.CreateBuilder(args);

#region OpenTelemetry
var serviceName = "AWS.SampleApp.BookStoreCore";
var serviceVersion = "1.0.0";
var MyActivitySource = new ActivitySource(serviceName);

var appResourceBuilder = ResourceBuilder.CreateDefault()
        .AddService(serviceName: serviceName, serviceVersion: serviceVersion);

//Configure important OpenTelemetry settings, the console exporter, and instrumentation library


builder.Services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .AddConsoleExporter()
        .AddOtlpExporter(options =>
        {
            options.Protocol = OtlpExportProtocol.Grpc;
            options.Endpoint = new Uri("http://adot:4317");

        })
        .AddSource(serviceName)
        .SetResourceBuilder(appResourceBuilder.AddTelemetrySdk())
        .AddXRayTraceId()
        .AddAWSInstrumentation()
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation();

});

Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());

var meter = new Meter(serviceName);
var counter = meter.CreateCounter<long>("app.request-counter");

builder.Services.AddOpenTelemetry().WithMetrics(metricProviderBuilder =>
{
    metricProviderBuilder
        .AddOtlpExporter(options =>
        {
            options.Protocol = OtlpExportProtocol.Grpc;
            options.Endpoint = new Uri("http://adot:4317");

        })
        // *** Using Otel Collector now *** //
        //.AddPrometheusExporter(options =>
        //{
        //    options.ScrapeResponseCacheDurationMilliseconds = 0;
        //})
        .AddMeter(meter.Name)
        .SetResourceBuilder(appResourceBuilder)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation();

});

#endregion

// Add Additional services to the container.
builder.Services.AddRazorPages();

//Dependency Injection for DB Context
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        )); 

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#region Identity
    builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false;
    });
    builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

//Ensures the DB is Created with the latest schema.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
     DbInitializer.Initialize(context);
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

//Config Otel Endpoint for Prometheus *** Using Otel Collector now ***
//app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();
