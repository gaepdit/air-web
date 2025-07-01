﻿using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Utilities;

namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public abstract record PermitRevocationCommandDto : WorkEntryCommandDto, IPermitRevocationCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Permit Revocation Date")]
    public DateOnly PermitRevocationDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Physical Shutdown Date")]
    public DateOnly? PhysicalShutdownDate { get; init; }

    [Display(Name = "Follow-Up Action Taken")]
    public bool FollowupTaken { get; init; }
}
