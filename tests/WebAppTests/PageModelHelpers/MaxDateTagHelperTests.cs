using AirWeb.Domain.Core.Data.DataAttributes;
using AirWeb.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebAppTests.PageModelHelpers;

public class MaxDateTagHelperTests
{
    private class TestModel
    {
        [MaxDate]
        public DateOnly DateWithMaxToday { get; set; }

        [MaxDate(2025, 12, 31)]
        public DateOnly DateWithSpecificMax { get; set; }

        [MaxDate(2)]
        public DateOnly DateWithDayDelta { get; set; }

        public DateOnly DateWithoutMaxAttribute { get; set; }
    }

    [Test]
    public void Process_WithMaxDateAttribute_SetsMaxAttributeToToday()
    {
        // Arrange
        var tagHelper = new MaxDateTagHelper
        {
            For = CreateModelExpression(nameof(TestModel.DateWithMaxToday)),
        };

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput("input");

        // Act
        tagHelper.Process(context, output);

        // Assert
        var expectedMax = DateTime.Today.ToString(MaxDateAttribute.HtmlInputDate);
        output.Attributes["max"].Value.Should().Be(expectedMax);
    }

    [Test]
    public void Process_WithMaxDateAttributeWithSpecificDate_SetsMaxAttributeToSpecificDate()
    {
        // Arrange
        var tagHelper = new MaxDateTagHelper
        {
            For = CreateModelExpression(nameof(TestModel.DateWithSpecificMax)),
        };

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput("input");

        // Act
        tagHelper.Process(context, output);

        // Assert
        output.Attributes["max"].Value.Should().Be("2025-12-31");
    }

    [Test]
    public void Process_WithMaxDateAttributeWithDayDelta_SetsMaxAttributeToCorrectDate()
    {
        // Arrange
        var tagHelper = new MaxDateTagHelper
        {
            For = CreateModelExpression(nameof(TestModel.DateWithDayDelta)),
        };

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput("input");

        // Act
        tagHelper.Process(context, output);

        // Assert
        output.Attributes["max"].Value.Should()
            .Be(DateOnly.FromDateTime(DateTime.Now).AddDays(2).ToString(MaxDateAttribute.HtmlInputDate));
    }

    [Test]
    public void Process_WithoutMaxDateAttribute_DoesNotSetMaxAttribute()
    {
        // Arrange
        var tagHelper = new MaxDateTagHelper
        {
            For = CreateModelExpression(nameof(TestModel.DateWithoutMaxAttribute)),
        };

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput("input");

        // Act
        tagHelper.Process(context, output);

        // Assert
        output.Attributes.ContainsName("max").Should().BeFalse();
    }

    [Test]
    public void Process_WithExistingMaxAttribute_DoesNotOverride()
    {
        // Arrange
        var tagHelper = new MaxDateTagHelper
        {
            For = CreateModelExpression(nameof(TestModel.DateWithMaxToday)),
        };

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput("input");
        output.Attributes.Add("max", "2020-01-01");

        // Act
        tagHelper.Process(context, output);

        // Assert
        output.Attributes["max"].Value.Should().Be("2020-01-01");
    }

    [Test]
    public void Process_WithNullFor_DoesNotThrow()
    {
        // Arrange
        var tagHelper = new MaxDateTagHelper { For = null };
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput("input");

        // Act
        var act = () => tagHelper.Process(context, output);

        // Assert
        act.Should().NotThrow();
        output.Attributes.ContainsName("max").Should().BeFalse();
    }

    private static ModelExpression CreateModelExpression(string propertyName)
    {
        var metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(TestModel), propertyName);

        return new ModelExpression(propertyName, new ModelExplorer(
            new EmptyModelMetadataProvider(),
            metadata,
            null));
    }

    private static TagHelperContext CreateTagHelperContext()
    {
        return new TagHelperContext(
            [],
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));
    }

    private static TagHelperOutput CreateTagHelperOutput(string tagName)
    {
        return new TagHelperOutput(
            tagName,
            [],
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }
}
