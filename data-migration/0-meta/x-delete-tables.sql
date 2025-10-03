-- This script will delete all tables from [air-web].

use [air-web]
go

drop table [air-web].dbo.AspNetRoleClaims;
drop table [air-web].dbo.AspNetUserClaims;
drop table [air-web].dbo.AspNetUserLogins;
drop table [air-web].dbo.AspNetUserRoles;
drop table [air-web].dbo.AspNetRoles;
drop table [air-web].dbo.AspNetUserTokens;
drop table [air-web].dbo.AuditPoints;
drop table [air-web].dbo.CaseFileComplianceEvents;
drop table [air-web].dbo.Comments;
drop table [air-web].dbo.ComplianceWork;
drop table [air-web].dbo.EmailLogs;
drop table [air-web].dbo.EnforcementActionReviews;
drop table [air-web].dbo.Fces;
drop table [air-web].dbo.StipulatedPenalties;
drop table [air-web].dbo.EnforcementActions;
drop table [air-web].dbo.CaseFiles;
drop table [air-web].dbo.AspNetUsers;
drop table [air-web].dbo.Lookups;
drop table [air-web].dbo.ViolationTypes;
drop table [air-web].dbo.__EFMigrationsHistory;
