insert into AIRBRANCH.air.ComplianceUserIds (IaipUserId)

select IaipUserId
from (select STRMODIFINGPERSON as IaipUserId
      from AIRBRANCH.dbo.SSCPACCS
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCPENFORCEMENTSTIPULATED
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCPFCEMASTER
      union
      select STRREVIEWER
      from AIRBRANCH.dbo.SSCPFCE
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCPFCE
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCPINSPECTIONS
      union
      select STRRESPONSIBLESTAFF
      from AIRBRANCH.dbo.SSCPITEMMASTER
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCPITEMMASTER
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCPNOTIFICATIONS
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCPREPORTS
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCPTESTREPORTS
      union
      select STRMODIFINGPERSON
      from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT
      union
      select NUMSTAFFRESPONSIBLE
      from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT
      union
      select CreatedBy
      from AIRBRANCH.dbo.SSCP_EnforcementEvents) t
where t.IaipUserId is not null;

select *
from AIRBRANCH.air.ComplianceUserIds;
