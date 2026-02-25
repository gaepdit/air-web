using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Core.ValueObjects;
using AirWeb.Domain.Sbeap.Entities.Customers;
using AirWeb.Domain.Sbeap.ValueObjects;

namespace AirWeb.Domain.Sbeap.Entities.Contacts;

public class Contact : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Contact() { }

    internal Contact(Guid id, Customer customer) : base(id)
    {
        Customer = customer;
    }

    // Properties

    public Customer Customer { get; private init; } = null!;
    public ApplicationUser? EnteredBy { get; set; }
    public DateTimeOffset? EnteredOn { get; init; }

    public string? Honorific { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Title { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public string? Notes { get; set; }
    public Address Address { get; set; } = null!;

    // Collections

    public List<PhoneNumber> PhoneNumbers { get; } = [];
}
