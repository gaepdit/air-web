using AirWeb.Domain.Core.Entities;

namespace AirWeb.AppServices.Core.EntityServices.Staff.Dto;

public record StaffUpdateDto
{
    [StringLength(ApplicationUser.MaxPhoneLength,
        ErrorMessage = "The Phone Number must not be longer than {1} characters.")]
    [Display(Name = "Phone")]
    [Phone]
    public string? PhoneNumber { get; init; }

    [Required]
    [Display(Name = "Office")]
    public Guid? OfficeId { get; init; }

    [Required]
    public bool Active { get; set; }
}
