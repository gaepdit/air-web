insert into AirWeb.dbo.CaseFileComplianceEvents (CaseFileId, ComplianceEventId)

select c.EnforcementNumber as CaseFilesId,
       c.TrackingNumber    as ComplianceEventsId
from AIRBRANCH.dbo.SSCP_EnforcementEvents c
    inner join AirWeb.dbo.CaseFiles ae
        on ae.Id = c.EnforcementNumber
    inner join AirWeb.dbo.ComplianceWork ac
        on ac.Id = c.TrackingNumber;

select *
from AirWeb.dbo.CaseFileComplianceEvents;
