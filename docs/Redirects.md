# Site URL Redirects

This document defines existing routes that should be redirected according to the [Site Map](Site-map.md). Also included
are partial/incomplete URLs.

Rules are approximately ordered from the most frequently matched rule to the least frequently matched rule.

## Previous Reporting App

| Redirect                                                  | To                                         | Done |
|-----------------------------------------------------------|--------------------------------------------|:----:|
| `/facility/{facilityId}/acc-report/{id:int}`              | `/print/acc/{id:int}`                      |  ✔️  |
| `/facility/{facilityId}/fce/{id:int}`                     | `/print/fce/{id:int}`                      |  ✔️  |
| `/facility/{facilityId}/stack-test/{referenceNumber:int}` | `/print/source-test/{referenceNumber:int}` |  ✔️  |

### Incomplete URLs

| Redirect                           | To                             | Done |
|------------------------------------|--------------------------------|:----:|
| `/Facility/Details/`               | `/Facility/Index`              |  ✔️  |
| `/Facility/SourceTests/`           | `/Facility/Index`              |  ✔️  |
| `/Compliance/[FCE,Work]/Details/`  | `/Compliance/[FCE,Work]/Index` |  ✔️  | 
| `/Compliance/[FCE,Work]/(action)/` | `/Compliance/[FCE,Work]/Index` |  ✔️  | 
| `/Compliance/SourceTest/Details/`  | `/Compliance/Work/Index`       |  ✔️  | 
| `/Enforcement/Details/`            | `/Enforcement/Index`           |  ✔️  |
| `/Enforcement/(action)/`           | `/Enforcement/Index`           |  ✔️  |
| `/Enforcement/Edit/[Type]/`        | `/Enforcement/Index`           |  ✔️  |
| `/Enforcement/Edit/`               | `/Enforcement/Index`           |  ❌   |
| `/Admin/Maintenance/[Type]/Edit`   | `/Admin/Maintenance/Index`     |  ✔️  |
| `/Admin/Users/[Edit,EditRoles]`    | `/Admin/Users/Index`           |  ✔️  |
