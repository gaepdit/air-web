using AirWeb.AppServices;
using AirWeb.AppServices.Compliance;
using AirWeb.AppServices.Compliance.AutoMapper;
using AirWeb.AppServices.Core;
using AirWeb.AppServices.Core.AutoMapper;
using AirWeb.WebApp.Platform.AppConfiguration;
using AirWeb.WebApp.Platform.OrgNotifications;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.EmailService.Utilities;
using IaipDataService;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders().AddZLoggerConsole(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UsePlainTextFormatter();
    else
        options.UseJsonFormatter();
});

// Configure basic settings.
builder.BindAppSettings().AddSecurityHeaders().AddErrorLogging();
builder.Services.AddDataProtection();

// Configure authentication and authorization.
builder.ConfigureAuthentication().ConfigureAuthorization();

// Add UI services.
builder.Services.AddRazorPages();

// Add common services.
builder.Services
    .AddAppServices()
    .AddIdentityStores()
    .AddAutoMapperProfiles()
    .AddEmailService().AddApiDocumentation()
    .AddWebOptimizer().AddMemoryCache().AddOrgNotifications();

// Add compliance/enforcement services.
builder.Services
    .AddComplianceAppServices()
    .AddComplianceAutoMapperProfiles();

// Add data stores and initialize the database.
await builder.ConfigureDataPersistenceAsync();
builder.AddIaipDataServices(AppSettings.ConnectToIaip);

// Build the application.
var app = builder.Build();

// Configure the application pipeline.
app.UseSecurityHeaders().UseErrorHandling().UseStatusCodePagesWithReExecute("/Error/{0}").UseHttpsRedirection()
    .UseWebOptimizer().UseStaticFiles().UseAppUrlRedirects().UseRouting().UseAuthentication().UseAuthorization()
    .UseApiDocumentation();

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
