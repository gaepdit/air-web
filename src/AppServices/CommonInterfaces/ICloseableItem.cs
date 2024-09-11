namespace AirWeb.AppServices.CommonInterfaces;

public interface ICloseableItem
{
    public bool TrackClosure { get; }
    public bool IsClosed { get; }
}
