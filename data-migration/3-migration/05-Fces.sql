use AirWeb
go

SET IDENTITY_INSERT AirWeb.dbo.Fces ON;

-- insert into AirWeb.dbo.Fces
-- (Id, FacilityId, Year, ReviewedById, CompletedDate, OnsiteInspection, Notes, DataExchangeStatus,
--  CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments)

select i.STRFCENUMBER                                         as Id,
       AIRBRANCH.air.FormatAirsNumber(i.STRAIRSNUMBER)
                                                              as FacilityId,
       d.STRFCEYEAR                                           as Year,
       ur.Id                                                  as ReviewedById,
       convert(date, d.DATFCECOMPLETED)                       as CompletedDate,
       convert(bit, d.STRSITEINSPECTION)                      as OnsiteInspection,
       IIF(d.STRFCECOMMENTS = 'N/A', null, d.STRFCECOMMENTS)  as Notes,
       i.ICIS_STATUSIND                                       as DataExchangeStatus,
       i.DATMODIFINGDATE at time zone 'Eastern Standard Time' as CreatedAt,
       uc.Id                                                  as CreatedById,
       d.DATMODIFINGDATE at time zone 'Eastern Standard Time' as UpdatedAt,
       um.Id                                                  as UpdatedById,
       isnull(i.IsDeleted, 0)                                 as IsDeleted,
       null                                                   as DeletedAt,
       null                                                   as DeletedById,
       null                                                   as DeleteComments
from AIRBRANCH.dbo.SSCPFCEMASTER i
    inner join AIRBRANCH.dbo.SSCPFCE d
        on i.STRFCENUMBER = d.STRFCENUMBER
    inner join AirWeb.dbo.AspNetUsers ur
        on ur.AirbranchUserId = d.STRREVIEWER
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.AirbranchUserId = i.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers um
        on um.AirbranchUserId = d.STRMODIFINGPERSON;

SET IDENTITY_INSERT AirWeb.dbo.Fces OFF;

select *
from AirWeb.dbo.Fces;
