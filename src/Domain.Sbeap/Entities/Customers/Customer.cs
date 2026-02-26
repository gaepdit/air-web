using AirWeb.Domain.Core.Data;
using AirWeb.Domain.Core.ValueObjects;
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

    [StringLength(4000)]
    public string? Description { get; set; }

    [StringLength(4)]
    public string? SicCode { get; set; }

    public Sic? Sic => SicCode is null ? null : SicData.Find(SicCode);

    [StringLength(13)]
    public string? County { get; set; }

    [StringLength(2000)] // https://stackoverflow.com/q/417142/212978
    public string? Website { get; set; }

    public Address Location { get; set; } = null!;
    public Address MailingAddress { get; set; } = null!;

    // Collections

    public List<Contact> Contacts { get; set; } = [];
    public List<Casework> Cases { get; set; } = [];

    // Properties: Deletion

    [StringLength(4000)]
    public string? DeleteComments { get; set; }
}
