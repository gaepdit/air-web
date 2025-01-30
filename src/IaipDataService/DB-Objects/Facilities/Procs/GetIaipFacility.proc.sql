USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetIaipFacility
    @FacilityId varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves facility summary info for a given ID for use by the Air Web IAIP data service.

Input Parameters:
    @FacilityId - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-12-16  DWaldron            Initial version (based on `air.GetIaipFacilityDetails`)

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

END;

GO
