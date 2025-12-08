-- IAIP roles to Air Web App roles:
--
-- | IAIP                    | Air Web                      |
-- |-------------------------|------------------------------|
-- | N'District Manager'     | N'EnforcementReviewer'       |
-- | N'District Manager'     | N'ComplianceManager'         |
-- | N'District Staff'       | N'ComplianceStaff'           |
-- | N'SSCP Administrative'  | N'AppUserAdmin'              |
-- | N'SSCP Administrative'  | N'ComplianceSiteMaintenance' |
-- | N'SSCP Engineer'        | N'ComplianceStaff'           |
-- | N'SSCP Program Manager' | N'EnforcementManager'        |
-- | N'SSCP Program Manager' | N'ComplianceManager'         |
-- | N'SSCP Program Manager' | N'AppUserAdmin'              |
-- | N'SSCP Program Manager' | N'ComplianceSiteMaintenance' |
-- | N'SSCP Unit Manager'    | N'EnforcementReviewer'       |
-- | N'SSCP Unit Manager'    | N'ComplianceManager'         |
-- | N'SSCP Unit Manager'    | N'ComplianceSiteMaintenance' |

-- Roles derived from these queries:
--
-- select Id, Name
-- from AirWeb.dbo.AspNetRoles
-- order by Name;
--
-- select distinct convert(int, l.value) as RoleId,
--                 a.STRACCOUNTDESC      as RoleName
-- from AIRBRANCH.dbo.EPDUSERPROFILES u
--     inner join AIRBRANCH.air.ComplianceUserIds c
--         on c.UserId = u.NUMUSERID
--     left join AIRBRANCH.dbo.IAIPPERMISSIONS p
--         on p.NUMUSERID = u.NUMUSERID
--     outer apply STRING_SPLIT(replace(trim('()' from p.STRIAIPPERMISSIONS), ')(', ','), ',') l
--     left join AIRBRANCH.dbo.LOOKUPIAIPACCOUNTS a
--         on a.NUMACCOUNTCODE = l.value
-- where u.NUMEMPLOYEESTATUS = 1
--   and a.Active = 1
-- --   and a.STRACCOUNTDESC in
-- --       (N'District Manager', N'District Staff', N'SSCP Administrative',
-- --        N'SSCP Engineer', N'SSCP Program Manager', N'SSCP Unit Manager')
-- order by RoleName;

with roleX(IaipRole, AirWebRole) as
         (select N'District Manager' as IaipRole, N'EnforcementReviewer' as AirWebRole
          union
          select N'District Manager', N'ComplianceManager'
          union
          select N'District Staff', N'ComplianceStaff'
          union
          select N'SSCP Administrative', N'AppUserAdmin'
          union
          select N'SSCP Administrative', N'ComplianceSiteMaintenance'
          union
          select N'SSCP Engineer', N'ComplianceStaff'
          union
          select N'SSCP Program Manager', N'EnforcementManager'
          union
          select N'SSCP Program Manager', N'ComplianceManager'
          union
          select N'SSCP Program Manager', N'AppUserAdmin'
          union
          select N'SSCP Program Manager', N'ComplianceSiteMaintenance'
          union
          select N'SSCP Unit Manager', N'EnforcementReviewer'
          union
          select N'SSCP Unit Manager', N'ComplianceManager'
          union
          select N'DMU Management', N'AppUserAdmin'
          union
          select N'DMU Management', N'ComplianceSiteMaintenance'
          union
          select N'DMU Management', N'GeneralStaff'
          union
          select N'DMU Management', N'SiteMaintenance'),

     iaipRoles(IaipUserId, Email, IaipRoleName) as
         (select u.NUMUSERID as IaipUserId,
                 u.STREMAILADDRESS as Email,
                 a.STRACCOUNTDESC as IaipRoleName
          from AIRBRANCH.dbo.EPDUSERPROFILES u
              inner join AIRBRANCH.air.ComplianceUserIds c
                  on c.IaipUserId = u.NUMUSERID
              left join AIRBRANCH.dbo.IAIPPERMISSIONS p
                  on p.NUMUSERID = u.NUMUSERID
              outer apply STRING_SPLIT(replace(trim('()' from p.STRIAIPPERMISSIONS), ')(', ','), ',') l
              left join AIRBRANCH.dbo.LOOKUPIAIPACCOUNTS a
                  on a.NUMACCOUNTCODE = l.value
          where u.NUMEMPLOYEESTATUS = 1
            and (a.Active = 1 or a.NUMACCOUNTCODE = 118)
            and convert(int, l.value) in
                (19, 20, 79, 80, 82, 84, 85, 86, 113, 114, 133, 134, 135, 136, 137, 138, 140, 118))

-- insert
-- into AirWeb.dbo.AspNetUserRoles (UserId, RoleId)

select distinct u.Id as UserId,
                r.Id as RoleId
--         ,
--                 u.Email,
--                 iaipRoles.IaipRoleName,
--                 r.Name
from AirWeb.dbo.AspNetUsers u
    inner join iaipRoles
        on iaipRoles.IaipUserId = u.IaipUserId
    inner join roleX
        on roleX.IaipRole = iaipRoles.IaipRoleName
    inner join AirWeb.dbo.AspNetRoles r
        on roleX.AirWebRole = r.Name;

select u.Email,
       r.Name
from AirWeb.dbo.AspNetUsers u
    inner join AirWeb.dbo.AspNetUserRoles ur
        on u.Id = ur.UserId
    inner join AirWeb.dbo.AspNetRoles r
        on r.Id = ur.RoleId
order by u.Email;
