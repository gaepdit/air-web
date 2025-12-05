using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Enforcement.Validators;

public class CaseFileCreateValidatorTests
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var validator = new CaseFileCreateValidator(Substitute.For<IWorkEntryRepository>());
        var model = new CaseFileCreateDto
        {
            ResponsibleStaffId = "1",
            DiscoveryDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DiscoveryDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new CaseFileCreateValidator(Substitute.For<IWorkEntryRepository>());
        var model = new CaseFileCreateDto
        {
            ResponsibleStaffId = "1",
            DiscoveryDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ValidDto_WithComplianceEvent_ReturnsAsValid()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var report = new Report(1, (FacilityId)"00100001") { ReceivedDate = date };

        var entryRepoMock = Substitute.For<IWorkEntryRepository>();
        entryRepoMock.GetAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(report);

        var validator = new CaseFileCreateValidator(entryRepoMock);
        var model = new CaseFileCreateDto
        {
            EventId = report.Id,
            ResponsibleStaffId = "1",
            DiscoveryDate = date,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DiscoveryDatePrecedesComplianceEvent_ReturnsAsInvalid()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var report = new Report(1, (FacilityId)"00100001") { ReceivedDate = date };

        var entryRepoMock = Substitute.For<IWorkEntryRepository>();
        entryRepoMock.GetAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(report);

        var validator = new CaseFileCreateValidator(entryRepoMock);
        var model = new CaseFileCreateDto
        {
            EventId = report.Id,
            ResponsibleStaffId = "1",
            DiscoveryDate = date.AddDays(-1),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task MissingStaff_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new CaseFileCreateValidator(Substitute.For<IWorkEntryRepository>());
        var model = new CaseFileCreateDto
        {
            ResponsibleStaffId = null,
            DiscoveryDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
