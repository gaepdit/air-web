USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetFacilitySourceTests
    @FacilityId varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves a list of stack tests for a given facility.

Input Parameters:
    @FacilityId - The Facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-07  DWaldron            Initial version
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
           ReportClosed,
           DateReceivedByApb,
           DateTestReviewComplete,

           'TestDates'       as Id,
           StartDate,
           EndDate,

           'ReviewedByStaff' as Id,
           GivenName,
           FamilyName
    from air.IaipSourceTestSummary
    where FacilityId = @FacilityId
    order by StartDate desc, ReferenceNumber desc;

END
GO
