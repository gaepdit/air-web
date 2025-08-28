USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER VIEW air.IaipAnnualFeesSummary
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Summarizes annual fees data.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-04  DWaldron            Initial version (#320)

***************************************************************************************************/

select right(a.STRAIRSNUMBER, 8)                                    as FacilityId,
       convert(int, a.NUMFEEYEAR)                                   as FeeYear,
       convert(decimal(10, 2), inv.InvoicedAmount)                  as InvoicedAmount,
       convert(decimal(10, 2), trx.AmountPaid)                      as PaidAmount,
       convert(decimal(10, 2), inv.InvoicedAmount - trx.AmountPaid) as Balance,
       convert(int, a.NUMCURRENTSTATUS)                             as Status,
       ls.STRIAIPDESC                                               as StatusDescription
from dbo.FS_ADMIN as a
    left join(select STRAIRSNUMBER,
                     NUMFEEYEAR,
                     sum(NUMAMOUNT) as InvoicedAmount
              from dbo.FS_FEEINVOICE
              where ACTIVE = '1'
              group by STRAIRSNUMBER, NUMFEEYEAR) as inv
        on a.STRAIRSNUMBER = inv.STRAIRSNUMBER
        and a.NUMFEEYEAR = inv.NUMFEEYEAR
    left join(select STRAIRSNUMBER,
                     NUMFEEYEAR,
                     sum(NUMPAYMENT) as AmountPaid
              from dbo.FS_TRANSACTIONS
              where ACTIVE = '1'
              group by STRAIRSNUMBER, NUMFEEYEAR) as trx
        on a.STRAIRSNUMBER = trx.STRAIRSNUMBER
        and a.NUMFEEYEAR = trx.NUMFEEYEAR
    left join dbo.FSLK_ADMIN_STATUS as ls
        on a.NUMCURRENTSTATUS = ls.ID
where a.ACTIVE = '1';

GO
