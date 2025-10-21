using AirWeb.AppServices.AutoMapper;
using AirWeb.AppServices.ServiceRegistration;
using AirWeb.WebApp.Platform.AppConfiguration;
using AirWeb.WebApp.Platform.OrgNotifications;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.EmailService.Utilities;
using IaipDataService;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Configure basic settings.
builder.BindAppSettings().AddSecurityHeaders().AddErrorLogging();
builder.Services.AddDataProtection();

// Configure authentication and authorization.
builder.ConfigureAuthentication();

// Add UI services.
builder.Services.AddRazorPages();

// Add various services.
builder.Services.AddAppServices().AddIdentityStores().AddAutoMapperProfiles().AddEmailService().AddApiDocumentation()
    .AddWebOptimizer().AddMemoryCache().AddOrgNotifications();

// Add data stores and initialize the database.
await builder.ConfigureDataPersistenceAsync();
builder.AddIaipDataServices(AppSettings.ConnectToIaip);

// Configure Aspire.
builder.AddServiceDefaults();

// Build the application.
var app = builder.Build();

// Configure the application pipeline.
app.UseSecurityHeaders().UseErrorHandling().UseStatusCodePagesWithReExecute("/Error/{0}").UseHttpsRedirection()
    .UseWebOptimizer().UseStaticFiles().UseAppUrlRedirects().UseRouting().UseAuthentication().UseAuthorization()
    .UseApiDocumentation();

// Map endpoints.
app.MapDefaultEndpoints();
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
