use AirWeb
go

SET IDENTITY_INSERT AirWeb.dbo.Fces ON;

-- insert into AirWeb.dbo.Fces
-- (Id, FacilityId, Year, ReviewedById, CompletedDate, OnsiteInspection, Notes, DataExchangeStatus,
--  CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments)

select f.STRFCENUMBER                                         as Id,
       AIRBRANCH.air.FormatAirsNumber(f.STRAIRSNUMBER)        as FacilityId,
       d.STRFCEYEAR                                           as Year,
       ur.Id                                                  as ReviewedById,
       convert(date, d.DATFCECOMPLETED)                       as CompletedDate,
       convert(bit, d.STRSITEINSPECTION)                      as OnsiteInspection,
       IIF(d.STRFCECOMMENTS = 'N/A', null, d.STRFCECOMMENTS)  as Notes,
       f.ICIS_STATUSIND                                       as DataExchangeStatus,
       f.DATMODIFINGDATE at time zone 'Eastern Standard Time' as CreatedAt,
       um.Id                                                  as CreatedById,
       f.DATMODIFINGDATE at time zone 'Eastern Standard Time' as UpdatedAt,
       um.Id                                                  as UpdatedById,
       f.IsDeleted                                            as IsDeleted,
       null                                                   as DeletedAt,
       null                                                   as DeletedById,
       null                                                   as DeleteComments
from AIRBRANCH.dbo.SSCPFCEMASTER f
    inner join AIRBRANCH.dbo.SSCPFCE d
        on f.STRFCENUMBER = d.STRFCENUMBER
    inner join AirWeb.dbo.AspNetUsers ur
        on ur.AirbranchUserId = d.STRREVIEWER
    inner join AirWeb.dbo.AspNetUsers um
        on um.AirbranchUserId = f.STRMODIFINGPERSON;

SET IDENTITY_INSERT AirWeb.dbo.Fces OFF;

select *
from AirWeb.dbo.Fces;
