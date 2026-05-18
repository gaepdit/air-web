using System;
using System.Collections.Generic;
using System.Text;
using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using IaipDataService.Facilities;

namespace AppServicesTests.Compliance.Fces.Validators;

public class FceSearchValidatorTests
{
    private IFacilityService _service = null!;
    private FceSearchValidator _sut;
    private IFacilityService _serviceFalse = null!;
    private FceSearchValidator _sutFalse;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _service = Substitute.For<IFacilityService>();
        _service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(true);
        _sut = new FceSearchValidator(_service);

        _serviceFalse = Substitute.For<IFacilityService>();
        _service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(false);
        _sut = new FceSearchValidator( _serviceFalse);
    }


}
