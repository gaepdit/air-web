USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetOpenSourceTestAssignments
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves a list of compliance staff assigned to stack tests that have not yet been reviewed.
            Source tests older than 2023 are not included.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2026-06-03  DWaldron            Initial version (#613)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select distinct u.Id         as [UserId],
                    u.GivenName  as [GivenName],
                    u.FamilyName as [FamilyName],
                    l.Id         as [OfficeId],
                    l.Name       as [OfficeName]
    from air.IaipSourceTestSummary a
        inner join AirWeb.dbo.AspNetUsers u
            on a.IaipComplianceAssignment = u.Email
        left join AirWeb.dbo.Lookups l
            on l.Id = u.OfficeId
    where a.ReportClosed = convert(bit, 1)
      and a.IaipComplianceComplete = convert(bit, 0)
      and a.DateTestReviewComplete > '2023-01-01'
      and a.IaipComplianceAssignment is not null
    order by FamilyName, GivenName;

END
GO
