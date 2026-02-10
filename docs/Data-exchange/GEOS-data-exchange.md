# GEOS DX Code

Note: Only code related to Air Branch compliance and enforcement is listed. The inspection tracking section of GEOS has
never been used. All the tables and triggers listed below are inactive. References to inspection data should be removed
from the ETL scripts.

| Database     | Object                                                           | Type    | Repo        | Use                             | Modification needed |
|--------------|------------------------------------------------------------------|---------|-------------|---------------------------------|---------------------|
| `AIRBRANCH`  | `dbo.TG_GST_INSP_INSPECTION / GST_INSP_INSPECTION`               | Trigger | `airbranch` | Update status                   | Obsolete - ignore   |
| `AIRBRANCH`  | `dbo.TG_GST_INSP_USERID / GST_INSP_INSPECTION`                   | Trigger | `airbranch` | Manage user data                | Obsolete - ignore   |
| `AIRBRANCH`  | `dbo.TG_GST_INSP_INSPECTION_RESULT / GST_INSP_INSPECTION_RESULT` | Trigger | `airbranch` | Update status                   | Obsolete - ignore   |
| `AIRBRANCH`  | `dbo.TG_GST_INSP_XMLDATA / GST_INSP_XMLDATA`                     | Trigger | `airbranch` | Update status                   | Obsolete - ignore   |
| `AIRBRANCH`  | `dbo.GEOS_INSPECTIONS_XREF`                                      | Table   | `airbranch` | Staging table                   | Obsolete - ignore   |
| `AIRBRANCH`  | `dbo.GST_INSP_INSPECTION_RESULT`                                 | Table   | `airbranch` | Staging table                   | Obsolete - ignore   |
| `AIRBRANCH`  | `dbo.GST_INSP_INSPECTION`                                        | Table   | `airbranch` | Staging table                   | Obsolete - ignore   |
| `AIRBRANCH`  | `dbo.GST_INSP_XMLDATA`                                           | Table   | `airbranch` | Staging table                   | Obsolete - ignore   |
| `AIRBRANCH`  | `etl.GEOS_STG_TO_IAIP_INSPECTIONS`                               | SProc   | `geos`      | Staging tables ETL to IAIP      | *Disable*           |
| `MFL_GA_STG` | `USP_STG_TO_IAIP`                                                | SProc   | `geos`      | GEOS data ETL to staging tables | *Refactor*          |
