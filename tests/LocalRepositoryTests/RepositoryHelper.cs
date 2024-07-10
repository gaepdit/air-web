using AirWeb.LocalRepository.Identity;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;
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

    public static LocalFceRepository GetFceRepository()
    {
        ClearAllStaticData();
        return new LocalFceRepository();
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
