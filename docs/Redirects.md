# Site URL Redirects

This document defines existing routes that should be redirected according to the [Site Map](Site-map.md). Also included
are partial/incomplete URLs.

Rules are approximately ordered from the most frequently matched rule to the least frequently matched rule.

## Previous Reporting App

| Redirect                                                  | To                                                                |
|-----------------------------------------------------------|-------------------------------------------------------------------|
| `/facility/{facilityId}/acc-report/{reportId}`            | `/report/facility/{facilityId}/acc/{id:int}`                      |
| `/facility/{facilityId}/stack-test/{referenceNumber:int}` | `/report/facility/{facilityId}/source-test/{referenceNumber:int}` |
| `/facility/{facilityId}/fce/{id:int}`                     | `/report/facility/{facilityId}/fce/{id:int}`                      |

### Incomplete URLs

| Redirect                                                   | To                                    |
|------------------------------------------------------------|---------------------------------------|
| `/Staff/Facility/Details/`                                 | `/Staff/Facility`                     |
| `/Staff/Compliance/[FCE,WorkEntry[/[Edit,Delete,Restore]]` | `/Staff/Compliance`                   | 
| `/Staff/SourceTests/Report`                                | `/Staff/SourceTests`                  |
| `/Staff/Enforcement/Details`                               | `/Staff/Enforcement`                  |
| `/Staff/Enforcement/Add/{facilityId}/ComplianceEvent`      | `/Staff/Enforcement/Add/{facilityId}` |
