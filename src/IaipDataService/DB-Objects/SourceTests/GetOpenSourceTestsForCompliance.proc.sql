USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetOpenSourceTestsForCompliance
    @AssignmentUser   nvarchar(450) = null,
    @AssignmentOffice uniqueidentifier = null,
    @Skip             int,
    @Take             int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves a list of stack tests which have not yet been reviewed by compliance staff.
            Source tests older than 2023 are not included.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-09-11  DWaldron            Initial version (#355)
2026-02-19  DWaldron            Filter by date and compliance assignment (#439)
2026-05-28  DWaldron            Format the Facility ID (#625)
2026-06-03  DWaldron            Filter by Air Web User ID and Office ID (#613)

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
           iaip_facility.FormatAirsNumber(FacilityId) as Id,
           Name,
           County,
           City,
           State,

           'TestDates'                                as Id,
           StartDate,
           EndDate,

           'ReviewedByStaff'                          as Id,
           GivenName,
           FamilyName
    from air.IaipSourceTestSummary
    where ReportClosed = convert(bit, 1)
      and IaipComplianceComplete = convert(bit, 0)
      and DateTestReviewComplete > '2023-01-01'
      and (@AssignmentUser is null or AirWebUserId = @AssignmentUser)
      and (@AssignmentOffice is null or AirWebOfficeId = @AssignmentOffice)
    order by DateTestReviewComplete desc, ReferenceNumber desc
    offset @Skip rows fetch next @Take rows only;

    select count(*)
    from air.IaipSourceTestSummary
    where ReportClosed = convert(bit, 1)
      and IaipComplianceComplete = convert(bit, 0)
      and DateTestReviewComplete > '2023-01-01'
      and (@AssignmentUser is null or AirWebUserId = @AssignmentUser)
      and (@AssignmentOffice is null or AirWebOfficeId = @AssignmentOffice);

END
GO
