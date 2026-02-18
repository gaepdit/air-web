using AirWeb.AppServices.Core.EntityServices.LookupsBase;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AutoMapper;

namespace AirWeb.AppServices.Sbeap.ActionItemTypes;

// DTOs

public record ActionItemTypeViewDto : LookupViewDto;

public record ActionItemTypeCreateDto : LookupCreateDto;

public record ActionItemTypeUpdateDto : LookupUpdateDto;

// App Services

public interface IActionItemTypeService : ILookupService<ActionItemTypeViewDto, ActionItemTypeUpdateDto>;

public class ActionItemTypeService(
    IMapper mapper,
    IActionItemTypeRepository repository,
    IActionItemTypeManager manager,
    IUserService userService)
    : LookupService<ActionItemType, ActionItemTypeViewDto, ActionItemTypeUpdateDto>
        (mapper, repository, manager, userService), IActionItemTypeService;

// Validators

public class ActionItemTypeCreateValidator(IActionItemTypeRepository repository)
    : LookupCreateValidator<ActionItemTypeCreateDto, IActionItemTypeRepository, ActionItemType>(repository);

public class ActionItemTypeUpdateValidator(IActionItemTypeRepository repository)
    : LookupUpdateValidator<ActionItemTypeUpdateDto, IActionItemTypeRepository, ActionItemType>
        (repository);
