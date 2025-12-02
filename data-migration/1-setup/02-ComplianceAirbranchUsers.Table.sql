use [AIRBRANCH]
go

create table air.ComplianceUserIds
(
    UserId int not null
        constraint ComplianceUserIds_pk primary key
)
go

-- insert into AIRBRANCH.air.ComplianceUserIds (UserId)
select UserId
from (select STRMODIFINGPERSON as UserId
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
where t.UserId is not null;
