namespace IaipDataService.Structs;

public record struct Staff
{
    public int Id { get; init; }
    public PersonName Name { get; init; }
    public string EmailAddress { get; init; }
}
