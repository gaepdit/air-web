USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestSummary
    @ReferenceNumber int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves summary information for a given stack test.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-09  DWaldron            Initial version

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(int, r.STRREFERENCENUMBER) as ReferenceNumber,
           convert(int, r.STRREPORTTYPE)      as ReportType,
           convert(int, r.STRDOCUMENTTYPE)    as DocumentType,
           trim(r.STREMISSIONSOURCE)          as Source,
           lp.STRPOLLUTANTDESCRIPTION         as Pollutant,
           trim(r.STRAPPLICABLEREQUIREMENT)   as ApplicableRequirement,
           convert(date, r.DATRECEIVEDDATE)   as DateReceivedByApb,
           r.STRCLOSED                        as ReportClosed,
           s.STRCOMPLIANCESTATEMENT           as ReportStatement,
           r.STRDIRECTOR                      as EpdDirector,

           -- Facility Summary
           right(i.STRAIRSNUMBER, 8)          as Id,
           f.STRFACILITYNAME                  as Name,
           lc.STRCOUNTYNAME                   as County,
           trim(f.STRFACILITYCITY)            as City,
           f.STRFACILITYSTATE                 as State,

           'TestDates'                        as Id,
           convert(date, r.DATTESTDATESTART)  as StartDate,
           convert(date, r.DATTESTDATEEND)    as EndDate,

           'ReviewedByStaff'                  as Id,
           pr.STRFIRSTNAME                    as GivenName,
           pr.STRLASTNAME                     as FamilyName
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
      and r.STRDELETE is null
      and r.STRREFERENCENUMBER = @ReferenceNumber;

END
GO
