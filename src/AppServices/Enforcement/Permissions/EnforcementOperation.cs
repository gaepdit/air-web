using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AirWeb.AppServices.Enforcement.Permissions;

public class EnforcementOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private EnforcementOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<EnforcementOperation> AllOperations { get; } = [];

    public static readonly EnforcementOperation AddComment = new(nameof(AddComment));
    public static readonly EnforcementOperation Close = new(nameof(Close));
    public static readonly EnforcementOperation Delete = new(nameof(Delete));
    public static readonly EnforcementOperation DeleteComment = new(nameof(DeleteComment));
    public static readonly EnforcementOperation Edit = new(nameof(Edit));
    public static readonly EnforcementOperation Reopen = new(nameof(Reopen));
    public static readonly EnforcementOperation Restore = new(nameof(Restore));
    public static readonly EnforcementOperation View = new(nameof(View));
    public static readonly EnforcementOperation ViewDeleted = new(nameof(ViewDeleted));
}
