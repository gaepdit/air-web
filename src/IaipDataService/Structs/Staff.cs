namespace IaipDataService.Structs;

public readonly record struct Staff
{
    public int Id { get; init; }
    public Guid IdAsGuid => IntToGuid(Id);
    public PersonName Name { get; init; }
    public string EmailAddress { get; init; }
    public bool Active { get; init; }

    public static Guid IntToGuid(int value)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}
