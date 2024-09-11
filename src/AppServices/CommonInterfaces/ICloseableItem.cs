using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.CommonInterfaces;

public interface ICloseableItem
{
    public bool TrackClosure { get; }

    [Display(Name = "Closed")]
    public bool IsClosed { get; }
}
