using IaipDataService.Facilities;

namespace IaipDataServiceTests;

[TestFixture]
[TestOf(typeof(FacilityId))]
public class FacilityIdTests
{
    [Test]
    [TestCase("00100001")]
    [TestCase("00110000")]
    [TestCase("00199999")]
    [TestCase("32100001")]
    [TestCase("32110000")]
    [TestCase("32199999")]
    [TestCase("77700001")]
    [TestCase("77710000")]
    [TestCase("77799999")]
    [TestCase("001-00001")]
    [TestCase("001-10000")]
    [TestCase("001-99999")]
    [TestCase("321-00001")]
    [TestCase("321-10000")]
    [TestCase("321-99999")]
    [TestCase("777-00001")]
    [TestCase("777-10000")]
    [TestCase("777-99999")]
    [TestCase("1-1")]
    [TestCase("1-10")]
    [TestCase("1-10000")]
    [TestCase("1-99999")]
    [TestCase("321-1")]
    [TestCase("777-1")]
    [TestCase("321-10")]
    [TestCase("777-10")]
    [TestCase("1-01")]
    [TestCase("1-10")]
    [TestCase("1-001")]
    [TestCase("1-0001")]
    [TestCase("1-00001")]
    [TestCase("01-1")]
    [TestCase("01-10")]
    [TestCase("01-99999")]
    [TestCase("041300100001")]
    public void IsValidFormat_ValidIdFormat_Accepted(string input)
    {
        FacilityId.IsValidFormat(input).Should().BeTrue();
    }

    [Test]
    [TestCase("")]
    [TestCase("111")]
    [TestCase("ABC")]
    [TestCase("04130010000")]
    [TestCase("041300200001")]
    [TestCase("041332300001")]
    [TestCase("041300000001")]
    [TestCase("041300100000")]
    [TestCase("04-13-001-00001")]
    [TestCase("0413-001-0001")]
    [TestCase("abc-defgh")]
    [TestCase("001-0000a")]
    [TestCase("0100001")]
    [TestCase("0010001")]
    [TestCase("00200001")]
    [TestCase("002-00001")]
    [TestCase("2-1")]
    [TestCase("32300001")]
    [TestCase("323-00001")]
    [TestCase("323-1")]
    [TestCase("00000001")]
    [TestCase("000-00001")]
    [TestCase("000-99999")]
    [TestCase("0-1")]
    [TestCase("00100000")]
    [TestCase("001-00000")]
    [TestCase("001-0")]
    [TestCase("1-0")]
    public void IsValidFormat_InvalidIdFormat_Rejected(string input)
    {
        FacilityId.IsValidFormat(input).Should().BeFalse();
    }

    [Test]
    public void Normalize_ValidIdFormat_SucceedsAndNormalizesId()
    {
        var result = new FacilityId("1-1");
        result.Id.Should().Be("00100001");
        result.FormattedId.Should().Be("001-00001");
        result.ToString().Should().Be("001-00001");
        result.EpaFacilityIdentifier.Should().Be("GA0000001300100001");
    }

    [Test]
    public void Normalize_InvalidIdFormat_Throws()
    {
        var func = () => new FacilityId("0-1");
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    [TestCase("00100001")]
    [TestCase("001-00001")]
    public void ImplicitOperator_Succeeds(string input)
    {
        var result = (FacilityId)input;
        result.Id.Should().Be("00100001");
    }

    [Test]
    [TestCase("00100001")]
    [TestCase("001-00001")]
    public void ExplicitOperator_Succeeds(string input)
    {
        string result = new FacilityId(input);
        result.Should().Be("001-00001");
    }

    [Test]
    public void Equals_WhenEqual_ReturnsTrue()
    {
        var value1 = new FacilityId("1-1");
        var value2 = new FacilityId("041300100001");
        value1.Equals(value2).Should().BeTrue();
    }

    [Test]
    public void Equals_WhenNotEqual_ReturnsFalse()
    {
        var value1 = new FacilityId("1-1");
        var value2 = new FacilityId("1-2");
        value1.Equals(value2).Should().BeFalse();
    }

    [Test]
    public void TryParse_ValidIdFormat_Succeeds()
    {
        var result = FacilityId.TryParse("1-1", out var facilityId);
        result.Should().BeTrue();
        facilityId.Should().NotBeNull();
        facilityId.Id.Should().Be("00100001");
    }

    [Test]
    public void TryParse_InvalidIdFormat_Fails()
    {
        var result = FacilityId.TryParse("0-1", out var facilityId);
        result.Should().BeFalse();
        facilityId.Should().BeNull();
    }

    [Test]
    public void TryParse_NullInput_Fails()
    {
        var result = FacilityId.TryParse(null, out var facilityId);
        result.Should().BeFalse();
        facilityId.Should().BeNull();
    }

    [Test]
    [TestCase("1-1", "1-1")]
    [TestCase("11", "11")]
    [TestCase("a", "")]
    [TestCase("a1-", "1-")]
    [TestCase("001-00001", "001-00001")]
    public void CleanFacilityId_PartialId_ReturnsCleanedValue(string input, string expected)
    {
        FacilityId.CleanPartialFacilityId(input).Should().Be(expected);
    }

    [Test]
    [TestCase("00100001", "001-00001")]
    [TestCase("00000000", "000-00000")]
    public void CleanFacilityId_FullId_ReturnsFormattedValue(string input, string expected)
    {
        FacilityId.CleanPartialFacilityId(input).Should().Be(expected);
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public void CleanFacilityId_EmptyId_ReturnsEmptyString(string? input)
    {
        var result = FacilityId.CleanPartialFacilityId(input);
        result.Should().Be(string.Empty);
    }
}
