using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace WebAppTests.DisplayMessages;

public class DisplayMessageTests
{
    private class TestPage : PageModel;

    [Test]
    public void SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Arrange
        var page = new TestPage { TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage = new DisplayMessage(DisplayMessage.AlertContext.Info, "Info message");

        // Act
        page.TempData.AddDisplayMessage(expectedMessage.Context, expectedMessage.Message);

        // Assert
        page.TempData.GetDisplayMessages().Should().BeEquivalentTo([expectedMessage]);
    }

    [Test]
    public void SetMultipleDisplayMessages_ReturnsWithDisplayMessages()
    {
        // Arrange
        // The actual Page model here doesn't matter. DisplayMessage is available for all pages.
        var page = new TestPage { TempData = WebAppTestsSetup.PageTempData() };
        var message1 = new DisplayMessage(DisplayMessage.AlertContext.Info, "Info message one");
        var message2 = new DisplayMessage(DisplayMessage.AlertContext.Success, "Info message two");

        // Act
        page.TempData.AddDisplayMessage(message1.Context, message1.Message);
        page.TempData.AddDisplayMessage(message2.Context, message2.Message);

        // Assert
        page.TempData.GetDisplayMessages().Should().BeEquivalentTo([message1, message2]);
    }
}
