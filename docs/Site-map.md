# Site Map

* `/` Home page with application description/welcome portal/public search.
* `/Support` Help/support page.

---

## Public Pages

These pages are available to the public.

### Report Printouts

*Note: URLs are changed from existing reporting app. Appropriate [redirects](Redirects.md) should be implemented.*

* `/print/acc/{accId}` Printable ACC report page.
* `/print/fce/{fcdId}` Printable FCE report page.
* `/print/source-test/{referenceNumber}` Printable source test report page.

---

## Staff Pages

These pages are only available to logged-in staff.

* `/Home` Staff dashboard.

### Facility

* `/Facility` Facility quick-find form.
* `/Facility/Details/{facilityId}` Facility details page (retrieved from IAIP database).

### Compliance

* `/Compliance` Compliance dashboard.

#### FCE Workflow

* `/Compliance/FCE` FCE search form.
* `/Compliance/FCE/Details/{fceId}` FCE details page.
* `/Compliance/FCE/Add/{facilityId}` Add a new FCE for the specified Facility.
* `/Compliance/FCE/Edit/{fceId}` Edit an FCE.
* `/Compliance/FCE/[Delete|Restore]/{fceId}` Delete/restore an FCE.

#### Compliance Work Entry Workflow

* `/Compliance/Work` Compliance Work Entry search form.
* `/Compliance/Work/Details/{entryId}` Compliance Work Entry details page.
* `/Compliance/Work/{workEntryType}/Add/{facilityId}` Add a new Work Entry of the specified type for the
  specified Facility.
* `/Compliance/Work/{workEntryType}/Edit/{entryId}` Edit a Work Entry.
* `/Compliance/Work/Edit/{entryId}` Redirect to appropriate Work Entry edit page.
* `/Compliance/Work/[Close|Reopen]/{entryId}` Close/reopen a Work Entry (applies to ACCs and Permit Revocations only).
* `/Compliance/Work/[Delete|Restore]/{entryId}` Delete/restore a Work Entry.

### Source Tests

* `/Compliance/TestReport/{referenceNumber}` Source Test report details page (retrieved from IAIP database).
* `/Compliance/TestReport/{referenceNumber}#compliance-review` Embedded form for adding a Source Test Compliance
  Review.

### Enforcement Case File

* `/Enforcement` Enforcement search form.
* `/Enforcement/Details/{caseFileId}` Enforcement Case File details.

#### Case File Workflow

* `/Enforcement/Begin/{facilityId}/{eventId?}` Start a new case file for the specified facility and, if included, the
  specified compliance event.
* `/Enforcement/Edit/{caseFileId}` Edit details.
* `/Enforcement/[Close|Reopen]/{caseFileId}` Close/reopen a case file.
* `/Enforcement/LinkedEvents/{caseFileId}` Link a case file to a compliance event.
* `/Enforcement/PollutantsPrograms/{caseFileId}` Link a case file to facility pollutants and air programs.
* `/Enforcement/[Delete|Restore]/{caseFileId}` Delete/restore a case file.

#### Enforcement Action Workflow

* `/Enforcement/Details/{caseFileId}#enforcement-actions` Enforcement action details.
* `/Enforcement/Details/{caseFileId}` `[modal dialog]` Add a simple enforcement action to a case file.
* `/Enforcement/Details/{caseFileId}/Add/{action-type}` Add a complex enforcement action to a case file.
* `/Enforcement/Details/{caseFileId}/Edit/{actionId}` Edit enforcement action details.
* `/Enforcement/Details/{caseFileId}/RequestReview/{actionId}` Request review for an enforcement action.
* `/Enforcement/Details/{caseFileId}/[Approve/Return]/{actionId}` Approve or return an enforcement
  action review.
* `/Enforcement/Details/{caseFileId}` `[modal dialog]` Issue/cancel/add response/resolve/execute/delete an enforcement
  action.

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
