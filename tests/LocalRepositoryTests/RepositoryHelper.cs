using AirWeb.LocalRepository.Identity;
using AirWeb.LocalRepository.Repositories;

namespace LocalRepositoryTests;

public static class RepositoryHelper
{
    public static LocalUserStore GetUserStore() => new();
    public static LocalOfficeRepository GetOfficeRepository() => new();
    public static LocalFceRepository GetFceRepository() => new();
    public static LocalComplianceWorkRepository GetComplianceWorkRepository() => new();
    public static LocalCaseFileRepository GetCaseFileRepository() => new();
    public static LocalEnforcementActionRepository GetEnforcementActionRepository() => new();
}
