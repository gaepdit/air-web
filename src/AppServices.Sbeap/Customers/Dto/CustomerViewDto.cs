using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Sbeap.Customers.Dto;

public record CustomerViewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? County { get; init; }
    public string Description { get; init; } = string.Empty;

    [Display(Name = "SIC Code")]
    public string? SicCodeDisplay { get; init; }

    public string? Website { get; init; }

    [Display(Name = "Site Location")]
    public Address Location { get; init; } = null!;

    [Display(Name = "Mailing Address")]
    public Address MailingAddress { get; init; } = null!;

    [UsedImplicitly]
    public List<ContactViewDto> Contacts { get; } = new();

    [UsedImplicitly]
    public List<CaseworkSearchResultDto> Cases { get; } = new();

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }
}
