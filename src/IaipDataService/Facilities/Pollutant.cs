using System.ComponentModel.DataAnnotations;

namespace IaipDataService.Facilities;

public record Pollutant(
    [StringLength(9)] string Code,
    string Description
);
