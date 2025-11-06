# Site Map

* `/` Home page with application description and sign-in options.
* `/Support` Help/support page.

---

## Public Pages

These pages are available to the public.

### Report Printouts

*Note: URLs are changed from existing reporting app. Appropriate [redirects](Redirects.md) have been implemented.*

* `/print/acc/{accId}` Printable ACC report page.
* `/print/fce/{fcdId}` Printable FCE report page.
* `/print/source-test/{referenceNumber}` Printable source test report page.

---

## Staff Pages

These pages are only available to logged-in staff.

* `/` Staff dashboard.

### Facility

* `/Facility` Facility list and quick-find form.
* `/Facility/Details/{facilityId}` Facility details page (some data retrieved from IAIP database).

### Compliance

#### FCEs

* `/Compliance/FCE` FCE search form.
* `/Compliance/FCE/Details/{fceId}` FCE details page.
* `/Compliance/FCE/Add/{facilityId}` Add a new FCE for the specified Facility.
* `/Compliance/FCE/Edit/{fceId}` Edit an FCE.
* `/Compliance/FCE/[Delete|Restore]/{fceId}` Delete/restore an FCE.

#### Compliance Monitoring

* `/Compliance/Work` Compliance monitoring search form.
* `/Compliance/Work/Details/{entryId}` Compliance monitoring details page.
* `/Compliance/Work/{workEntryType}/Add/{facilityId}` Add a new compliance monitoring record of the specified type for
  the specified Facility.
* `/Compliance/Work/{workEntryType}/Edit/{entryId}` Edit a compliance monitoring record.
* `/Compliance/Work/Edit/{entryId}` Redirect to the appropriate compliance monitoring record edit page.
* `/Compliance/Work/[Close|Reopen]/{entryId}` Close/reopen a compliance monitoring record.
* `/Compliance/Work/[Delete|Restore]/{entryId}` Delete/restore a compliance monitoring record.

### Source Tests

* `/Compliance/SourceTest` List of source tests needing compliance review.
* `/Compliance/SourceTest/Details/{referenceNumber}` Source test report details page (data retrieved from IAIP
  database).
* `/Compliance/SourceTest/Details/{referenceNumber}#compliance-review` Source test compliance review or embedded form
  for adding a review.

### Enforcement Cases

* `/Enforcement` Enforcement case search form.
* `/Enforcement/Details/{caseFileId}` Enforcement case details page.

#### Case File Workflow

* `/Enforcement/Begin/{facilityId}/{eventId?}` Start a new case file for the specified facility and, if included, the
  specified compliance event.
* `/Enforcement/Edit/{caseFileId}` Edit case file details.
* `/Enforcement/[Close|Reopen]/{caseFileId}` Close/reopen a case file.
* `/Enforcement/LinkedEvents/{caseFileId}` Link a case file to a compliance event.
* `/Enforcement/PollutantsPrograms/{caseFileId}` Edit the associated pollutants and air programs and the violation type.
* `/Enforcement/[Delete|Restore]/{caseFileId}` Delete/restore a case file.

#### Enforcement Action Workflow

* `/Enforcement/Details/{caseFileId}#enforcement-actions` List of enforcement actions with details.
* `/Enforcement/Details/{caseFileId}` `[modal dialog]` Add a simple enforcement action to a case file.
* `/Enforcement/Add/{action-type}/{caseFileId}` Add a complex enforcement action to a case file.
* `/Enforcement/Edit/{actionId}` Edit enforcement action details.
* `/Enforcement/RequestReview/{actionId}` Request review for an enforcement action.
* `/Enforcement/SubmitReview/{actionId}` Submit review for an enforcement action.
* `/Enforcement/StipulatedPenalties/{actionId}` Edit stipulated penalties for a Consent Order.
* `/Enforcement/Details/{caseFileId}` `[modal dialogs]` Issue/cancel/add response/resolve/execute/delete an enforcement
  action.

**NOTE:** `/Enforcement/Edit/{id}` can result in editing either a case file or enforcement action depending on whether
the ID is an integer or GUID.

## User Account

* `/Account` View profile.
* `/Account/Login` Work account login form.
* `/Account/Edit` Edit contact info.

## Admin pages

### User Management

* `/Admin/Users` User search page.
* `/Admin/Users/Details/{userId}` View user profile.
* `/Admin/Users/Edit/{userId}` Edit contact info.
* `/Admin/Users/EditRoles/{userId}` Edit roles.

### Site Maintenance

Maintenance pages available to Site Admin personnel to modify lookup tables used for drop-down lists.

* `/Admin/Maintenance` List of maintenance item types.
* `/Admin/Maintenance/[type]` List of items of given type.
* `/Admin/Maintenance/[type]/Add` Add new item.
* `/Admin/Maintenance/[type]/Edit/{itemId}` Edit item.
