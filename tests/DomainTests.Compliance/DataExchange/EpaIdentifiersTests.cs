using AirWeb.Domain.Compliance.DataExchange;

namespace DomainTests.Compliance.DataExchange;

[TestFixture]
[TestOf(typeof(IDataExchange))]
public class IDataExchangeTests
{
    private record DataExchange : IDataExchange
    {
        public string FacilityId { get; init; } = "00100001";
        public DataExchangeStatus DataExchangeStatus { get; set; } = DataExchangeStatus.N;
        public DateTimeOffset? DataExchangeStatusDate { get; set; } = null;
    }

    private record DataExchangeAction : DataExchange, IDataExchangeAction
    {
        public ushort? ActionNumber { get; set; } = 1;
    }

    [Test]
    public void GivenValid_ReturnsEpaFacilityId()
    {
        // Arrange
        var test = new DataExchange();

        // Assert
        ((IDataExchange)test).EpaFacilityId.Should().Be("GA0000001300100001");
    }

    [Test]
    public void GivenValidAction_ReturnsEpaActionIdentifier()
    {
        // Arrange
        var test = new DataExchangeAction();

        // Assert
        ((IDataExchangeAction)test).EpaActionId.Should().Be("GA000A0000130010000100001");
    }

    [Test]
    public void GivenInvalidFacilityId_ThrowsException()
    {
        // Arrange
        var test = new DataExchange { FacilityId = "1" };

        // Act
        var func = () => ((IDataExchange)test).EpaFacilityId;

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void GivenNullActionNumber_ReturnsNull()
    {
        // Arrange
        var test = new DataExchangeAction() { ActionNumber = null };

        // Assert
        ((IDataExchangeAction)test).EpaActionId.Should().BeNull();
    }
}
