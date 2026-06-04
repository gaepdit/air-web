using AirWeb.Domain.Core.Entities;
using IaipDataService.Structs;
using IaipDataService.TestData;

namespace AirWeb.TestData.Identity;

internal static partial class UserData
{
    public static readonly string AdminUserId = Staff.IntToGuid(1).ToString();
    public static ApplicationUser GetRandomUser() => Users[Random.Shared.Next(Users.Count)];

    private static List<ApplicationUser>? _users;

    public static List<ApplicationUser> Users
    {
        get
        {
            if (_users is not null) return _users;

            _users = StaffData.GetData
                .OrderBy(s => s.Id)
                .Select(staff => new ApplicationUser
                {
                    Id = staff.IdAsGuid.ToString(),
                    GivenName = staff.Name.GivenName,
                    FamilyName = staff.Name.FamilyName,
                    Email = staff.EmailAddress,
                    UserName = staff.EmailAddress.ToLowerInvariant(),
                    NormalizedEmail = staff.EmailAddress.ToUpperInvariant(),
                    NormalizedUserName = staff.EmailAddress.ToUpperInvariant(),
                    Office = OfficeData.GetRandomOffice(),
                    Active = staff.Active,
                }).ToList();

            return _users;
        }
    }

    public static void ClearData() => _users = null;
}
