-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, FacilityId, Notes, Status, IssueDate, IsReportableAction,
-- 
--     -- AdministrativeOrder, ConsentOrder, LetterOfNoncompliance
--     ResolvedDate,
-- 
--     -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
--     ResponseRequested, ResponseReceived, ResponseComment,
-- 
--     -- EnforcementAction (All)
--     CreatedAt, UpdatedAt, UpdatedById, IsDeleted)

select newid()                                                   as Id,
       e.STRENFORCEMENTNUMBER                                    as CaseFileId,
       'LetterOfNoncompliance'                                   as ActionType,
       AIRBRANCH.iaip_facility.FormatAirsNumber(e.STRAIRSNUMBER) as FacilityId,

       nullif(concat_ws(CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
                        iif(e.DATLONTOUC is null, null, 'Date LON to UC: ' + format(e.DATLONTOUC, 'dd-MMM-yyyy')),
                        AIRBRANCH.air.ReduceText(e.STRLONCOMMENTS)),
              '')                                                as Notes,
       iif(e.STRLONSENT = 'True', 'Issued', 'Draft')             as Status,
       convert(date, e.DATLONSENT)                               as IssueDate,
       0                                                         as IsReportableAction,

       convert(date, e.DATLONRESOLVED)                           as ResolvedDate,
       0                                                         as ResponseRequested,
       null                                                      as ResponseReceived,
       null                                                      as ResponseComment,

       e.DATLONTOUC at time zone 'Eastern Standard Time'         as CreateAt,
       e.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as UpdatedAt,
       um.Id                                                     as UpdatedById,
       isnull(e.IsDeleted, 0)                                    as IsDeleted

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'LON'
  and (e.STRLONTOUC = 'True' or e.STRLONSENT = 'True' or e.STRLONCOMMENTS is not null)

order by e.STRENFORCEMENTNUMBER;

select *
from AirWeb.dbo.EnforcementActions
where ActionType = 'LetterOfNoncompliance';
