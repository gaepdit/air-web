using AirWeb.Domain.DataExchange;

namespace DomainTests.DataExchange;

[TestFixture]
[TestOf(typeof(IDataExchange))]
public class IDataExchangeTests
{
    private record DataExchange : IDataExchange
    {
        public string FacilityId { get; init; } = "00100001";
        public ushort? ActionNumber { get; init; } = 1;
        public DataExchangeStatus DataExchangeStatus => DataExchangeStatus.N;
        public DateTimeOffset? DataExchangeStatusDate => null;
    }

    [Test]
    public void GivenValidFacilityId_ReturnsEpaIdentifier()
    {
        // Arrange
        var test = new DataExchange();

        // Assert
        ((IDataExchange)test).EpaActionId.Should().Be("GA000A0000130010000100001");
    }

    [Test]
    public void GivenInvalidFacilityId_ThrowsException()
    {
        // Arrange
        var test = new DataExchange { FacilityId = "1" };

        // Act
        var act = () => ((IDataExchange)test).EpaActionId;

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void GivenNullActionNumber_ReturnsNull()
    {
        // Arrange
        var test = new DataExchange { ActionNumber = null };

        // Assert
        ((IDataExchange)test).EpaActionId.Should().BeNull();
    }
}
