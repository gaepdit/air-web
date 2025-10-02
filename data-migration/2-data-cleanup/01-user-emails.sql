use [AIRBRANCH]
go

-- Stats on malformed email addresses
select distinct substring(STREMAILADDRESS, charindex('@', STREMAILADDRESS), len(STREMAILADDRESS)) as domain
from AIRBRANCH.dbo.EPDUSERPROFILES;

select *
from AIRBRANCH.dbo.EPDUSERPROFILES
where substring(STREMAILADDRESS, charindex('@', STREMAILADDRESS), len(STREMAILADDRESS))
          not in (N'@dnr.ga.gov', N'@dnr.state.ga.us');

-- Update malformed email addresses
update dbo.EPDUSERPROFILES
set STREMAILADDRESS = replace(STREMAILADDRESS, '@dne.state.ga.us', '@dnr.ga.gov')

update dbo.EPDUSERPROFILES
set STREMAILADDRESS = replace(STREMAILADDRESS, '@dnr.state.ga.u', '@dnr.state.ga.us')
where STREMAILADDRESS like '%@dnr.state.ga.u'

update dbo.EPDUSERPROFILES
set STREMAILADDRESS = null
where STREMAILADDRESS like N'%@no.email'
   or STREMAILADDRESS like N'%@yahoo.com'
   or STREMAILADDRESS in (N'Sam.Stevens', N'Tom.Atkinson', N'douglas.waldron@gaepd.org');
