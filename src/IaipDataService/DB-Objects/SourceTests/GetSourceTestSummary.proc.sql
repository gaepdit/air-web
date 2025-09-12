USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestSummary
    @ReferenceNumber int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves summary information for a given stack test.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-09  DWaldron            Initial version
2025-09-11  DWaldron            Use a common view instead (#355)

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
    where ReferenceNumber = @ReferenceNumber;

END
GO
