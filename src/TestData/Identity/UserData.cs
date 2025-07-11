﻿using AirWeb.Domain.Identity;
using AirWeb.TestData.NamedEntities;

namespace AirWeb.TestData.Identity;

internal static partial class UserData
{
    public const string AdminUserId = "00000000-0000-0000-0000-000000000001";
    public static ApplicationUser GetRandomUser() => GetUsers.ElementAt(Random.Shared.Next(GetUsers.Count()));

    private static IEnumerable<ApplicationUser> UserSeedItems =>
    [
        new() // 0
        {
            Id = AdminUserId,
            GivenName = "Test",
            FamilyName = "User1",
            Email = "test.user@example.net",
            Office = OfficeData.GetData.ElementAt(0),
        },
        new() // 1
        {
            Id = "00000000-0000-0000-0000-000000000002",
            GivenName = "Another",
            FamilyName = "User2",
            Email = "another.user2@example.net",
            Office = OfficeData.GetData.ElementAt(1),
        },
        new() // 2
        {
            Id = "00000000-0000-0000-0000-000000000003",
            GivenName = "Inactive",
            FamilyName = "User3",
            Email = "inactive.user@example.net",
            Active = false,
            Office = OfficeData.GetData.ElementAt(0),
        },
    ];

    private static List<ApplicationUser>? _users;

    public static IEnumerable<ApplicationUser> GetUsers
    {
        get
        {
            if (_users is not null) return _users;

            _users = UserSeedItems.ToList();
            foreach (var user in _users)
            {
                user.UserName = user.Email?.ToLowerInvariant();
                user.NormalizedEmail = user.Email?.ToUpperInvariant();
                user.NormalizedUserName = user.Email?.ToUpperInvariant();
            }

            return _users;
        }
    }

    public static void ClearData() => _users = null;
}
