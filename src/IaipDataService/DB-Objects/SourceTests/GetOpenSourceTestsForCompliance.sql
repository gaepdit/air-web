USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetOpenSourceTestsForCompliance
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
    order by DateTestReviewComplete desc, ReferenceNumber desc;

END
GO
