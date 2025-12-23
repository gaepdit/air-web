using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public abstract class ReportableEnforcementAction : EnforcementAction, IDataExchange, IDataExchangeWrite
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected ReportableEnforcementAction() { }

    private protected ReportableEnforcementAction(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user) { }

    // Properties
    [JsonIgnore]
    public ushort? ActionNumber { get; internal set; }

    [JsonIgnore]
    [StringLength(1)]
    public DataExchangeStatus DataExchangeStatus { get; internal set; }

    [JsonIgnore]
    public DateTimeOffset? DataExchangeStatusDate { get; internal set; }

    internal void RemoveFromDataExchange()
    {
        if (ActionNumber is not null) this.DeleteDataExchange();
    }

    internal void AddToDataExchange(ushort actionNumber)
    {
        if (ActionNumber is null) this.InitiateDataExchange(actionNumber);
        else this.UpdateDataExchange();
    }

    void IDataExchangeWrite.SetActionNumber(ushort actionNumber)
    {
        ActionNumber = actionNumber;
        DataExchangeStatus = DataExchangeStatus.I;
        DataExchangeStatusDate = DateTimeOffset.Now;
    }

    void IDataExchangeWrite.SetDataExchangeStatus(DataExchangeStatus status)
    {
        DataExchangeStatus = status;
        DataExchangeStatusDate = DateTimeOffset.Now;
    }
}
