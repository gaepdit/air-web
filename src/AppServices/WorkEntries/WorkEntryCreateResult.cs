using MyApp.Domain.Entities.WorkEntries;

namespace MyApp.AppServices.WorkEntries;

public class WorkEntryCreateResult
{
    /// <summary>
    /// Returns <see cref="WorkEntryCreateResult"/> indicating a successfully created <see cref="WorkEntry"/>.
    /// </summary>
    /// <param name="workEntryId">The ID of the new WorkEntry.</param>
    /// <returns><see cref="WorkEntryCreateResult"/> indicating a successful operation.</returns>
    public WorkEntryCreateResult(Guid workEntryId) => WorkEntryId = workEntryId;

    /// <summary>
    /// If the <see cref="WorkEntry"/> is successfully created, contains the ID of the new WorkEntry. 
    /// </summary>
    /// <value>The WorkEntry ID if the operation succeeded, otherwise null.</value>
    public Guid? WorkEntryId { get; protected init; }

    /// <summary>
    /// <see cref="List{T}"/> of <see cref="string"/> containing warnings that occurred during the operation.
    /// </summary>
    /// <value>A <see cref="List{T}"/> of <see cref="string"/> instances.</value>
    public List<string> Warnings { get; } = [];

    /// <summary>
    /// Flag indicating whether warnings were generated.
    /// </summary>
    /// <value>True if warnings were generated, otherwise false.</value>
    public bool HasWarnings { get; private set; }

    /// <summary>
    /// Adds a warning to result.
    /// </summary>
    /// <param name="warning">The warning generated.</param>
    public void AddWarning(string warning)
    {
        HasWarnings = true;
        Warnings.Add(warning);
    }
}
