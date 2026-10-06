USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetIaipFceInspectionStatusReport
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves a report of most recent FCE, most recent Inspection, assignments, and other 
            data useful for inspection year planning

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2026-10-06  DWaldron            Initial version (#700)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select Id,
           Name,
           City,
           OperatingStatusCode,
           ClassificationCode,
           CmsClassificationCode,
           MostRecentFce,
           MostRecentInspectionDate,
           IaipComplianceAssignment,
           IaipAssignmentYear,
           ComplianceUnit,
           District,
           IsDistrictResponsible
    from air.IaipFceInspectionStatusReport
    order by Id;

END;

GO
