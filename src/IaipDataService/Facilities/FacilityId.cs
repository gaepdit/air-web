﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IaipDataService.Facilities;

public partial record FacilityId : IComparable<FacilityId>
{
    private readonly string? _id = string.Empty;

    // Constructor

    [UsedImplicitly] // Used by ORM.
    private FacilityId() { }

    public FacilityId(string id) => Id = id;

    // Properties

    [Key]
    public string Id
    {
        get => _id ?? throw new InvalidOperationException("Id not initialized.");

        private init => _id = IsValidFormat(value)
            ? value.Replace("-", "")
            : throw new ArgumentException($"The value '{value}' is not a valid Facility ID format.");
    }

    public string FormattedId => $"{Id[..3]}-{Id[3..8]}";
    public string EpaFacilityIdentifier => $"GA00000013{Id}";

    // Operators
    public static implicit operator string(FacilityId id) => id.FormattedId;
    public static explicit operator FacilityId(string id) => new(id);
    public override string ToString() => FormattedId;
    public virtual bool Equals(FacilityId? other) => !string.IsNullOrEmpty(other?._id) && other._id == _id;
    public int CompareTo(FacilityId? other) => string.Compare(_id, other?._id, StringComparison.Ordinal);
    public override int GetHashCode() => string.GetHashCode(_id, StringComparison.Ordinal);

    // Format validation
    private static bool IsValidFormat(string id) => FacilityIdRegex().IsMatch(id);

    // FUTURE: Update regex to limit first three digits based on county list.
    // Test at https://regex101.com/r/2uYyHl/4
    // language:regex
    private const string FacilityIdPattern = @"^\d{3}-?\d{5}$";

    [GeneratedRegex(FacilityIdPattern)]
    private static partial Regex FacilityIdRegex();

    public static string CleanFacilityId(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var cleanFacilityId = new string(input.Where(c => c == '-' || char.IsDigit(c)).ToArray());
        return EightDigitsRegex().IsMatch(cleanFacilityId)
            ? cleanFacilityId.Insert(3, "-")
            : cleanFacilityId;
    }

    private const string EightDigits = @"^\d{8}$";

    [GeneratedRegex(EightDigits)]
    private static partial Regex EightDigitsRegex();
}
