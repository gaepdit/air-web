using AirWeb.Domain;
using FluentValidation;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AirWeb.AppServices.NamedEntities.NamedEntitiesBase;

#pragma warning disable S2436 // Types and methods should not have too many generic parameters

public class NamedEntityUpdateValidator<TDto, TRepository, TEntity> : AbstractValidator<TDto>
    where TDto : NamedEntityUpdateDto
    where TRepository : INamedEntityRepository<TEntity>
    where TEntity : IEntity, INamedEntity
#pragma warning restore S2436
{
    private readonly TRepository _repository;

    protected NamedEntityUpdateValidator(TRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, context, token) =>
                await NotDuplicateName(name, context, token).ConfigureAwait(false))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, ValidationContext<TDto> context,
        CancellationToken token)
    {
        var item = await _repository.FindByNameAsync(name, token).ConfigureAwait(false);
        return item is null || item.Id == (Guid)context.RootContextData[nameof(item.Id)];
    }
}
