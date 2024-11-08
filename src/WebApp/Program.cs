using AirWeb.AppServices.ErrorLogging;
using AirWeb.AppServices.RegisterServices;
using AirWeb.WebApp.Platform.AppConfiguration;
using AirWeb.WebApp.Platform.Logging;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.FileService;
using IaipDataService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
// ReSharper disable once HeapView.BoxingAllocation
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Bind application settings.
BindingsConfiguration.BindSettings(builder);

// Configure Identity.
builder.Services.AddIdentityStores();

// Configure Authentication.
builder.Services.AddAuthenticationServices(builder.Configuration);

// Persist data protection keys.
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"] ?? "", "DataProtectionKeys");
builder.Services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));

// Configure authorization policies and handlers.
builder.Services.AddAuthorizationHandlers();

// Configure UI services.
builder.Services.AddRazorPages();

var isDevelopment = builder.Environment.IsDevelopment();

// Starting value for HSTS max age is five minutes to allow for debugging.
// For more info on updating HSTS max age value for production, see:
// https://gaepdit.github.io/web-apps/use-https.html#how-to-enable-hsts
if (!isDevelopment)
{
    builder.Services.AddHsts(options => options.MaxAge = TimeSpan.FromMinutes(300))
        .AddHttpsRedirection(options =>
        {
            options.HttpsPort = 443;
            options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
        });
}

// Configure application monitoring.
builder.Services.AddTransient<IErrorLogger, ErrorLogger>();
builder.Services.AddSingleton(provider =>
{
    var client = new RaygunClient(provider.GetService<RaygunSettings>()!, provider.GetService<IRaygunUserProvider>()!);
    client.SendingMessage += (_, eventArgs) => eventArgs.Message.Details.Tags.Add(builder.Environment.EnvironmentName);
    return client;
});
builder.Services.AddRaygun(opts =>
{
    opts.ApiKey = AppSettings.RaygunSettings.ApiKey;
    opts.ApplicationVersion = AppSettings.SupportSettings.InformationalVersion;
    opts.ExcludeErrorsFromLocal = AppSettings.RaygunSettings.ExcludeErrorsFromLocal;
    opts.IgnoreFormFieldNames = ["*Password"];
    opts.EnvironmentVariables.Add("ASPNETCORE_*");
});
builder.Services.AddRaygunUserProvider();
builder.Services.AddHttpContextAccessor(); // needed by RaygunScriptPartial

// Add app services.
builder.Services.AddAutoMapperProfiles();
builder.Services.AddAppServices();
builder.Services.AddValidators();

// Add data stores.
builder.Services.AddIaipDataServices(AppSettings.DevSettings.UseInMemoryIaipData,
    builder.Configuration.GetConnectionString("IaipConnection"));
builder.Services.AddDataPersistence(builder.Configuration, builder.Environment);
builder.Services.AddFileServices(builder.Configuration);

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
builder.Services.AddWebOptimizer(minifyJavaScript: !isDevelopment);

//Add simple cache.
builder.Services.AddMemoryCache();

// Build the application.
var app = builder.Build();

// Configure error handling.
if (isDevelopment) app.UseDeveloperExceptionPage(); // Development
else app.UseExceptionHandler("/Error"); // Production or Staging

// Configure security HTTP headers
if (!isDevelopment || AppSettings.DevSettings.UseSecurityHeadersInDev)
{
    app.UseHsts();
    app.UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());
}

if (!string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) app.UseRaygun();

// Configure the application pipeline.
app.UseStatusCodePagesWithReExecute("/Error/{0}")
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

// Configure API documentation.
app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
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
