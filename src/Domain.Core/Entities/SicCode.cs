using System.Text;

namespace AirWeb.Domain.Core.Entities;

public record SicCode
{
    public required string Id { get; init; }
    public required string Description { get; init; }
    public bool Active { get; init; } = true;

    public string Display
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append($"{Id} – {Description}");
            if (!Active) sn.Append(" [Inactive]");
            return sn.ToString();
        }
    }
}
