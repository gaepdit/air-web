USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetIaipAnnualFeesSummary
    @FacilityId varchar(8),
    @LowerYear int,
    @UpperYear int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves a summary of annual fees for a given facility for use by the Air Web IAIP data service.

Input Parameters:
    @FacilityId - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-08-28  DWaldron            Initial version

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select FeeYear, InvoicedAmount, PaidAmount, Balance, Status, StatusDescription
    from air.IaipAnnualFeesSummary
    where FacilityId = @FacilityId
      and FeeYear between @LowerYear and @UpperYear;

END;

GO
