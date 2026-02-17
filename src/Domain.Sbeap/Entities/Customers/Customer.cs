using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Core.Entities.ValueObjects;
using AirWeb.Domain.Sbeap.Entities.Cases;
using AirWeb.Domain.Sbeap.Entities.Contacts;

namespace AirWeb.Domain.Sbeap.Entities.Customers;

public class Customer : AuditableSoftDeleteEntity
{
    // Constants

    public const int MinNameLength = 2;

    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Customer() { }

    internal Customer(Guid id) : base(id) { }

    // Properties

    [StringLength(4000, MinimumLength = MinNameLength)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    public SicCode? SicCode { get; set; }
    public string? County { get; set; }

    [MaxLength(2000)] // https://stackoverflow.com/q/417142/212978
    public string? Website { get; set; }

    public Address Location { get; set; } = null!;
    public Address MailingAddress { get; set; } = null!;

    // Collections

    public List<Contact> Contacts { get; set; } = [];
    public List<Casework> Cases { get; set; } = [];

    // Properties: Deletion

    public string? DeleteComments { get; set; }
}
