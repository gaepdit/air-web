using System;
using System.Collections.Generic;
using System.Text;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Inspections;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Validators;

public class ComplianceWorkSearchValidatorTests
{
    private static readonly ComplianceWorkCommandValidator ComplianceWorkCommandValidator = new();

    private static readonly ComplianceWorkCreateValidator ComplianceWorkCreateValidator =
        new(ComplianceWorkCommandValidator);

}

