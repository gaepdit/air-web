using AirWeb.Domain.Core.Data;
using AirWeb.Domain.Core.Entities;
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

    public string? Description { get; set; }

    // TODO: Change SicCode to SIC; change SicCode.Id to SIC.Code; Change SicCodeId to SicCode
    public string? SicCodeId { get; set; }
    public SicCode? SicCode => SicCodeId is null ? null : SicCodes.Get(SicCodeId);
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
