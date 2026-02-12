using AirWeb.Domain.CommonInterfaces;
using System.ComponentModel;

namespace AirWeb.Domain.DataExchange;

// EPA Data Exchange properties are used by compliance monitoring, case files and enforcement actions.

public interface IDataExchange : IFacilityId
{
    public DataExchangeStatus DataExchangeStatus { get; set; }
    public DateTimeOffset? DataExchangeStatusDate { get; set; }

    public string EpaFacilityId => ((FacilityId)FacilityId).EpaFacilityId;
}

public interface IDataExchangeAction : IDataExchange
{
    public ushort? ActionNumber { get; set; }

    /// <summary>
    /// The ID used by EPA.
    /// </summary>
    public string? EpaActionId =>
        ActionNumber is null ? null : $"GA000A000013{((FacilityId)FacilityId).Id}{ActionNumber:D5}";
}

internal static class DataExchangeExtensions
{
    extension(IDataExchange dx)
    {
        public void UpdateDataExchange() => dx.SetDataExchangeStatus(DataExchangeStatus.U);

        public void DeleteDataExchange()
        {
            if (dx.DataExchangeStatus is DataExchangeStatus.U or DataExchangeStatus.P)
                dx.SetDataExchangeStatus(DataExchangeStatus.D);
        }

        private void SetDataExchangeStatus(DataExchangeStatus status)
        {
            dx.DataExchangeStatus = status;
            dx.DataExchangeStatusDate = DateTimeOffset.Now;
        }
    }

    extension(IDataExchangeAction dx)
    {
        public void InitializeDataExchange(ushort actionNumber)
        {
            dx.ActionNumber = actionNumber;
            dx.UpdateDataExchange();
        }
    }
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
