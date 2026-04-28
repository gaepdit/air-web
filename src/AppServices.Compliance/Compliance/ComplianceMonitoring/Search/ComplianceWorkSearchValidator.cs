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

        RuleFor(dto => dto.EventDateTo)
            .Cascade(CascadeMode.Stop)
            .Must(date => date <= today || date == null)
            .WithMessage("The Event To Date cannot be in the future")
            .Must((dto, date) => dto.EventDateFrom == default || date >= dto.EventDateFrom || date == null)
            .WithMessage("The Even To Date must be later than the From Date");
    }
}
