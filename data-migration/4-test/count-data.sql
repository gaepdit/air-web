select 'FCE AIRBRANCH' as [table_name],
       count(*)        as [count]
from SSCPFCEMASTER i
    inner join SSCPFCE f
        on f.STRFCENUMBER = i.STRFCENUMBER
where i.IsDeleted = 'False' or
      i.IsDeleted is null
union
select 'FCE AirWeb' as [table_name],
       count(*)     as [count]
from AirWeb.dbo.Fces

union

select 'ACC AIRBRANCH' as [table_name],
       count(*)        as [count]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPACCS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '04'
union
select 'ACC AirWeb' as [table_name],
       count(*)     as [count]
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'AnnualComplianceCertification'

union

select 'Inspection AIRBRANCH' as [table_name],
       count(*)               as [count]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPINSPECTIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '02'
union
select 'Inspection AirWeb' as [table_name],
       count(*)            as [count]
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'Inspection'

union

select 'RMP Inspection AIRBRANCH' as [table_name],
       count(*)                   as [count]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPINSPECTIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '07'
union
select 'RMP Inspection AirWeb' as [table_name],
       count(*)                as [count]
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'RmpInspection'

union

select 'Notification AIRBRANCH' as [table_name],
       count(*)                 as [count]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPNOTIFICATIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
        and (d.STRNOTIFICATIONTYPE not in ('03', '04', '05')
            or d.STRNOTIFICATIONTYPE is null)
where i.STRDELETE is null
  and i.STREVENTTYPE = '05'
union
select 'Notification AirWeb' as [table_name],
       count(*)              as [count]
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'Notification'

union

select 'PermitRevocation AIRBRANCH' as [table_name],
       count(*)                     as [count]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    inner join AIRBRANCH.dbo.SSCPNOTIFICATIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '05'
  and d.STRNOTIFICATIONTYPE = '03'
union
select 'PermitRevocation AirWeb' as [table_name],
       count(*)                  as [count]
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'PermitRevocation'

union

select 'Report AIRBRANCH' as [table_name],
       count(*)           as [count]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPTESTREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '01'
union
select 'Report AirWeb' as [table_name],
       count(*)        as [count]
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'Report'

union

select 'SourceTestReview AIRBRANCH' as [table_name],
       count(*)                     as [count]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPTESTREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '03'
union
select 'SourceTestReview AirWeb' as [table_name],
       count(*)                  as [count]
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'SourceTestReview'

union

select 'Case File AIRBRANCH' as [table_name],
       count(*)              as [count]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
union
select 'Case File AirWeb' as [table_name],
       count(*)           as [count]
from AirWeb.dbo.CaseFiles

union

select 'AdministrativeOrder AIRBRANCH' as [table_name],
       count(*)                        as [count]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and e.STRAOEXECUTED = 'True'
union
select 'AdministrativeOrder AirWeb' as [table_name],
       count(*)                     as [count]
from AirWeb.dbo.EnforcementActions
where ActionType = 'AdministrativeOrder'

union

select 'ConsentOrder AIRBRANCH' as [table_name],
       count(*)                 as [count]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRCORECEIVEDFROMCOMPANY = 'True' or e.STRCORECEIVEDFROMDIRECTOR = 'True' or e.STRCOEXECUTED = 'True')
union
select 'ConsentOrder AirWeb' as [table_name],
       count(*)              as [count]
from AirWeb.dbo.EnforcementActions
where ActionType = 'ConsentOrder'

union

select 'LetterOfNoncompliance AIRBRANCH' as [table_name],
       count(*)                          as [count]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'LON'
  and (e.STRLONTOUC = 'True' or e.STRLONSENT = 'True' or e.STRLONCOMMENTS is not null)
union
select 'LetterOfNoncompliance AirWeb' as [table_name],
       count(*)                       as [count]
from AirWeb.dbo.EnforcementActions
where ActionType = 'LetterOfNoncompliance'

union

select 'NoFurtherActionLetter AIRBRANCH' as [table_name],
       count(*)                          as [count]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRNFATOUC = 'True' or e.STRNFATOPM = 'True' or e.STRNFALETTERSENT = 'True')
  and (e.DATNOVSENT is null or convert(date, e.DATNOVSENT) <> convert(date, e.DATNFALETTERSENT))
union
select 'NoFurtherActionLetter AirWeb' as [table_name],
       count(*)                       as [count]
from AirWeb.dbo.EnforcementActions
where ActionType = 'NoFurtherActionLetter'

union

select 'NoticeOfViolation AIRBRANCH' as [table_name],
       count(*)                      as [count]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRNOVTOUC = 'True' or e.STRNOVTOPM = 'True' or e.STRNOVSENT = 'True')
  and (e.DATNFALETTERSENT is null or convert(date, e.DATNOVSENT) <> convert(date, e.DATNFALETTERSENT))
union
select 'NoticeOfViolation AirWeb' as [table_name],
       count(*)                   as [count]
from AirWeb.dbo.EnforcementActions
where ActionType = 'NoticeOfViolation'

union

select 'NovNfaLetter AIRBRANCH' as [table_name],
       count(*)                 as [count]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and convert(date, e.DATNOVSENT) = convert(date, e.DATNFALETTERSENT)
union
select 'NovNfaLetter AirWeb' as [table_name],
       count(*)              as [count]
from AirWeb.dbo.EnforcementActions
where ActionType = 'NovNfaLetter'

union

select 'ProposedConsentOrder AIRBRANCH' as [table_name],
       count(*)                         as [count]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRCOTOUC = 'True' or e.STRCOTOPM = 'True' or e.STRCOPROPOSED = 'True')
union
select 'ProposedConsentOrder AirWeb' as [table_name],
       count(*)                      as [count]
from AirWeb.dbo.EnforcementActions
where ActionType = 'ProposedConsentOrder'

union

select 'Stipulated penalties AIRBRANCH' as [table_name],
       count(*)                         as [count]
from AIRBRANCH.dbo.SSCPENFORCEMENTSTIPULATED
where STRSTIPULATEDPENALTY <> '0'
union
select 'Stipulated penalties AirWeb' as [table_name],
       count(*)                      as [count]
from AirWeb.dbo.StipulatedPenalties

union

select 'CaseFileComplianceEvents AIRBRANCH' as [table_name],
       count(*)                             as [count]
from AIRBRANCH.dbo.SSCP_EnforcementEvents c
    inner join AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
        on e.STRENFORCEMENTNUMBER = c.EnforcementNumber
    inner join AIRBRANCH.dbo.SSCPITEMMASTER i
        on i.STRTRACKINGNUMBER = c.TrackingNumber
where isnull(e.IsDeleted, 0) = 0
  and i.STRDELETE is null
union
select 'CaseFileComplianceEvents AirWeb' as [table_name],
       count(*)                          as [count]
from AirWeb.dbo.CaseFileComplianceEvents
