﻿namespace AirWeb.WebApp.Pages.Dev;

// FUTURE: Remove this page once testing of error handling is complete.
[AllowAnonymous]
public class ThrowErrorModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public void OnGetHandledAsync([FromServices] ILogger<ThrowErrorModel> logger)
    {
        try
        {
            throw new TestException("Test handled exception");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Handled exception message");
        }
    }

    public void OnGetUnhandled()
    {
        throw new TestException("Test unhandled exception");
    }


    public class TestException(string message) : Exception(message);
}
