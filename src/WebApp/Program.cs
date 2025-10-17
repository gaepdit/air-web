using AirWeb.AppServices.AutoMapper;
using AirWeb.AppServices.ServiceRegistration;
using AirWeb.WebApp.Platform.AppConfiguration;
using AirWeb.WebApp.Platform.OrgNotifications;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.EmailService.Utilities;
using GaEpd.FileService;
using IaipDataService;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Configure basic settings.
builder.BindAppSettings().AddSecurityHeaders().AddErrorLogging();
builder.Services.AddDataProtection();

// Configure authentication and authorization.
builder.ConfigureAuthentication();

// Add UI services.
builder.Services.AddRazorPages();

// Add data stores and initialize the database.
builder.Services.AddIdentityStores();
await builder.ConfigureDataPersistence();

// Add various services.
builder.Services
    .AddAppServices()
    .AddAutoMapperProfiles()
    .AddEmailService()
    .AddApiDocumentation()
    .AddWebOptimizer()
    .AddMemoryCache()
    .AddIaipDataServices(AppSettings.ConnectToIaip, builder.Configuration.GetConnectionString("IaipConnection"))
    .AddFileServices(builder.Configuration)
    .AddOrgNotifications();

// Configure Aspire.
builder.AddServiceDefaults();

// Build the application.
var app = builder.Build();

// Configure the application pipeline.
app
    .UseSecurityHeaders()
    .UseErrorHandling()
    .UseStatusCodePagesWithReExecute("/Error/{0}")
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseStaticFiles()
    .UseAppUrlRedirects()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseApiDocumentation();

// Map endpoints.
app.MapDefaultEndpoints();
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
