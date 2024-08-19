﻿using Microsoft.AspNetCore.Authorization.Infrastructure;

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

    public static readonly ComplianceWorkOperation Edit = new(nameof(Edit));
    public static readonly ComplianceWorkOperation Comment = new(nameof(Comment));
    public static readonly ComplianceWorkOperation ManageDeletions = new(nameof(ManageDeletions));
}
