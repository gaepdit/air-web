using AirWeb.Domain.EnforcementEntities;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.TestData.Enforcement;

namespace AirWeb.LocalRepository.Repositories;

public class LocalEnforcementActionRepository()
    : BaseRepository<EnforcementAction, Guid>(EnforcementActionData.GetData),
        IEnforcementActionRepository;
