using AirWeb.Domain.Sbeap.Entities.Agencies;
using AirWeb.TestData.Sbeap;

namespace AirWeb.MemRepository.SbeapRepositories;

public sealed class AgencyMemRepository() : NamedEntityRepository<Agency>(AgencyData.GetAgencies), IAgencyRepository;
