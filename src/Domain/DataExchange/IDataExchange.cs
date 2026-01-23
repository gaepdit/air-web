using AirWeb.Domain.BaseEntities.Interfaces;
using System.ComponentModel;

namespace AirWeb.Domain.DataExchange;

// EPA Data Exchange properties are used by compliance monitoring, case files and enforcement actions.
public interface IDataExchange : IFacilityId
{
    public ushort? ActionNumber { get; set; }
    public DataExchangeStatus DataExchangeStatus { get; set; }
    public DateTimeOffset? DataExchangeStatusDate { get; set; }

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
    [Description("Not Applicable")] N,
    [Description("Updated")] U,
    [Description("Update Processed")] P,
    [Description("Deleted")] D,
    [Description("Deletion Processed")] X,
}

internal interface IDataExchangeWrite : IDataExchange;

internal static class DataExchangeExtensions
{
    extension(IDataExchangeWrite dx)
    {
        public void InitiateDataExchange(ushort actionNumber) => dx.SetActionNumber(actionNumber);
        public void UpdateDataExchange() => dx.SetDataExchangeStatus(DataExchangeStatus.U);
        public void DeleteDataExchange() => dx.SetDataExchangeStatus(DataExchangeStatus.D);

        private void SetActionNumber(ushort actionNumber)
        {
            dx.ActionNumber = actionNumber;
            dx.DataExchangeStatus = DataExchangeStatus.U;
            dx.DataExchangeStatusDate = DateTimeOffset.Now;
        }

        private void SetDataExchangeStatus(DataExchangeStatus status)
        {
            dx.DataExchangeStatus = status;
            dx.DataExchangeStatusDate = DateTimeOffset.Now;
        }
    }
}
