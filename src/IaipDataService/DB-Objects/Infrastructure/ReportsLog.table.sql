if object_id(N'air.ReportsLog', N'U') is null
    begin

        create table air.ReportsLog (
            Id         uniqueidentifier default newid()
                constraint ReportsLog_pk primary key,
            Type       nvarchar(max) not null,
            Parameters nvarchar(max) not null,
            Date       datetimeoffset   default sysdatetimeoffset()
        );

    end
go
