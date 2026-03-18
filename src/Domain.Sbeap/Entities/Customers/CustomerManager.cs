using AirWeb.Domain.Sbeap.Entities.Contacts;
using AirWeb.Domain.Sbeap.ValueObjects;

namespace AirWeb.Domain.Sbeap.Entities.Customers;

public class CustomerManager(IContactRepository contactRepository) : ICustomerManager
{
    public Customer Create(string name, string? createdById)
    {
        var item = new Customer(Guid.NewGuid()) { Name = name };
        item.SetCreator(createdById);
        return item;
    }

    public Contact CreateContact(Customer customer, string? createdById)
    {
        var item = new Contact(Guid.NewGuid(), customer) { EnteredOn = DateTimeOffset.Now };
        item.SetCreator(createdById);
        return item;
    }

    public PhoneNumber CreatePhoneNumber(string number, PhoneType type) =>
        new()
        {
            Id = contactRepository.GetNextPhoneNumberId(),
            Number = number,
            Type = type,
        };
}
