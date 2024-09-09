﻿using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public record WorkEntryCreateDto : IWorkEntryCreateDto
{
    [Required]
    public string? FacilityId { get; init; }

    [Required]
    [Display(Name = "Staff Responsible")]
    public string? ResponsibleStaffId { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Notes { get; init; } = string.Empty;
}
