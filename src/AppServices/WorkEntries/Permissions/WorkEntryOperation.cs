using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace MyApp.AppServices.WorkEntries.Permissions;

public class WorkEntryOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private WorkEntryOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<WorkEntryOperation> AllOperations { get; } = [];

    public static readonly WorkEntryOperation EditWorkEntry = new(nameof(EditWorkEntry));
    public static readonly WorkEntryOperation ManageDeletions = new(nameof(ManageDeletions));
    public static readonly WorkEntryOperation ViewDeletedActions = new(nameof(ViewDeletedActions));
}
