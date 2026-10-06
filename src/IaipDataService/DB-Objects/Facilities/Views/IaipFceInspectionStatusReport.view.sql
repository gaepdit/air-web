USE AIRBRANCH
GO

CREATE OR ALTER VIEW air.IaipFceInspectionStatusReport
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

with recentAssignments as
         (select STRAIRSNUMBER, max(INTYEAR) as INTYEAR
          from dbo.SSCPINSPECTIONSREQUIRED
          group by STRAIRSNUMBER),

     fces as
         (select FacilityId,
                 max(CompletedDate) as MostRecentFce
          from AirWeb.dbo.Fces
          where IsDeleted = 0
          group by FacilityId),

     inspections as
         (select FacilityId, max(InspectionStarted) as MostRecentInspectionDate
          from AirWeb.dbo.ComplianceWork
          where IsDeleted = 0
            and ComplianceWorkType = 'Inspection'
          group by FacilityId)

select right(fi.STRAIRSNUMBER, 8)                       as Id,
       trim(fi.STRFACILITYNAME)                         as Name,
       trim(fi.STRFACILITYCITY)                         as City,
       hd.STROPERATIONALSTATUS                          as OperatingStatusCode,
       hd.STRCLASS                                      as ClassificationCode,
       COALESCE(sd.STRCMSMEMBER, 'X')                   as CmsClassificationCode,
       fc.MostRecentFce                                 as MostRecentFce,
       convert(date, sp.MostRecentInspectionDate)       as MostRecentInspectionDate,
       concat_ws(', ', up.STRLASTNAME, up.STRFIRSTNAME) as IaipComplianceAssignment,
       ir.INTYEAR                                       as IaipAssignmentYear,
       lu.STRUNITDESC                                   as ComplianceUnit,
       ld.STRDISTRICTNAME                               as District,
       convert(bit, dr.STRDISTRICTRESPONSIBLE)          as IsDistrictResponsible

from dbo.APBFACILITYINFORMATION fi
    inner join dbo.APBHEADERDATA hd
        on fi.STRAIRSNUMBER = hd.STRAIRSNUMBER
    inner join dbo.APBSUPPLAMENTALDATA sd
        on fi.STRAIRSNUMBER = sd.STRAIRSNUMBER
    inner join dbo.AFSFACILITYDATA ad
        on fi.STRAIRSNUMBER = ad.STRAIRSNUMBER
    inner join dbo.LOOKUPDISTRICTINFORMATION as li
        on SUBSTRING(hd.STRAIRSNUMBER, 5, 3) = li.STRDISTRICTCOUNTY
    inner join dbo.LOOKUPDISTRICTS as ld
        on li.STRDISTRICTCODE = ld.STRDISTRICTCODE
    inner join dbo.SSCPDISTRICTRESPONSIBLE as dr
        on hd.STRAIRSNUMBER = dr.STRAIRSNUMBER
    left join fces fc
        on fc.FacilityId = iaip_facility.FormatAirsNumber(fi.STRAIRSNUMBER)
    left join inspections sp
        on sp.FacilityId = iaip_facility.FormatAirsNumber(fi.STRAIRSNUMBER)
    inner join recentAssignments ra
        on ra.STRAIRSNUMBER = fi.STRAIRSNUMBER
    inner join dbo.SSCPINSPECTIONSREQUIRED ir
        on ir.STRAIRSNUMBER = ra.STRAIRSNUMBER
        and ir.INTYEAR = ra.INTYEAR
    left join dbo.EPDUSERPROFILES up
        on up.NUMUSERID = ir.NUMSSCPENGINEER
        and up.NUMEMPLOYEESTATUS = 1
    inner join dbo.LOOKUPEPDUNITS lu
        on lu.NUMUNITCODE = ir.NUMSSCPUNIT
        and lu.Active = 1

where ad.STRUPDATESTATUS in ('A', 'C')
  and hd.STROPERATIONALSTATUS <> 'X'
  and up.NUMUSERID is not null;

GO
