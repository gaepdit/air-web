using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.AutoMapper;
using AirWeb.AppServices.IdentityServices;
using AirWeb.AppServices.ServiceRegistration;
using AirWeb.WebApp.Platform.AppConfiguration;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.EmailService.Utilities;
using GaEpd.FileService;
using IaipDataService;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

builder.BindAppSettings();
builder.AddErrorLogging();

// Persist data protection keys.
builder.Services.AddDataProtection();

// Configure Identity.
builder.Services.AddIdentityStores();

// Configure Authentication.
builder.AddAuthenticationServices();

// Configure authorization and identity services.
builder.Services.AddAuthorizationPolicies().AddIdentityServices();

// Configure UI services.
builder.Services.AddRazorPages();

// Starting value for HSTS max age is five minutes to allow for debugging.
// For more info on updating HSTS max age value for production, see:
// https://gaepdit.github.io/web-apps/use-https.html#how-to-enable-hsts
if (!builder.Environment.IsDevelopment())
{
    builder.Services
        .AddHsts(options => options.MaxAge = TimeSpan.FromMinutes(300))
        .AddHttpsRedirection(options =>
        {
            options.HttpsPort = 443;
            options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
        });
}

// Add app services.
builder.Services.AddAppServices().AddAutoMapperProfiles().AddEmailService();

// Add data stores.
builder.Services
    .AddIaipDataServices(AppSettings.DevSettings.UseInMemoryIaipData,
        builder.Configuration.GetConnectionString("IaipConnection"))
    .AddDataPersistence(builder.Configuration, builder.Environment)
    .AddFileServices(builder.Configuration);

// Initialize database.
builder.Services.AddHostedService<MigratorHostedService>();

// Add API documentation.
builder.Services.AddMvcCore().AddApiExplorer();

const string apiVersion = "v1";
const string apiTitle = "Air Protection Branch Web App API";
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(apiVersion, new OpenApiInfo
    {
        Version = apiVersion,
        Title = apiTitle,
        Contact = new OpenApiContact
        {
            Name = $"{apiTitle} Support",
            Email = builder.Configuration["SupportEmail"],
        },
    });
});

// Configure bundling and minification.
builder.Services.AddWebOptimizer(
    minifyJavaScript: AppSettings.DevSettings.EnableWebOptimizer,
    minifyCss: AppSettings.DevSettings.EnableWebOptimizer);

//Add simple cache.
builder.Services.AddMemoryCache();

// Build the application.
var app = builder.Build();

// Configure security HTTP headers
if (!builder.Environment.IsDevelopment() || AppSettings.DevSettings.UseSecurityHeadersInDev)
{
    app.UseHsts().UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());
}

// Configure the application pipeline.
app
    .UseErrorHandling()
    .UseStatusCodePagesWithReExecute("/Error/{0}")
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseStaticFiles()
    .UseAppUrlRedirects()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

// Configure API documentation.
app
    .UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
    .UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"{apiVersion}/openapi.json", $"{apiTitle} {apiVersion}");
        options.RoutePrefix = "api-docs";
        options.DocumentTitle = apiTitle;
    });

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
