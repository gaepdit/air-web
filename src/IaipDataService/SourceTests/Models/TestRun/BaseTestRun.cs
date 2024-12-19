using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IaipDataService.SourceTests.Models.TestRun;

public abstract record BaseTestRun
{
    [Display(Name = "Test run #")]
    public string RunNumber { get; init; } = null!;

    #region Confidential info handling

    [JsonIgnore]
    public string ConfidentialParametersCode { protected get; init; } = null!;

    protected ICollection<string> ConfidentialParameters { get; set; } = new HashSet<string>();

    protected abstract BaseTestRun RedactedTestRun();

    protected T RedactedBaseTestRun<T>() where T : BaseTestRun =>
        (T)this with
        {
            RunNumber = CheckConfidential(RunNumber, nameof(RunNumber)),
        };

    protected string CheckConfidential(string input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? BaseSourceTestReport.ConfidentialInfoPlaceholder
            : input;

    protected ValueWithUnits CheckConfidential(ValueWithUnits input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? input with { Value = BaseSourceTestReport.ConfidentialInfoPlaceholder }
            : input;

    protected abstract void ParseConfidentialParameters();

    protected void ParseBaseConfidentialParameters() => AddIfConfidential(1, nameof(RunNumber));

    // Uses "ONE"-based position to better correlate with IAIP code
    protected void AddIfConfidential(int position, string parameter)
    {
        if (ConfidentialParametersCode.Length < position) return;
        if (ConfidentialParametersCode[position - 1] == '1') ConfidentialParameters.Add(parameter);
    }

    internal static List<T> RedactedTestRuns<T>(List<T> testRuns) where T : BaseTestRun =>
        testRuns.Select(r => (T)r.RedactedTestRun()).ToList();

    internal static List<T> ParsedTestRuns<T>(List<T> testRuns) where T : BaseTestRun
    {
        var parsedTestRuns = new List<T>();
        foreach (var r in testRuns)
        {
            r.ParseConfidentialParameters();
            parsedTestRuns.Add(r);
        }

        return parsedTestRuns;
    }

    #endregion
}
