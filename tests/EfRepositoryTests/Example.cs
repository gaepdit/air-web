namespace EfRepositoryTests;

[TestFixture]
public class Example
{
    public static int? Divide(int num, int den)
    {
        if (den == 0) throw new ArgumentException("den cannot be 0");
        return num / den;
    }

    [Test]
    [TestCase(2, 2, 1)]
    public void Divide_works(int num, int den, int expected)
    {
        Divide(num, den).Should().Be(expected);
    }

    [Test]
    [TestCase(2, 0, 0)]
    public void GivenNonexistentId_Throws(int num, int den, int expected)
    {
        // Act
        var func = () => Divide(num, den);

        // Assert
        func.Should().Throw<ArgumentException>();
    }
}
