using AirWeb.AppServices.ErrorLogging;

namespace AirWeb.WebApp.Pages.Dev;

// FUTURE: Remove this page once testing of error handling is complete.
[AllowAnonymous]
public class ThrowErrorModel(IErrorLogger errorLogger) : PageModel
{
    public string ShortCode { get; private set; } = string.Empty;

    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task OnGetHandledAsync()
    {
        try
        {
            throw new TestException("Test handled exception");
        }
        catch (Exception e)
        {
            ShortCode = await errorLogger.LogErrorAsync(e);
        }
    }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static 
    public void OnGetUnhandled()
    {
        throw new TestException("Test unhandled exception");
    }
#pragma warning restore S2325


    public class TestException(string message) : Exception(message);
}
