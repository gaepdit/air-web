using AirWeb.Domain.NamedEntities.Offices;
using GaEpd.AppLibrary.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace AirWeb.Domain.Identity;

// Add profile data for application users by adding properties to the ApplicationUser class.
// (IdentityUser already includes ID, Email, UserName, and PhoneNumber properties.)
public sealed class ApplicationUser : IdentityUser, IEntity<string>
{
    /// <summary>
    /// A claim that specifies the given name of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname
    /// </summary>
    [ProtectedPersonalData]
    [StringLength(150)]
    public string GivenName { get; set; } = string.Empty;

    /// <summary>
    /// A claim that specifies the surname of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname
    /// </summary>
    [ProtectedPersonalData]
    [StringLength(150)]
    public string FamilyName { get; set; } = string.Empty;

    // Editable user/staff properties
    public const int MaxPhoneLength = 25;
    public Office? Office { get; set; }
    public bool Active { get; set; } = true;

    // Auditing properties
    public DateTimeOffset? AccountCreatedAt { get; init; }
    public DateTimeOffset? AccountUpdatedAt { get; set; }
    public DateTimeOffset? ProfileUpdatedAt { get; set; }
    public DateTimeOffset? MostRecentLogin { get; set; }

    // Display properties
    public string SortableFullName => new[] { FamilyName, GivenName }.ConcatWithSeparator(", ");
    public string FullName => new[] { GivenName, FamilyName }.ConcatWithSeparator();

    public string SortableNameWithInactive
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append(SortableFullName);
            if (!Active) sn.Append(" [Inactive]");
            return sn.ToString();
        }
    }
}
