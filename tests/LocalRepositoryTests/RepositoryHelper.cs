using AirWeb.LocalRepository.Identity;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;
using AirWeb.TestData.Entities.WorkEntries;
using AirWeb.TestData.Identity;

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

    private static void ClearAllStaticData()
    {
        OfficeData.ClearData();
        UserData.ClearData();
        AllWorkEntryData.ClearData();
    }
}
