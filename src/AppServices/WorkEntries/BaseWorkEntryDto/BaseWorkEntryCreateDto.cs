﻿using AirWeb.Domain.Entities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;

public abstract record BaseWorkEntryCreateDto : IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string FacilityId { get; init; } = string.Empty;

    [Required]
    public WorkEntryType WorkEntryType { get; init; } = WorkEntryType.Unknown;

    [Required]
    [Display(Name = "Staff Responsible")]
    public string? ResponsibleStaffId { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string Notes { get; init; } = string.Empty;
}