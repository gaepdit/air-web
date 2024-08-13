namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class ComplianceEvent : WorkEntry
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private protected ComplianceEvent() { }

    private protected ComplianceEvent(int? id) : base(id)
    {
        IsComplianceEvent = true;
        EpaDxStatus = DxStatus.Inserted;
    }
}
