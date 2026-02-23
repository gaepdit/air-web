using AirWeb.Domain.Sbeap.Entities.Agencies;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.SbeapRepositories;

public sealed class AgencyRepository(AppDbContext context)
    : NamedEntityRepository<Agency, AppDbContext>(context), IAgencyRepository;
