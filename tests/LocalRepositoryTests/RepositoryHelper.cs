using AirWeb.LocalRepository.Identity;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Identity;
using AirWeb.TestData.NamedEntities;

namespace LocalRepositoryTests;

public static class RepositoryHelper
{
    public static LocalUserStore GetUserStore()
    {
        ClearAllStaticData();
        return new LocalUserStore();
    }

    public static LocalOfficeRepository GetOfficeRepository()
    {
        ClearAllStaticData();
        return new LocalOfficeRepository();
    }

    public static LocalFceRepository GetFceRepository()
    {
        ClearAllStaticData();
        return new LocalFceRepository();
    }

    public static LocalWorkEntryRepository GetWorkEntryRepository()
    {
        ClearAllStaticData();
        return new LocalWorkEntryRepository();
    }

    private static void ClearAllStaticData()
    {
        FceData.ClearData();
        WorkEntryData.ClearData();
        NotificationTypeData.ClearData();
        OfficeData.ClearData();
        UserData.ClearData();
    }
}
