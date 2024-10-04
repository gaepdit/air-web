USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetIaipFacility
    @FacilityId varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves facility info for a given ID.

Input Parameters:
    @FacilityId - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2022-02-22  DWaldron            Initial version
2022-12-08  DWaldron            Update filter for approved facilities (IAIP-1177)
2024-10-04  DWaldron            Change Facility ID to 8 characters and move data to Views (#162)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select Id,
           Name,
           Description,
           County,
           FacilityAddressId,
           Street,
           Street2,
           City,
           State,
           PostalCode,
           GeoCoordinatesId,
           Latitude,
           Longitude,
           RegulatoryDataId,
           OperatingStatusCode,
           StartupDate,
           PermitRevocationDate,
           ClassificationCode,
           CmsClassificationCode,
           OwnershipType,
           Sic,
           Naics,
           RmpId,
           OneHourOzoneNonattainment,
           EightHourOzoneNonattainment,
           PmFineNonattainment,
           NspsFeeExempt
    from air.IaipFacilityData
    where Id = @FacilityId;

    select AirProgram
    from air.IaipFacilityAirProgramData
    where FacilityId = @FacilityId
    order by Sequence;

    select AirProgram
    from air.IaipFacilityProgramClassificationData
    where FacilityId = @FacilityId
    order by Sequence;

END;

GO
