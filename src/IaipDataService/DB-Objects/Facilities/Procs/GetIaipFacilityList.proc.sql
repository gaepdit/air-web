USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetIaipFacilityList
    @includePortableSources bit
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves list of active facilities including AIRS # and Name for use by the 
  Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-11-21  DWaldron            Initial version
2026-04-08  DWaldron            Refactored to include more information for each facility (#545)
2026-04-10  DWaldron            Add option to exclude portable sources (county code = "777") (#559)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select iaip_facility.FormatAirsNumber(f.STRAIRSNUMBER) as [Id],
           trim(f.STRFACILITYNAME)                         as [Name],
           f.STRFACILITYCITY                               as [City],
           f.STRFACILITYSTATE                              as [State],
           'GeoCoordinates'                                as [GeoCoordinatesId],
           f.NUMFACILITYLATITUDE                           as [Latitude],
           f.NUMFACILITYLONGITUDE                          as [Longitude]
    from dbo.APBFACILITYINFORMATION f
        inner join dbo.AFSFACILITYDATA a
            on f.STRAIRSNUMBER = a.STRAIRSNUMBER
    where a.STRUPDATESTATUS in ('A', 'C')
        and (@includePortableSources = 1 or left(f.STRAIRSNUMBER, 7) <> '0413777')
    order by f.STRAIRSNUMBER;

END;

GO
