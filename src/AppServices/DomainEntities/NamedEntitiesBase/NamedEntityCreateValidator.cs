using AirWeb.Domain;
using FluentValidation;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AirWeb.AppServices.DomainEntities.NamedEntitiesBase;

#pragma warning disable S2436 // Types and methods should not have too many generic parameters

public abstract class NamedEntityCreateValidator<TCreateDto, TRepository, TEntity> : AbstractValidator<TCreateDto>
    where TCreateDto : NamedEntityCreateDto
    where TRepository : INamedEntityRepository<TEntity>
    where TEntity : IEntity, INamedEntity
#pragma warning restore S2436
{
    private readonly TRepository _repository;

    protected NamedEntityCreateValidator(TRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token).ConfigureAwait(false))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token).ConfigureAwait(false) is null;
}
