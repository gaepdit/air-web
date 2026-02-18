using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Domain.Core.Entities.ValueObjects;
using AirWeb.Domain.Sbeap.ValueObjects;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AirWeb.AppServices.Sbeap.Customers.Dto;

public record ContactViewDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }

    [UsedImplicitly]
    public string Honorific { get; init; } = string.Empty;

    [UsedImplicitly]
    public string GivenName { get; init; } = string.Empty;

    [UsedImplicitly]
    public string FamilyName { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = string.Empty;

    public string Notes { get; init; } = string.Empty;
    public Address Address { get; init; } = null!;

    [Display(Name = "Phone Numbers")]
    public ICollection<PhoneNumber> PhoneNumbers { get; } = [];

    public StaffViewDto? EnteredBy { get; init; }

    [Display(Name = "Entered On")]
    public DateTimeOffset? EnteredOn { get; init; }

    // Read-only properties

    [JsonIgnore]
    public string Name => new[] { Honorific, GivenName, FamilyName }.ConcatWithSeparator();
}
