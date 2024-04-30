namespace DomainTests.DataProcessing;

public class PersonalInformation
{
    [Test]
    public void PersonalInformationShouldBeRemoved()
    {
        const string data = "Phone: 404-555-1212; Email: test@example.net!";
        var result = MyApp.Domain.DataProcessing.PersonalInformation.RedactPersonalInformation(data);
        result.Should().Be("Phone: 404-[phone number removed]; Email: [email@removed.invalid]!");
    }
}
