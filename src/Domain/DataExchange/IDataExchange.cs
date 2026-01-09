using AirWeb.Domain.BaseEntities.Interfaces;
using System.ComponentModel;

namespace AirWeb.Domain.DataExchange;

// EPA Data Exchange properties are used by compliance monitoring, case files and enforcement actions.
public interface IDataExchange : IFacilityId
{
    public ushort? ActionNumber { get; }
    public DataExchangeStatus DataExchangeStatus { get; }
    public DateTimeOffset? DataExchangeStatusDate { get; }

    /// <summary>
    /// The ID used by EPA.
    /// </summary>
    public string? EpaActionId =>
        ActionNumber is null ? null : $"GA000A000013{((FacilityId)FacilityId).Id}{ActionNumber:D5}";

    public string EpaFacilityIdentifier => ((FacilityId)FacilityId).EpaFacilityIdentifier;
}

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum DataExchangeStatus
{
    [Description("Not Included")] N,
    [Description("Processed")] P,
    [Description("Inserted")] I,
    [Description("Updated")] U,
    [Description("Deleted")] D,
}

internal interface IDataExchangeWrite
{
    void SetActionNumber(ushort actionNumber);
    void SetDataExchangeStatus(DataExchangeStatus status);
}

internal static class DataExchangeExtensions
{
    extension(IDataExchangeWrite dx)
    {
        public void InitiateDataExchange(ushort actionNumber) => dx.SetActionNumber(actionNumber);
        public void UpdateDataExchange() => dx.SetDataExchangeStatus(DataExchangeStatus.U);
        public void DeleteDataExchange() => dx.SetDataExchangeStatus(DataExchangeStatus.D);
    }
}
