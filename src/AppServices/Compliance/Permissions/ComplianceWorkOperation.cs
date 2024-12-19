using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AirWeb.AppServices.Compliance.Permissions;

public class ComplianceWorkOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private ComplianceWorkOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<ComplianceWorkOperation> AllOperations { get; } = [];

    public static readonly ComplianceWorkOperation AddComment = new(nameof(AddComment));
    public static readonly ComplianceWorkOperation Close = new(nameof(Close));
    public static readonly ComplianceWorkOperation Delete = new(nameof(Delete));
    public static readonly ComplianceWorkOperation DeleteComment = new(nameof(DeleteComment));
    public static readonly ComplianceWorkOperation Edit = new(nameof(Edit));
    public static readonly ComplianceWorkOperation Reopen = new(nameof(Reopen));
    public static readonly ComplianceWorkOperation Restore = new(nameof(Restore));
    public static readonly ComplianceWorkOperation ViewDeleted = new(nameof(ViewDeleted));
}
