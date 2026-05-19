using System;
using System.Collections.Generic;
using System.Text;
using AirWeb.AppServices.Compliance.Enforcement.Search;
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
}
