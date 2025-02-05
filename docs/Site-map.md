# Site Map

* `/` Home page with application description/welcome portal/public search.
* `/Support` Help/support page.

---

## Public Pages

These pages are available to the public.

### Report Printouts

*Note: URLs are changed from existing reporting app. Appropriate [redirects](Redirects.md) should be implemented.*

* `/print/acc/{accId}` Printable ACC report page.
* `/print/source-test/{referenceNumber}` Printable source test report page.
* `/print/fce/{fcdId}` Printable FCE report page.

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

### Enforcement

* `/Enforcement` Enforcement search form.
* `/Enforcement/Details/{enforcementId}` Case File details.

#### Case File Initiation

* `/Enforcement/Add?{facilityId}` Start new case file for the specified facility.
* `/Enforcement/Add?{entryId}` Start new case file for the specified compliance entry.

#### Case File Workflow

* `/Enforcement/Edit/{enforcementId}` Edit details.
* `/Enforcement/[Close|Reopen]/{enforcementId}` Close/reopen a case file.
* `/Enforcement/LinkedEvents/{enforcementId}` Link a case file to a compliance event.
* `/Enforcement/[Delete|Restore]/{enforcementId}` Delete/restore a case file.

#### Enforcement Action Workflow

* `/Enforcement/Details/{enforcementId}/Action/{actionId}` View enforcement action details.
* `/Enforcement/Details/{enforcementId}/Action/Add` Add an enforcement action to a case file.
* `/Enforcement/Details/{enforcementId}/Action/Add/{actionId}` Add an enforcement action linked from another
  enforcement action.
* `/Enforcement/Details/{enforcementId}/Action/Edit/{actionId}` Edit an enforcement action details.
* `/Enforcement/Details/{enforcementId}/Action/RequestReview/{actionId}` Request review for an enforcement action.
* `/Enforcement/Details/{enforcementId}/Action/[Approve/Return]/{actionId}` Approve or return an enforcement
  action review.
* `/Enforcement/Details/{enforcementId}/Action/Issue/{actionId}` Issue (and close) an enforcement action.

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
