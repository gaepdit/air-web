USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetOpenSourceTestsForCompliance
    @AssignmentEmail varchar(100) = null,
    @Skip int,
    @Take int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves a list of stack tests which have not yet been reviewed by compliance staff.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-09-11  DWaldron            Initial version (#355)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select ReferenceNumber,
           ReportType,
           DocumentType,
           Source,
           Pollutant,
           ApplicableRequirement,
           ComplianceStatus,
           ReportClosed,
           DateReceivedByApb,
           DateTestReviewComplete,
           IaipComplianceAssignment,

           -- Facility Summary
           FacilityId        as Id,
           Name,
           County,
           City,
           State,

           'TestDates'       as Id,
           StartDate,
           EndDate,

           'ReviewedByStaff' as Id,
           GivenName,
           FamilyName
    from air.IaipSourceTestSummary
    where ReportClosed = convert(bit, 1)
      and IaipComplianceComplete = convert(bit, 0)
      and DateTestReviewComplete > '2023-01-01'
      and (IaipComplianceAssignment = @AssignmentEmail or @AssignmentEmail is null)
    order by DateTestReviewComplete desc, ReferenceNumber desc
    offset @Skip rows fetch next @Take rows only;

    select count(*)
    from air.IaipSourceTestSummary
    where ReportClosed = convert(bit, 1)
      and IaipComplianceComplete = convert(bit, 0)
      and DateTestReviewComplete > '2023-01-01'
      and (IaipComplianceAssignment = @AssignmentEmail or @AssignmentEmail is null);
    
END
GO
