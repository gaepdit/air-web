USE [AIRBRANCH]
GO

create table air.ComplianceUserIds
(
    IaipUserId int not null
        constraint ComplianceUserIds_pk primary key
)
go
