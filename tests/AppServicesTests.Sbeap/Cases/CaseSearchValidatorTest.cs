using FluentValidation.TestHelper;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Cases.Validators;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class CaseSearchValidatorTests
{
    private CaseSearchValidator _sut = null;

    [SetUp]
    public void SetUp()
    {
        // Arrange           
        _sut = new CaseSearchValidator();
    }
    [Test]
    public async Task EmptyDto_ReturnsAsValid()
    {
        // Arrange
        var model = new CaseworkSearchDto();

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new CaseworkSearchDto
        {
            OpenedFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            OpenedThrough = DateOnly.FromDateTime(DateTime.Today),
            ClosedFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            ClosedThrough = DateOnly.FromDateTime(DateTime.Today),
            ReferredFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            ReferredThrough = DateOnly.FromDateTime(DateTime.Today)
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
