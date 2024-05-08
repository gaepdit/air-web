# Site Map

* `/` Home page with application description/welcome portal/public search.
* `/Support` Help/support page.

---

## Public Pages

These pages are available to the public.

* *Add a list of public pages.*

### Reporting

*Note: URLs are changed from existing reporting app. Appropriate [redirects](Redirects.md) should be implemented.*

Pages must be named "report" because "reports" is reserved by the ArcGIS application.

* `/report/facility/{facilityId}/acc/{id:int}` Printable ACC report page.
* `/report/facility/{facilityId}/source-monitoring/{referenceNumber:int}` Printable source monitoring report page.
* `/report/facility/{facilityId}/fce/{id:int}` Printable FCE report page.

---

## Staff Pages

These pages are only available to logged-in staff.

* `/Staff` Staff dashboard.

### Facility

* `/Staff/Facility` Facility search form (searches IAIP database).
* `/Staff/Facility/Details/{facilityId}` Facility details page.

### Compliance

* `/Staff/Compliance` Compliance search form (combined search for FCEs and Work Entries).
* `/Staff/Compliance/FCE/{fceId:int}` FCE details page.
* `/Staff/Compliance/WorkEntry/{entryId:int}` Compliance work entry details page (compliance events, notifications, and
  permit revocations).

#### Compliance Monitoring Workflow

* `/Staff/Compliance/WorkEntry/Add` Add a new Work Entry.
* `/Staff/Compliance/WorkEntry/Add/{facilityId}` Add a new Work Entry for the Facility.
* `/Staff/Compliance/WorkEntry/Add/[work entry type]` Add a new Work Entry of the specified type.
* `/Staff/Compliance/WorkEntry/Add/[work entry type]/{facilityId}` Add a new Work Entry of the specified type for the Facility.
* `/Staff/Compliance/WorkEntry/Edit/{id:int}` Edit a Work Entry.
* `/Staff/Compliance/WorkEntry/Delete/{id:int}` Delete a Work Entry.
* `/Staff/Compliance/WorkEntry/Restore/{id:int}` Restore a Work Entry.

#### FCE Workflow

* `/Staff/Compliance/FCE/Add` Add a new FCE.
* `/Staff/Compliance/FCE/Add/{facilityId}` Add a new FCE for the Facility.
* `/Staff/Compliance/FCE/Edit/{id:int}` Edit an FCE.
* `/Staff/Compliance/FCE/Delete/{id:int}` Delete an FCE.
* `/Staff/Compliance/FCE/Restore/{id:int}` Restore an FCE.

### Source Monitoring

* `/Staff/SourceMonitoring` Source Monitoring search form (searches IAIP database).
* `/Staff/SourceMonitoring/Report/{referenceNumber:int}` Source Monitoring report details page (with compliance work
  entry details).
* `/Staff/SourceMonitoring/Report/{referenceNumber:int}#compliance-review` Embedded form for adding a Source Monitoring
  Review.

### Enforcement

* `/Staff/Enforcement` Enforcement search form.
* `/Staff/Enforcement/Details/{enforcementId:int}` Enforcement details.

### Enforcement Initiation

* `/Staff/Enforcement/Add` Start new enforcement.
* `/Staff/Enforcement/Add/{facilityId}` Start new enforcement for the facility.
* `/Staff/Enforcement/Add/{facilityId}/ComplianceEvent/{entryId:int}` Start new enforcement for the work entry.

### Enforcement Workflow

TBD

## User Account

* `/Account` View profile.
* `/Account/Login` Work account login form.
* `/Account/Edit` Edit contact info.
* `/Account/Settings` Potential location for a personal settings page.

## Admin pages

### Reports

Pages must be named "Reporting" because "Reports" is reserved by the ArcGIS application.

* `/Admin/Reporting` Management & error reports.
* `/Admin/Reporting/[report type]` View report.

### Site Maintenance

Maintenance pages available to Site Admin personnel to modify lookup tables used for drop-down lists.

* `/Admin/Maintenance` List of maintenance item types.
* `/Admin/Maintenance/[type]` List of items of given type.
* `/Admin/Maintenance/[type]/Add` Add new item.
* `/Admin/Maintenance/[type]/Edit/{id}` Edit item.

### User Management

* `/Admin/Users` User search.
* `/Admin/Users/Details/{id}` View user profile.
* `/Admin/Users/EditProfile/{id}` Edit contact info.
* `/Admin/Users/EditRoles/{id}` Edit roles.
