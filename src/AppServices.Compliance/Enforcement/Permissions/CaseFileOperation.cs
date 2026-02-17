using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AirWeb.AppServices.Compliance.Enforcement.Permissions;

public class CaseFileOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private CaseFileOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<CaseFileOperation> AllOperations { get; } = [];

    public static readonly CaseFileOperation AddComment = new(nameof(AddComment));
    public static readonly CaseFileOperation CloseCaseFile = new(nameof(CloseCaseFile));
    public static readonly CaseFileOperation DeleteCaseFile = new(nameof(DeleteCaseFile));
    public static readonly CaseFileOperation DeleteComment = new(nameof(DeleteComment));
    public static readonly CaseFileOperation EditCaseFile = new(nameof(EditCaseFile));
    public static readonly CaseFileOperation ReopenCaseFile = new(nameof(ReopenCaseFile));
    public static readonly CaseFileOperation RestoreCaseFile = new(nameof(RestoreCaseFile));
    public static readonly CaseFileOperation View = new(nameof(View));
    public static readonly CaseFileOperation ViewDeleted = new(nameof(ViewDeleted));
    public static readonly CaseFileOperation ViewDraftEnforcement = new(nameof(ViewDraftEnforcement));
}
