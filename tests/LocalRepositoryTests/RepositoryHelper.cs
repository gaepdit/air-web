using MyApp.LocalRepository.Identity;
using MyApp.LocalRepository.Repositories;
using MyApp.TestData;
using MyApp.TestData.Identity;

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
        EntryActionData.ClearData();
        WorkEntryData.ClearData();
    }
}
