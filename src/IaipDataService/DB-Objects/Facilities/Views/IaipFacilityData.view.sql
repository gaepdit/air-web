USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER VIEW air.IaipFacilityData
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Consolidates Facility info from several tables and formats it to be compatible with Dapper.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-04  DWaldron            Initial version (#162)

***************************************************************************************************/

select right(f.STRAIRSNUMBER, 8)     as Id,
       trim(f.STRFACILITYNAME)       as Name,
       trim(char(13) + char(10) + ' ' from h.STRPLANTDESCRIPTION)
                                     as Description,
       lc.STRCOUNTYNAME              as County,
       'FacilityAddress'             as FacilityAddressId,
       dbo.NullIfNaOrEmpty(f.STRFACILITYSTREET1)
                                     as Street,
       dbo.NullIfNaOrEmpty(f.STRFACILITYSTREET2)
                                     as Street2,
       trim(f.STRFACILITYCITY)       as City,
       f.STRFACILITYSTATE            as State,
       f.STRFACILITYZIPCODE          as PostalCode,
       'GeoCoordinates'              as GeoCoordinatesId,
       f.NUMFACILITYLATITUDE         as Latitude,
       f.NUMFACILITYLONGITUDE        as Longitude,
       'RegulatoryData'              as RegulatoryDataId,
       h.STROPERATIONALSTATUS        as OperatingStatusCode,
       h.DATSTARTUPDATE              as StartupDate,
       h.DATSHUTDOWNDATE             as PermitRevocationDate,
       h.STRCLASS                    as ClassificationCode,
       COALESCE(s.STRCMSMEMBER, 'X') as CmsClassificationCode,
       IIF(s.FacilityOwnershipTypeCode = 'FDF', 'Federal Facility (U.S. Government)', '')
                                     as OwnershipType,
       h.STRSICCODE                  as Sic,
       h.STRNAICSCODE                as Naics,
       s.STRRMPID                    as RmpId,
       convert(int, substring(coalesce(h.STRATTAINMENTSTATUS, '00000'), 2, 1))
                                     as OneHourOzoneNonattainment,
       convert(int, substring(coalesce(h.STRATTAINMENTSTATUS, '00000'), 3, 1))
                                     as EightHourOzoneNonattainment,
       convert(int, substring(coalesce(h.STRATTAINMENTSTATUS, '00000'), 4, 1))
                                     as PmFineNonattainment,
       s.NspsFeeExempt
from dbo.APBFACILITYINFORMATION f
    inner join dbo.APBHEADERDATA h
        on f.STRAIRSNUMBER = h.STRAIRSNUMBER
    inner join dbo.APBSUPPLAMENTALDATA s
        on f.STRAIRSNUMBER = s.STRAIRSNUMBER
    inner join dbo.AFSFACILITYDATA a
        on f.STRAIRSNUMBER = a.STRAIRSNUMBER
    left join dbo.LOOKUPCOUNTYINFORMATION lc
        on substring(f.STRAIRSNUMBER, 5, 3) = lc.STRCOUNTYCODE
where STRUPDATESTATUS in ('A', 'C');

GO
