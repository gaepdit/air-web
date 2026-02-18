using AirWeb.AppServices.Sbeap.Customers;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AppServicesTests.Sbeap.TestData;

namespace AppServicesTests.Sbeap.Customers;

public class CustomerFilterTests
{
    private static CustomerSearchDto DefaultCustomerSearchDto => new();

    [Test]
    public void DefaultFilter_ReturnsAllNotDeleted()
    {
        // Arrange
        var spec = DefaultCustomerSearchDto;
        var expected = CustomerData.GetCustomers.Where(e => !e.IsDeleted);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeletedSpec_ReturnsAllDeleted()
    {
        // Arrange
        var spec = DefaultCustomerSearchDto with { DeletedStatus = CustomerDeletedStatus.Deleted };
        var expected = CustomerData.GetCustomers.Where(e => e.IsDeleted);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeletedSpecNeutral_ReturnsAll()
    {
        // Arrange
        var spec = DefaultCustomerSearchDto with { DeletedStatus = CustomerDeletedStatus.All };

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(CustomerData.GetCustomers);
    }

    [Test]
    public void TextNameSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CustomerData.GetCustomers.First(e => !e.IsDeleted);
        var spec = DefaultCustomerSearchDto with { Name = referenceItem.Name };
        var expected = CustomerData.GetCustomers.Where(e => !e.IsDeleted && e.Name == referenceItem.Name);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TextCitySpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem =
            CustomerData.GetCustomers.First(e => !e.IsDeleted && !string.IsNullOrEmpty(e.Location.City));
        var spec = DefaultCustomerSearchDto with { City = referenceItem.Location.City };
        var expected = CustomerData.GetCustomers.Where(e =>
            !e.IsDeleted && (e.Location.City == spec.City || e.MailingAddress.City == spec.City));

        var predicate = CustomerFilters.CustomerSearchPredicate(spec);

        // Act
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TextCountySpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CustomerData.GetCustomers.First(e => !e.IsDeleted && !string.IsNullOrEmpty(e.County));
        var spec = DefaultCustomerSearchDto with { County = referenceItem.County };
        var expected = CustomerData.GetCustomers.Where(e => !e.IsDeleted && e.County == spec.County);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TextDescriptionSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CustomerData.GetCustomers.First(e => !e.IsDeleted && !string.IsNullOrEmpty(e.Description));
        var spec = DefaultCustomerSearchDto with { Description = referenceItem.Description };
        var expected = CustomerData.GetCustomers.Where(e => !e.IsDeleted && e.Description == spec.Description);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
