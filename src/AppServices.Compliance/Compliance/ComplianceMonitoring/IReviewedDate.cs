namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;

public interface IReviewedDate
{
    [Display(Name = "Date Reviewed")]
    DateOnly? ReviewedDate { get; }
}
