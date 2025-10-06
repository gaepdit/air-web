-- This script will delete all tables from AirWeb.

use AirWeb
go

drop table AirWeb.dbo.AspNetRoleClaims;
drop table AirWeb.dbo.AspNetUserClaims;
drop table AirWeb.dbo.AspNetUserLogins;
drop table AirWeb.dbo.AspNetUserRoles;
drop table AirWeb.dbo.AspNetRoles;
drop table AirWeb.dbo.AspNetUserTokens;
drop table AirWeb.dbo.AuditPoints;
drop table AirWeb.dbo.CaseFileComplianceEvents;
drop table AirWeb.dbo.Comments;
drop table AirWeb.dbo.ComplianceWork;
drop table AirWeb.dbo.EmailLogs;
drop table AirWeb.dbo.EnforcementActionReviews;
drop table AirWeb.dbo.Fces;
drop table AirWeb.dbo.StipulatedPenalties;
drop table AirWeb.dbo.EnforcementActions;
drop table AirWeb.dbo.CaseFiles;
drop table AirWeb.dbo.AspNetUsers;
drop table AirWeb.dbo.Lookups;
drop table AirWeb.dbo.ViolationTypes;
drop table AirWeb.dbo.__EFMigrationsHistory;
