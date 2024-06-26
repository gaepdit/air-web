using AirWeb.Domain.ExternalEntities.Facilities;

namespace DomainTests;

[TestFixture]
[TestOf(typeof(FacilityId))]
public class FacilityIdTests
{
    [Test]
    [TestCase("")]
    [TestCase("1")]
    [TestCase("0010001")]
    [TestCase("001-0001")]
    [TestCase("AAA-BBBBB")]
    public void InvalidIdFormat_Throws(string input)
    {
        var func = () => new FacilityId(input);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    [TestCase("00100001")]
    [TestCase("001-00001")]
    public void ValidIdFormat_Succeeds(string input)
    {
        var result = new FacilityId(input);
        result.Id.Should().Be("001-00001");
        result.ToString().Should().Be("001-00001");
        result.EpaFacilityIdentifier.Should().Be("GA0000001300100001");
    }

    [Test]
    [TestCase("00100001")]
    [TestCase("001-00001")]
    public void ImplicitOperator_Succeeds(string input)
    {
        var result = (FacilityId)input;
        result.Id.Should().Be("001-00001");
        result.ToString().Should().Be("001-00001");
        result.EpaFacilityIdentifier.Should().Be("GA0000001300100001");
    }

    [Test]
    [TestCase("00100001")]
    [TestCase("001-00001")]
    public void ExplicitOperator_Succeeds(string input)
    {
        string result = new FacilityId(input);
        result.Should().Be("001-00001");
    }
}
