using PermitSearch.Platform;

namespace PermitSearchTests;

public class IdValidationTests
{
    [Test]
    [TestCase("ON-123", "ON-123")]
    [TestCase("VF-1", "VF-1")]
    [TestCase("PDF-ON-123", "ON-123")]
    [TestCase("DOC-ON-123", "ON-123")]
    public void ValidatePermit_GivenValidPermit_ShouldReturnTrue(string input, string expected)
    {
        var result = IdValidation.ValidatePermit(input, out var id);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(id, Is.EqualTo(expected));
        }
    }

    [Test]
    [TestCase("")]
    [TestCase("ON-")]
    [TestCase("-1")]
    [TestCase("PDF-123")]
    [TestCase("ON-123456789")]
    [TestCase("ABC-ON-123")]
    public void ValidatePermit_GivenInvalidPermit_ShouldReturnTrue(string input)
    {
        var result = IdValidation.ValidatePermit(input, out var id);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.False);
            Assert.That(id, Is.Null);
        }
    }
}
