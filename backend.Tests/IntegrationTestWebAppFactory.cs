using System.Data.Common;
using Backend.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;
using Testcontainers.MsSql;

namespace backend.Tests;

/// <summary>
/// Custom <see cref="WebApplicationFactory{TEntryPoint}"/> that spins up a
/// Testcontainers SQL Server instance shared across every test in the class fixture.
///
/// Lifecycle:
///   1. <see cref="InitializeAsync"/> — starts the container & applies EF migrations.
///   2. Tests run — each test can call <see cref="ResetDatabaseAsync"/> to
///      wipe data between tests via Respawn (fast checkpoint-based reset).
///   3. <see cref="DisposeAsync"/> — stops the container & cleans up the factory.
/// </summary>
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Pin the image version per Testcontainers best-practice #8
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
        .WithPassword("Test_Password_123!")
        .Build();

    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;

    /// <summary>
    /// Exposes the Testcontainers connection string so tests can open ad-hoc connections.
    /// </summary>
    public string ConnectionString => _dbContainer.GetConnectionString();

    // ── Setup ──────────────────────────────────────────────────────────────

    public async Task InitializeAsync()
    {
        // 1. Start the SQL Server container
        await _dbContainer.StartAsync();

        // 2. Apply EF Core migrations to establish the schema
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();

        // 3. Initialise Respawn so we can cheaply reset data between tests
        _dbConnection = new SqlConnection(ConnectionString);
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["dbo"],
        });
    }

    // ── Per-test reset ─────────────────────────────────────────────────────

    /// <summary>
    /// Deletes all table data while preserving the schema.
    /// Call this in the test base class constructor or setup to guarantee test isolation.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    // ── Teardown ───────────────────────────────────────────────────────────

    public new async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _dbContainer.StopAsync();
        await base.DisposeAsync();
    }

    // ── Service overrides ──────────────────────────────────────────────────

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove the production DbContext & DbConnection registrations
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<DbConnection>();

            // Register the Testcontainers connection string via UseSetting-style override
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });
        });

        builder.UseEnvironment("Testing");
    }
}
