USE AIRBRANCH
GO

CREATE OR ALTER VIEW air.IaipSourceTestSummary
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Summarizes source tests.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-09-11  DWaldron            Initial version (#355)
2025-09-25  DWaldron            Use the new source test compliance assignment columns in IAIP (#359)
2025-11-03  DWaldron            IAIP compliance assignment is saved as email instead of User ID (iaip#1334)

***************************************************************************************************/

select convert(int, r.STRREFERENCENUMBER)                      as ReferenceNumber,
       convert(int, r.STRREPORTTYPE)                           as ReportType,
       convert(int, r.STRDOCUMENTTYPE)                         as DocumentType,
       trim(r.STREMISSIONSOURCE)                               as Source,
       lp.STRPOLLUTANTDESCRIPTION                              as Pollutant,
       trim(r.STRAPPLICABLEREQUIREMENT)                        as ApplicableRequirement,
       s.STRCOMPLIANCESTATUS                                   as ComplianceStatus,
       convert(bit, r.STRCLOSED)                               as ReportClosed,
       convert(date, r.DATRECEIVEDDATE)                        as DateReceivedByApb,
       convert(date, nullif(r.DATCOMPLETEDATE, '1776-07-04')) as DateTestReviewComplete,
       r.ComplianceAssignment                                  as IaipComplianceAssignment,
       convert(bit, IIF(r.ComplianceReviewDate is null, 0, 1)) as IaipComplianceComplete,

       -- Facility Summary
       right(i.STRAIRSNUMBER, 8)                               as FacilityId,
       f.STRFACILITYNAME                                       as Name,
       lc.STRCOUNTYNAME                                        as County,
       trim(f.STRFACILITYCITY)                                 as City,
       f.STRFACILITYSTATE                                      as State,

       -- Test Dates
       convert(date, r.DATTESTDATESTART)                       as StartDate,
       convert(date, r.DATTESTDATEEND)                         as EndDate,

       -- Reviewed By Staff
       pr.STRFIRSTNAME                                         as GivenName,
       pr.STRLASTNAME                                          as FamilyName
from dbo.ISMPREPORTINFORMATION r
    left join dbo.LOOKUPPOLLUTANTS lp
        on r.STRPOLLUTANT = lp.STRPOLLUTANTCODE
    left join dbo.LOOKUPISMPCOMPLIANCESTATUS s
        on s.STRCOMPLIANCEKEY = r.STRCOMPLIANCESTATUS

    left join dbo.EPDUSERPROFILES pr
        on pr.NUMUSERID = r.STRREVIEWINGENGINEER

    inner join dbo.ISMPMASTER i
        on i.STRREFERENCENUMBER = r.STRREFERENCENUMBER
    inner join dbo.APBFACILITYINFORMATION f
        on f.STRAIRSNUMBER = i.STRAIRSNUMBER
    left join dbo.LOOKUPCOUNTYINFORMATION lc
        on substring(f.STRAIRSNUMBER, 5, 3) = lc.STRCOUNTYCODE

where r.STRDOCUMENTTYPE <> '001'
  and r.STRDELETE is null;

GO
