select 'FCE'                  as [category],
       count(*)               as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.Fces) as [AirWeb]
from SSCPFCEMASTER i
    inner join SSCPFCE f
        on f.STRFCENUMBER = i.STRFCENUMBER
where i.IsDeleted = 'False' or
      i.IsDeleted is null

union

select 'ACC'                                                   as [category],
       count(*)                                                as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.ComplianceWork
        where WorkEntryType = 'AnnualComplianceCertification') as [AirWeb]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPACCS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '04'

union

select 'Inspection'                         as [category],
       count(*)                             as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.ComplianceWork
        where WorkEntryType = 'Inspection') as [AirWeb]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPINSPECTIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '02'

union

select 'RMP Inspection'                        as [category],
       count(*)                                as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.ComplianceWork
        where WorkEntryType = 'RmpInspection') as [AirWeb]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPINSPECTIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '07'

union

select 'Notification'                         as [category],
       count(*)                               as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.ComplianceWork
        where WorkEntryType = 'Notification') as [AirWeb]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPNOTIFICATIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
        and (d.STRNOTIFICATIONTYPE not in ('03', '04', '05')
            or d.STRNOTIFICATIONTYPE is null)
where i.STRDELETE is null
  and i.STREVENTTYPE = '05'

union

select 'PermitRevocation'                         as [category],
       count(*)                                   as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.ComplianceWork
        where WorkEntryType = 'PermitRevocation') as [AirWeb]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    inner join AIRBRANCH.dbo.SSCPNOTIFICATIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '05'
  and d.STRNOTIFICATIONTYPE = '03'

union

select 'Report'                         as [category],
       count(*)                         as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.ComplianceWork
        where WorkEntryType = 'Report') as [AirWeb]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPTESTREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '01'

union

select 'SourceTestReview'                         as [category],
       count(*)                                   as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.ComplianceWork
        where WorkEntryType = 'SourceTestReview') as [AirWeb]
from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPTESTREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
where i.STRDELETE is null
  and i.STREVENTTYPE = '03'

union

select 'Case File'                 as [category],
       count(*)                    as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.CaseFiles) as [AirWeb]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
union

select 'AdministrativeOrder'                      as [category],
       count(*)                                   as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.EnforcementActions
        where ActionType = 'AdministrativeOrder') as [AirWeb]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and e.STRAOEXECUTED = 'True'

union

select 'ConsentOrder'                      as [category],
       count(*)                            as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.EnforcementActions
        where ActionType = 'ConsentOrder') as [AirWeb]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRCORECEIVEDFROMCOMPANY = 'True' or e.STRCORECEIVEDFROMDIRECTOR = 'True' or e.STRCOEXECUTED = 'True')

union

select 'LetterOfNoncompliance'                      as [category],
       count(*)                                     as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.EnforcementActions
        where ActionType = 'LetterOfNoncompliance') as [AirWeb]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'LON'
  and (e.STRLONTOUC = 'True' or e.STRLONSENT = 'True' or e.STRLONCOMMENTS is not null)

union

select 'NoFurtherActionLetter'                      as [category],
       count(*)                                     as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.EnforcementActions
        where ActionType = 'NoFurtherActionLetter') as [AirWeb]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRNFATOUC = 'True' or e.STRNFATOPM = 'True' or e.STRNFALETTERSENT = 'True')
  and (e.DATNOVSENT is null or convert(date, e.DATNOVSENT) <> convert(date, e.DATNFALETTERSENT))

union

select 'NoticeOfViolation'                      as [category],
       count(*)                                 as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.EnforcementActions
        where ActionType = 'NoticeOfViolation') as [AirWeb]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRNOVTOUC = 'True' or e.STRNOVTOPM = 'True' or e.STRNOVSENT = 'True')
  and (e.DATNFALETTERSENT is null or convert(date, e.DATNOVSENT) <> convert(date, e.DATNFALETTERSENT))

union

select 'NovNfaLetter'                      as [category],
       count(*)                            as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.EnforcementActions
        where ActionType = 'NovNfaLetter') as [AirWeb]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and convert(date, e.DATNOVSENT) = convert(date, e.DATNFALETTERSENT)

union

select 'ProposedConsentOrder'                      as [category],
       count(*)                                    as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.EnforcementActions
        where ActionType = 'ProposedConsentOrder') as [AirWeb]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRCOTOUC = 'True' or e.STRCOTOPM = 'True' or e.STRCOPROPOSED = 'True')

union

select 'Stipulated penalties'                as [category],
       count(*)                              as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.StipulatedPenalties) as [AirWeb]
from AIRBRANCH.dbo.SSCPENFORCEMENTSTIPULATED
where STRSTIPULATEDPENALTY <> '0'

union

select 'CaseFileComplianceEvents'                 as [category],
       count(*)                                   as [AIRBRANCH],
       (select count(*)
        from AirWeb.dbo.CaseFileComplianceEvents) as [AirWeb]
from AIRBRANCH.dbo.SSCP_EnforcementEvents c
    inner join AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
        on e.STRENFORCEMENTNUMBER = c.EnforcementNumber
    inner join AIRBRANCH.dbo.SSCPITEMMASTER i
        on i.STRTRACKINGNUMBER = c.TrackingNumber
where isnull(e.IsDeleted, 0) = 0
  and i.STRDELETE is null
