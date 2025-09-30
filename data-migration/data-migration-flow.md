# Data migration flow

## New tables

* `AuditPoints`
* `CaseFileComplianceEvents`
* `CaseFiles`
* `Comments`
* `EmailLogs`
* `EnforcementActionReviews`
* `EnforcementActions`
* `Fces`
* `SelectLists`
* `StipulatedPenalties`
* `ViolationTypes`
* `WorkEntries`

## Old tables

* `LK_VIOLATION_TYPE`
* `LOOKUPSSCPNOTIFICATIONS`
* `SSCPACCS`
* `SSCPENFORCEMENTSTIPULATED`
* `SSCPFCEMASTER`
* `SSCPFCE`
* `SSCPINSPECTIONS`
* `SSCPITEMMASTER`
* `SSCPNOTIFICATIONS`
* `SSCPREPORTS`
* `SSCPTESTREPORTS`
* `SSCP_AUDITEDENFORCEMENT`
* `SSCP_EnforcementEvents`

## General migration flow

```mermaid
---
title: Lookup tables
---
flowchart LR
    LK_VIOLATION_TYPE --> ViolationTypes
    LOOKUPSSCPNOTIFICATIONS --> SelectLists:NotificationType
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
    SSCPACCS --> WorkEntries
    SSCPINSPECTIONS --> WorkEntries
    SSCPITEMMASTER --> WorkEntries
    SSCPNOTIFICATIONS --> WorkEntries
    SSCPREPORTS --> WorkEntries
    SSCPTESTREPORTS --> WorkEntries
```

```mermaid
---
title: Compliance Work
---
flowchart LR
    SSCP_AUDITEDENFORCEMENT --> CaseFiles
    SSCP_AUDITEDENFORCEMENT --> EnforcementActionReviews
    SSCP_AUDITEDENFORCEMENT --> EnforcementActions
    SSCPENFORCEMENTSTIPULATED --> StipulatedPenalties
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
