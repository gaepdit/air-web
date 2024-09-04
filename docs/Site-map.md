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

* `/Home` Staff dashboard.

### Facility

* `/Facility` Facility search form (searches IAIP database).
* `/Facility/Details/{facilityId}` Facility details page.

### Compliance

* `/Compliance` Compliance dashboard.

#### FCE Workflow

* `/Compliance/FCE` FCE search form.
* `/Compliance/FCE/Details/{fceId}` FCE details page.
* ~~`/Compliance/FCE/Add` Add a new FCE.~~
* `/Compliance/FCE/Add/{facilityId}` Add a new FCE for the specified Facility.
* `/Compliance/FCE/Edit/{fceId}` Edit an FCE.
* `/Compliance/FCE/[Delete|Restore]/{fceId}` Delete/restore an FCE.

#### Compliance Work Entry Workflow

* `/Compliance/Work` Compliance Work Entry search form.
* `/Compliance/Work/Details/{entryId}` Compliance Work Entry details page.
* ~~`/Compliance/Work/Add` Add a new Work Entry.~~
* ~~`/Compliance/Work/Add?{facilityId}` Add a new Work Entry for the Facility.~~
* ~~`/Compliance/Work/Add/{workEntryType}` Add a new Work Entry of the specified type.~~
* `/Compliance/Work/{workEntryType}/Add/{facilityId}` Add a new Work Entry of the specified type for the
  specified Facility.
* `/Compliance/Work/Edit/{entryId}` Edit a Work Entry.
* `/Compliance/Work/[Close|Reopen]/{entryId}` Close/reopen a Work Entry (applies to ACCs and Permit Revocations only).
* `/Compliance/Work/[Delete|Restore]/{entryId}` Delete/restore a Work Entry.

### Source Tests

* `/Compliance/Tests` Source Test search form (searches IAIP database).
* `/Compliance/Tests/Report/{referenceNumber}` Source Test report details page (with compliance review details).
* `/Compliance/Tests/Report/{referenceNumber}#compliance-review` Embedded form for adding a Source Test Compliance
  Review.

### Enforcement

* `/Compliance/Enforcement` Enforcement search form.
* `/Compliance/Enforcement/Details/{enforcementId}` Enforcement details.

#### Enforcement Case Initiation

* ~~`/Compliance/Enforcement/Add` Start new enforcement case.~~
* `/Compliance/Enforcement/Add/{facilityId}` Start new enforcement case for the specified facility.
* `/Compliance/Enforcement/Add/{facilityId}/{entryId}` Start new enforcement case for the specified work entry.

#### Enforcement Case Workflow

* `/Compliance/Enforcement/Edit/{enforcementId}` Edit enforcement details.
* `/Compliance/Enforcement/[Close|Reopen]/{enforcementId}` Close/reopen an enforcement case.
* `/Compliance/Enforcement/Link/{enforcementId}` Link an enforcement case to a compliance event.
* `/Compliance/Enforcement/[Delete|Restore]/{enforcementId}` Delete/restore an enforcement case.

#### Enforcement Action Workflow

* `/Compliance/Enforcement/Details/{enforcementId}/Action/{actionId}` View enforcement action details.
* `/Compliance/Enforcement/Details/{enforcementId}/Action/Add` Add an enforcement action to an enforcement case.
* `/Compliance/Enforcement/Details/{enforcementId}/Action/Add/{actionId}` Add an enforcement action linked from another
  enforcement action.
* `/Compliance/Enforcement/Details/{enforcementId}/Action/Edit/{actionId}` Edit an enforcement action details.
* `/Compliance/Enforcement/Details/{enforcementId}/Action/RequestReview/{actionId}` Request review for an enforcement action.
* `/Compliance/Enforcement/Details/{enforcementId}/Action/[Approve/Return]/{actionId}` Approve or return an enforcement
  action review.
* `/Compliance/Enforcement/Details/{enforcementId}/Action/Issue/{actionId}` Issue (and close) an enforcement action.

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
