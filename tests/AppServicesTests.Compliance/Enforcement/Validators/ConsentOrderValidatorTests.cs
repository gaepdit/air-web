using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using FluentValidation.TestHelper;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class ConsentOrderValidatorTests
{
    [Test]
    public async Task DtoWithValidValues_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ZeroOrderId_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 0,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task OrderIdAlreadyExists_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ZeroPenalty_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 1,
            PenaltyAmount = 0,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task NegativePenalty_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 1,
            PenaltyAmount = -1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ReceivedFromFacilityDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ExecutedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ReceivedFromDirectorsOfficeDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task IssuedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ExecutedBeforeReceivedFromFacility_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ReceivedFromDirectorsOfficeBeforeReceivedFromFacility_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ReceivedFromFacility = DateOnly.FromDateTime(DateTime.Today),
            ReceivedFromDirectorsOffice = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ExecutedAndIssuedWithoutReceivedFromDates_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task IssueDateBeforeExecuted_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task IssuedWhenNotExecuted_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedBeforeExecuted_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            IssueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-3),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedBeforeIssued_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedWhenNotExecuted_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedWhenNotIssued_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.OrderIdExists(Arg.Any<short>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new ConsentOrderCommandValidator(repoMock);
        var model = new ConsentOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
            OrderId = 1,
            PenaltyAmount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
