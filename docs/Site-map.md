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

* `/report/facility/{facilityId}/acc/{accId}` Printable ACC report page.
* `/report/facility/{facilityId}/source-test/{referenceNumber}` Printable source test report page.
* `/report/facility/{facilityId}/fce/{fcdId}` Printable FCE report page.

---

## Staff Pages

These pages are only available to logged-in staff.

* `/Staff` Staff dashboard.

### Facility

* `/Staff/Facility` Facility search form (searches IAIP database).
* `/Staff/Facility/Details/{facilityId}` Facility details page.

### Compliance

* `/Staff/Compliance` Compliance search form (combined search for FCEs and Work Entries).

#### Compliance Work Entry Workflow

* `/Staff/Compliance/WorkEntry/{entryId}` Compliance work entry details page (compliance events, notifications, and
  permit revocations).
* `/Staff/Compliance/WorkEntry/Add` Add a new Work Entry.
* `/Staff/Compliance/WorkEntry/Add?{facilityId}` Add a new Work Entry for the Facility.
* `/Staff/Compliance/WorkEntry/Add/{workEntryType}` Add a new Work Entry of the specified type.
* `/Staff/Compliance/WorkEntry/Add/{workEntryType}?{facilityId}` Add a new Work Entry of the specified type for the
  specified Facility.
* `/Staff/Compliance/WorkEntry/Edit/{entryId}` Edit a Work Entry.
* `/Staff/Compliance/WorkEntry/Delete/{entryId}` Delete a Work Entry.
* `/Staff/Compliance/WorkEntry/Restore/{entryId}` Restore a Work Entry.

#### FCE Workflow

* `/Staff/Compliance/FCE/{fceId}` FCE details page.
* `/Staff/Compliance/FCE/Add` Add a new FCE.
* `/Staff/Compliance/FCE/Add?{facilityId}` Add a new FCE for the specified Facility.
* `/Staff/Compliance/FCE/Edit/{fceId}` Edit an FCE.
* `/Staff/Compliance/FCE/Delete/{fceId}` Delete an FCE.
* `/Staff/Compliance/FCE/Restore/{fceId}` Restore an FCE.

### Source Tests

* `/Staff/SourceTests` Source Test search form (searches IAIP database).
* `/Staff/SourceTests/Report/{referenceNumber}` Source Test report details page (with compliance review details).
* `/Staff/SourceTests/Report/{referenceNumber}#compliance-review` Embedded form for adding a Source Test Compliance
  Review.

### Enforcement

* `/Staff/Enforcement` Enforcement search form.
* `/Staff/Enforcement/Details/{enforcementId}` Enforcement details.

#### Enforcement Case Initiation

* `/Staff/Enforcement/Add` Start new enforcement case.
* `/Staff/Enforcement/Add?{facilityId}` Start new enforcement case for the specified facility.
* `/Staff/Enforcement/Add?{facilityId}&{entryId}` Start new enforcement case for the specified work entry.

#### Enforcement Case Workflow

* `/Staff/Enforcement/Edit/{enforcementId}` Edit enforcement details.
* `/Staff/Enforcement/[Close|Reopen]/{enforcementId}` Close/reopen an enforcement case.
* `/Staff/Enforcement/Link/{enforcementId}` Link an enforcement case to a compliance event.

#### Enforcement Action Workflow

* `/Staff/Enforcement/Details/{enforcementId}/Action/{actionId}` View enforcement action details.
* `/Staff/Enforcement/Details/{enforcementId}/Action/Add` Add an enforcement action to an enforcement case.
* `/Staff/Enforcement/Details/{enforcementId}/Action/Add?{actionId}` Add an enforcement action linked from another
  enforcement action.
* `/Staff/Enforcement/Details/{enforcementId}/Action/Edit/{actionId}` Edit an enforcement action details.
* `/Staff/Enforcement/Details/{enforcementId}/Action/RequestReview/{actionId}` Request review for an enforcement action.
* `/Staff/Enforcement/Details/{enforcementId}/Action/[Approve/Return]/{actionId}` Approve or return an enforcement
  action review.
* `/Staff/Enforcement/Details/{enforcementId}/Action/Issue/{actionId}` Issue (and close) an enforcement action.

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
