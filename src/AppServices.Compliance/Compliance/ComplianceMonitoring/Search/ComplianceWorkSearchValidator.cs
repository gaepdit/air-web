using System;
using System.Collections.Generic;
using System.Text;

using AirWeb.Domain.Compliance;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;

public class ComplianceWorkValidator : AbstractValidator<ComplianceWorkSearchDto>
{
    private readonly IFacilityService _service;
    public ComplianceWorkValidator(IFacilityService service)
    {
        _service = service;
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.EventDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The Event From Date cannot be in the future.");
    }
}
