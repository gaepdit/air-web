-- Stats on malformed email addresses
select substring(STREMAILADDRESS, charindex('@', STREMAILADDRESS), len(STREMAILADDRESS)) as domain, count(*)
from AIRBRANCH.dbo.EPDUSERPROFILES
group by substring(STREMAILADDRESS, charindex('@', STREMAILADDRESS), len(STREMAILADDRESS));

select *
from AIRBRANCH.dbo.EPDUSERPROFILES
where substring(STREMAILADDRESS, charindex('@', STREMAILADDRESS), len(STREMAILADDRESS))
          not in ('@dnr.ga.gov', '@dnr.state.ga.us');

-- Update malformed email addresses
update dbo.EPDUSERPROFILES
set STREMAILADDRESS = replace(STREMAILADDRESS, '@dne.state.ga.us', '@dnr.state.ga.us')
where STREMAILADDRESS like '%@dne.state.ga.us';

update dbo.EPDUSERPROFILES
set STREMAILADDRESS = replace(STREMAILADDRESS, '@dnr.state.ga.u', '@dnr.state.ga.us')
where STREMAILADDRESS like '%@dnr.state.ga.u';

update dbo.EPDUSERPROFILES
set STREMAILADDRESS = null
where STREMAILADDRESS like '%@no.email'
   or STREMAILADDRESS like '%@yahoo.com'
   or STREMAILADDRESS in ('Sam.Stevens', 'Tom.Atkinson', 'douglas.waldron@gaepd.org');
