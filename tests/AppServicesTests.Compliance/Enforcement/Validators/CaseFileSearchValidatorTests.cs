using System;
using System.Collections.Generic;
using System.Text;
using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class CaseFileSearchValidatorTests
{
    private IFacilityService _service = null!;
    private CaseFileSearchValidator _sut;
    private IFacilityService _serviceFalse = null!;
    private CaseFileSearchValidator _sutFalse;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _service = Substitute.For<IFacilityService>();
        _service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(true);
        _sut = new CaseFileSearchValidator(_service);

        _serviceFalse = Substitute.For<IFacilityService>();
        _serviceFalse.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(false);
        _sutFalse = new CaseFileSearchValidator(_serviceFalse);
    }

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            FacilityId = SampleText.ValidFacilityId,
            DiscoveryDateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            DiscoveryDateTo = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
