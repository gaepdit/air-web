using AirWeb.EfRepository.DbContext;
using AirWeb.EfRepository.DbContext.DevData;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Identity;
using AirWeb.TestData.NamedEntities;
using GaEpd.AppLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.CompilerServices;
using TestSupport.EfHelpers;

namespace EfRepositoryTests;

/// <summary>
/// <para>
/// This class can be used to create a new database for each unit test.
/// </para>
/// <para>
/// Use the <see cref="CreateRepositoryHelper"/> method to set up a Sqlite database.
/// </para>
/// <para>
/// If SQL Server-specific features need to be tested, then use <see cref="CreateSqlServerRepositoryHelper"/>.
/// </para>
/// </summary>
public sealed class RepositoryHelper : IDisposable, IAsyncDisposable
{
    public AppDbContext Context { get; private set; } = default!;

    private readonly DbContextOptions<AppDbContext> _options;
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor used by <see cref="CreateRepositoryHelper"/>.
    /// </summary>
    private RepositoryHelper()
    {
        _options = SqliteInMemory.CreateOptions<AppDbContext>(builder =>
            builder.LogTo(Console.WriteLine, [RelationalEventId.CommandExecuted]));

        _context = new AppDbContext(_options);
        _context.Database.EnsureCreated();
    }

    /// <summary>
    /// Constructor used by <see cref="CreateSqlServerRepositoryHelper"/>.
    /// </summary>
    /// <param name="callingClass">The class of the unit test method requesting the Repository Helper.</param>
    /// <param name="callingMember">The unit test method requesting the Repository Helper.</param>
    private RepositoryHelper(object callingClass, string callingMember)
    {
        _options = callingClass.CreateUniqueMethodOptions<AppDbContext>(callingMember: callingMember,
            builder: builder => builder.UseSqlServer()
                .LogTo(Console.WriteLine, [RelationalEventId.CommandExecuted]));

        _context = new AppDbContext(_options);
        _context.Database.EnsureClean();
    }

    /// <summary>
    /// Creates a new Sqlite database and returns a RepositoryHelper.
    /// </summary>
    /// <example>
    /// <para>
    /// Create an instance of a <c>RepositoryHelper</c> and a <c>Repository</c> like this:
    /// </para>
    /// <code>
    /// using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
    /// using var repository = repositoryHelper.GetOfficeRepository();
    /// </code>
    /// </example>
    /// <returns>A <see cref="RepositoryHelper"/> with an empty Sqlite database.</returns>
    public static RepositoryHelper CreateRepositoryHelper() => new();

    /// <summary>
    /// <para>
    /// Creates a SQL Server database and returns a RepositoryHelper. Use of this method requires that
    /// an "appsettings.json" exists in the project root with a connection string named "UnitTestConnection".
    /// </para>
    /// <para>
    /// (The "<c>callingClass</c>" and "<c>callingMember</c>" parameters are used to generate a unique
    /// database for each unit test method.)
    /// </para>
    /// </summary>
    /// <example>
    /// <para>
    /// Create an instance of a <c>RepositoryHelper</c> and a <c>Repository</c> like this:
    /// </para>
    /// <code>
    /// using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
    /// using var repository = repositoryHelper.GetOfficeRepository();
    /// </code>
    /// </example>
    /// <param name="callingClass">
    /// Enter "<c>this</c>". The class of the unit test method requesting the Repository Helper.
    /// </param>
    /// <param name="callingMember">
    /// Do not enter. The unit test method requesting the Repository Helper. This is filled in by the compiler.
    /// </param>
    /// <returns>A <see cref="RepositoryHelper"/> with a clean SQL Server database.</returns>
    public static RepositoryHelper CreateSqlServerRepositoryHelper(
        object callingClass, [CallerMemberName] string callingMember = "") =>
        new(callingClass, callingMember);

    /// <summary>
    /// Stops tracking all currently tracked entities.
    /// ReSharper disable once CommentTypo
    /// See https://github.com/JonPSmith/EfCore.TestSupport/wiki/Using-SQLite-in-memory-databases#1-best-approach-one-instance-and-use-changetrackerclear
    /// </summary>
    public void ClearChangeTracker() => Context.ChangeTracker.Clear();

    /// <summary>
    /// Deletes all data from the EF database table for the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity whose data is to be deleted.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public async Task ClearTableAsync<TEntity, TKey>()
        where TEntity : AuditableEntity<TKey, string>
        where TKey : IEquatable<TKey>
    {
        Context.RemoveRange(Context.Set<TEntity>());
        await Context.SaveChangesAsync();
        ClearChangeTracker();
    }

    private static void ClearAllStaticData()
    {
        FceData.ClearData();
        WorkEntryData.ClearData();
        NotificationTypeData.ClearData();
        OfficeData.ClearData();
        UserData.ClearData();
    }

    /// <summary>
    /// Seeds data and returns an instance of <see cref="FceRepository"/>.
    /// </summary>
    /// <returns>An FCE Repository.</returns>
    public FceRepository GetFceRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedIdentityData(_context);
        DbSeedDataHelpers.SeedFceData(_context);
        Context = new AppDbContext(_options);
        return new FceRepository(Context);
    }

    /// <summary>
    /// Seeds data and returns an instance of <see cref="WorkEntryRepository"/>.
    /// </summary>
    /// <returns>An WorkEntryRepository.</returns>
    public WorkEntryRepository GetWorkEntryRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedAllData(_context);
        Context = new AppDbContext(_options);
        return new WorkEntryRepository(Context);
    }

    /// <summary>
    /// Seeds data and returns an instance of <see cref="WorkEntryRepository"/>.
    /// </summary>
    /// <returns>An WorkEntryRepository.</returns>
    public ComplianceSearchRepository GetComplianceSearchRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedAllData(_context);
        Context = new AppDbContext(_options);
        return new ComplianceSearchRepository(Context);
    }

    /// <summary>
    /// Seeds data and returns an instance of <see cref="NotificationTypeRepository"/>.
    /// </summary>
    /// <returns>An NotificationTypeRepository.</returns>
    public NotificationTypeRepository GetNotificationTypeRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedNotificationTypeData(_context);
        Context = new AppDbContext(_options);
        return new NotificationTypeRepository(Context);
    }

    /// <summary>
    /// Seeds data and returns an instance of <see cref="OfficeRepository"/>.
    /// </summary>
    /// <returns>An OfficeRepository.</returns>
    public OfficeRepository GetOfficeRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedOfficeData(_context);
        DbSeedDataHelpers.SeedIdentityData(_context);
        Context = new AppDbContext(_options);
        return new OfficeRepository(Context);
    }

    public void Dispose()
    {
        _context.Dispose();
        Context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await Context.DisposeAsync();
    }
}
