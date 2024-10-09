USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetFacilitySourceTests
    @FacilityId varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves a list of stack tests for a given facility.

Input Parameters:
    @FacilityId - The Facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-07  DWaldron            Initial version

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(int, r.STRREFERENCENUMBER) as ReferenceNumber,
           convert(int, r.STRDOCUMENTTYPE)   as DocumentType,
           lp.STRPOLLUTANTDESCRIPTION         as Pollutant,
           trim(r.STREMISSIONSOURCE)          as Source,
           convert(int, r.STRREPORTTYPE)      as ReportType,
           trim(r.STRAPPLICABLEREQUIREMENT)   as ApplicableRequirement,
           r.STRCLOSED                       as ReportClosed,
           convert(date, r.DATRECEIVEDDATE)  as DateReceivedByApb,
           s.STRCOMPLIANCESTATEMENT           as ReportStatement,
           r.STRDIRECTOR                      as EpdDirector,

           'TestDates'                        as Id,
           convert(date, r.DATTESTDATESTART) as StartDate,
           convert(date, r.DATTESTDATEEND)   as EndDate,

           'ReviewedByStaff'                  as Id,
           pr.STRFIRSTNAME                    as GivenName,
           pr.STRLASTNAME                     as FamilyName,

           'TestingUnitManager'               as Id,
           pt.STRFIRSTNAME                    as GivenName,
           pt.STRLASTNAME                     as FamilyName
    from dbo.ISMPREPORTINFORMATION r
        left join dbo.LOOKUPPOLLUTANTS lp
        on r.STRPOLLUTANT = lp.STRPOLLUTANTCODE
        left join dbo.LOOKUPISMPCOMPLIANCESTATUS s
        on s.STRCOMPLIANCEKEY = r.STRCOMPLIANCESTATUS

        left join dbo.EPDUSERPROFILES pr
        on pr.NUMUSERID = r.STRREVIEWINGENGINEER
        left join dbo.EPDUSERPROFILES pt
        on pt.NUMUSERID = r.NUMREVIEWINGMANAGER

        inner join dbo.ISMPMASTER i
        on i.STRREFERENCENUMBER = r.STRREFERENCENUMBER
    where r.STRDOCUMENTTYPE <> '001'
      and r.STRDELETE is null
      and i.STRAIRSNUMBER = concat('0413', @FacilityId)
    order by r.DATTESTDATESTART desc, convert(int, r.STRREFERENCENUMBER) desc;

END
GO
