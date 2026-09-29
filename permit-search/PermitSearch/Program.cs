using PermitSearch.Platform;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddZLoggerConsole(options => options.UseJsonFormatter());
builder.LoadSettings();
builder.Services.AddRazorPages();

var app = builder.Build();
app.UseHttpsRedirection().UseRewriter(AppUrlRewrite.Options);
app.MapRazorPages();

await app.RunAsync();
