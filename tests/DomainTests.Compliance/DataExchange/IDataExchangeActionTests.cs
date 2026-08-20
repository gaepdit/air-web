using AirWeb.Domain.Compliance.DataExchange;

namespace DomainTests.Compliance.DataExchange;

[TestFixture]
[TestOf(typeof(IDataExchangeAction))]
public class IDataExchangeActionTests
{
    private record DataExchangeAction : IDataExchangeAction
    {
        public string FacilityId => "00100001";
        public DataExchangeStatus DataExchangeStatus { get; set; } = DataExchangeStatus.N;
        public DateTimeOffset? DataExchangeStatusDate { get; set; } = null;
        public ushort? ActionNumber { get; set; } = 1;
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
    public void GivenNullActionNumber_ReturnsNull()
    {
        // Arrange
        var test = new DataExchangeAction() { ActionNumber = null };

        // Assert
        ((IDataExchangeAction)test).EpaActionId.Should().BeNull();
    }
}
