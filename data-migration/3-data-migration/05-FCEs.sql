SET IDENTITY_INSERT AirWeb.dbo.Fces ON;

insert into AirWeb.dbo.Fces
(Id, FacilityId, Year, ReviewedById, CompletedDate, OnsiteInspection, Notes, ActionNumber, DataExchangeStatus,
 DataExchangeStatusDate, CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted)

select i.STRFCENUMBER                                            as Id,
       AIRBRANCH.iaip_facility.FormatAirsNumber(i.STRAIRSNUMBER) as FacilityId,
       d.STRFCEYEAR                                              as Year,
       ur.Id                                                     as ReviewedById,
       convert(date, d.DATFCECOMPLETED)                          as CompletedDate,
       convert(bit, d.STRSITEINSPECTION)                         as OnsiteInspection,
       AIRBRANCH.air.ReduceText(d.STRFCECOMMENTS)                as Notes,

       convert(int, f.STRAFSACTIONNUMBER)                        as ActionNumber,
       iif(f.STRAFSACTIONNUMBER is null, 'N', i.ICIS_STATUSIND)  as DataExchangeStatus,
       null                                                      as DataExchangeStatusDate,

       i.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as CreatedAt,
       uc.Id                                                     as CreatedById,
       d.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as UpdatedAt,
       um.Id                                                     as UpdatedById,
       0                                                         as IsDeleted
from AIRBRANCH.dbo.SSCPFCEMASTER i
    inner join AIRBRANCH.dbo.SSCPFCE d
        on i.STRFCENUMBER = d.STRFCENUMBER
    left join AIRBRANCH.dbo.AFSSSCPFCERECORDS f
        on f.STRFCENUMBER = i.STRFCENUMBER
    inner join AirWeb.dbo.AspNetUsers ur
        on ur.IaipUserId = d.STRREVIEWER
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.IaipUserId = i.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = d.STRMODIFINGPERSON

where i.IsDeleted = 'False'
   or i.IsDeleted is null

order by i.STRFCENUMBER;

SET IDENTITY_INSERT AirWeb.dbo.Fces OFF;

select *
from AirWeb.dbo.Fces;
