-- This script will delete all data from the [air-web] tables
-- (not including `__EFMigrationsHistory`).

use [air-web]
go

delete from dbo.AspNetRoleClaims;
delete from dbo.AspNetRoles;
delete from dbo.AspNetUserClaims;
delete from dbo.AspNetUserLogins;
delete from dbo.AspNetUserRoles;
delete from dbo.AspNetUsers;
delete from dbo.AspNetUserTokens;
delete from dbo.AuditPoints;
delete from dbo.CaseFileComplianceEvents;
delete from dbo.CaseFiles;
delete from dbo.Comments;
delete from dbo.EmailLogs;
delete from dbo.EnforcementActionReviews;
delete from dbo.EnforcementActions;
delete from dbo.Fces;
delete from dbo.Lookups;
delete from dbo.StipulatedPenalties;
delete from dbo.ViolationTypes;
delete from dbo.WorkEntries;
