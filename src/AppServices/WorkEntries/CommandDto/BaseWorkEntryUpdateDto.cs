﻿using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.CommandDto;

public abstract record BaseWorkEntryUpdateDto : IWorkEntryUpdateDto
{
    // Authorization handler assist properties
    public bool IsDeleted { get; init; }

    // Data
    [Required]
    [Display(Name = "Staff Responsible")]
    public string? ResponsibleStaffId { get; init; }

    [Display(Name = "Closed")]
    public bool IsClosed { get; init; }

    [Display(Name = "Date Completed")]
    public DateOnly? ClosedDate { get; init; }

    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string Notes { get; init; } = string.Empty;
}
