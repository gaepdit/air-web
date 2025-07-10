using AirWeb.AppServices.AutoMapper;
using AirWeb.AppServices.ServiceRegistration;
using AirWeb.WebApp.Platform.AppConfiguration;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.EmailService.Utilities;
using GaEpd.FileService;
using IaipDataService;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Configure basic settings.
builder.BindAppSettings().AddSecurityHeaders().AddErrorLogging();
builder.Services.AddDataProtection();

// Configure Identity stores.
builder.Services.AddIdentityStores();

// Configure authentication and authorization.
builder.ConfigureAuthentication();

// Add app services.
builder.Services.AddAppServices().AddAutoMapperProfiles().AddEmailService();

// Configure UI services.
builder.Services.AddRazorPages();

// Add data stores and initialize the database.
await builder.ConfigureDataPersistence();

// Add IAIP data service.
builder.Services.AddIaipDataServices(AppSettings.DevSettings.ConnectToIaipDatabase,
    builder.Configuration.GetConnectionString("IaipConnection"));

// Add file storage.
builder.Services.AddFileServices(builder.Configuration);

// Add API documentation.
builder.Services.AddApiDocumentation();

// Configure bundling and minification.
builder.AddWebOptimizer();

//Add simple cache.
builder.Services.AddMemoryCache();

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
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
