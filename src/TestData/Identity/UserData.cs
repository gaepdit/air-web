﻿using AirWeb.Domain.Identity;
using AirWeb.TestData.Lookups;
using IaipDataService.TestData;

namespace AirWeb.TestData.Identity;

internal static partial class UserData
{
    public static readonly string AdminUserId = IntToGuid(1).ToString();
    public static ApplicationUser GetRandomUser() => GetData[Random.Shared.Next(GetData.Count)];

    private static List<ApplicationUser>? _users;

    public static List<ApplicationUser> GetData
    {
        get
        {
            if (_users is not null) return _users;

            _users = StaffData.GetData.Select(staff => new ApplicationUser
            {
                Id = IntToGuid(staff.Id).ToString(),
                GivenName = staff.Name.GivenName,
                FamilyName = staff.Name.FamilyName,
                Email = staff.EmailAddress,
                UserName = staff.EmailAddress.ToLowerInvariant(),
                NormalizedEmail = staff.EmailAddress.ToUpperInvariant(),
                NormalizedUserName = staff.EmailAddress.ToUpperInvariant(),
                Office = OfficeData.GetRandomOffice(),
                Active = staff.Id != 3,
            }).ToList();

            return _users;
        }
    }

    private static Guid IntToGuid(int value)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }

    public static void ClearData() => _users = null;
}
