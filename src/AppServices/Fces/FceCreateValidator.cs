﻿using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Entities.Fces;
using FluentValidation;

namespace AirWeb.AppServices.Fces;

public abstract class FceCreateValidator : AbstractValidator<FceCreateDto>
{
    private readonly IFceRepository _repository;

    protected FceCreateValidator(IFceRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.FacilityId)
            .NotNull();

        RuleFor(dto => dto)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (dto, token) => await UniqueFacilityYear(dto.FacilityId!, dto.Year, token).ConfigureAwait(false))
            .WithMessage("An FCE already exists for that facility and year.");
    }

    private async Task<bool> UniqueFacilityYear(FacilityId facilityId, int year, CancellationToken token = default) =>
        !await _repository.ExistsAsync(facilityId, year, token).ConfigureAwait(false);
}
