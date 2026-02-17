using AirWeb.Domain.Core.Data.DataProcessing;

namespace CoreTests;

[TestFixture]
[TestOf(typeof(PersonalInformation))]
public class PersonalInformationTests
{
    [Test]
    public void PersonalInformationShouldBeRemoved()
    {
        const string data = "Phone: 404-555-1212; Email: test@example.net!";
        var result = PersonalInformation.RedactPersonalInformation(data);
        result.Should().Be("Phone: 404-[phone number removed]; Email: [email@removed.invalid]!");
    }
}
