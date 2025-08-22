using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;

namespace AppServicesTests.Enforcement.Validators;

[TestFixture]
public class EnforcementActionReviewValidatorTests
{
    [Test]
    public void RequestDto_Valid_ReturnsAsValid()
    {
        // Arrange
        var validator = new ReviewRequestValidator();
        var model = new EnforcementActionRequestReviewDto { RequestedOfId = "1" };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void RequestDto_MissingReviewer_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new ReviewRequestValidator();
        var model = new EnforcementActionRequestReviewDto();

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public void SubmitDto_Valid_ReturnsAsValid()
    {
        // Arrange
        var validator = new SubmitReviewValidator();
        var model = new EnforcementActionSubmitReviewDto { Result = ReviewResult.Approved };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void SubmitDto_ValidForwarded_ReturnsAsValid()
    {
        // Arrange
        var validator = new SubmitReviewValidator();
        var model = new EnforcementActionSubmitReviewDto { Result = ReviewResult.Forwarded, RequestedOfId = "1" };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void SubmitDto_ForwardedMissingReviewer_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new SubmitReviewValidator();
        var model = new EnforcementActionSubmitReviewDto { Result = ReviewResult.Forwarded };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public void SubmitDto_MissingResult_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new SubmitReviewValidator();
        var model = new EnforcementActionSubmitReviewDto { RequestedOfId = "1" };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
