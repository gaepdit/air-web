using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AirWeb.AppServices.Compliance.Permissions;

public class ComplianceOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private ComplianceOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<ComplianceOperation> AllOperations { get; } = [];

    public static readonly ComplianceOperation AddComment = new(nameof(AddComment));
    public static readonly ComplianceOperation Close = new(nameof(Close));
    public static readonly ComplianceOperation Delete = new(nameof(Delete));
    public static readonly ComplianceOperation DeleteComment = new(nameof(DeleteComment));
    public static readonly ComplianceOperation Edit = new(nameof(Edit));
    public static readonly ComplianceOperation Reopen = new(nameof(Reopen));
    public static readonly ComplianceOperation Restore = new(nameof(Restore));
    public static readonly ComplianceOperation ViewDeleted = new(nameof(ViewDeleted));
}
