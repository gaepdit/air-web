using AirWeb.MemRepository.ComplianceRepositories;
using AirWeb.MemRepository.Identity;
using AirWeb.MemRepository.Repositories;

namespace MemRepositoryTests;

public static class RepositoryHelper
{
    public static LocalUserStore GetUserStore() => new();
    public static OfficeMemRepository GetOfficeRepository() => new();
    public static FceMemRepository GetFceRepository() => new();
    public static ComplianceWorkMemRepository GetComplianceWorkRepository() => new();
    public static CaseFileMemRepository GetCaseFileRepository() => new();
    public static EnforcementActionMemRepository GetEnforcementActionRepository() => new();
}
