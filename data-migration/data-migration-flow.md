# Data migration flow

## New tables

* `AspNetRoles` (automatically populated)
* `AspNetUserRoles`
* `AspNetUsers`
* `AuditPoints`
    * FCE
    * Case File
    * Work Entry
* `CaseFileComplianceEvents`
* `CaseFiles`
* `Comments`
    * FCE
    * Case File
    * Work Entry
* `ComplianceWork` (`WorkEntry` entity)
* `EmailLogs`
* `EnforcementActionReviews`
* `EnforcementActions`
* `Fces`
* `Lookups`
* `StipulatedPenalties`
* `ViolationTypes`

## Old tables

* `LK_VIOLATION_TYPE`
* `LOOKUPSSCPNOTIFICATIONS`
* `SSCPACCS`
* `SSCPACCSHISTORY` *(not migrated)*
* `SSCPENFORCEMENTSTIPULATED`
* `SSCPFCEMASTER`
* `SSCPFCE`
* `SSCPINSPECTIONS`
* `SSCPITEMMASTER`
* `SSCPNOTIFICATIONS`
* `SSCPREPORTS`
* `SSCPTESTREPORTS`
* `SSCP_AUDITEDENFORCEMENT`
* `SSCP_ENFORCEMENT` *(not migrated)*
* `SSCP_EnforcementEvents`

## General migration flow

```mermaid
---
title: Lookup tables
---
flowchart LR
    LK_VIOLATION_TYPE --> ViolationTypes
    LOOKUPSSCPNOTIFICATIONS --> Lookups:NotificationType
    LOOKUPEPDPROGRAMS/LOOKUPEPDUNITS --> Lookups:Office
```

```mermaid
---
title: FCEs
---
flowchart LR
    SSCPFCEMASTER --> Fces
    SSCPFCE --> Fces
```

```mermaid
---
title: Compliance Work
---
flowchart LR
    SSCPACCS --> ComplianceWork
    SSCPINSPECTIONS --> ComplianceWork
    SSCPITEMMASTER --> ComplianceWork
    SSCPNOTIFICATIONS --> ComplianceWork
    SSCPREPORTS --> ComplianceWork
    SSCPTESTREPORTS --> ComplianceWork
```

```mermaid
---
title: Enforcement Work
---
flowchart LR
    SSCP_AUDITEDENFORCEMENT --> CaseFiles
    SSCP_AUDITEDENFORCEMENT --> EnforcementActions
    SSCP_EnforcementEvents --> CaseFileComplianceEvents
    SSCPENFORCEMENTSTIPULATED --> StipulatedPenalties
    none --> EnforcementActionReviews
```

```mermaid
---
title: Audit Points
---
flowchart LR
    SSCPITEMMASTER --> AuditPoints
    SSCPFCEMASTER --> AuditPoints
    SSCP_AUDITEDENFORCEMENT --> AuditPoints
```
