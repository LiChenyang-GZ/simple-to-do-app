using Backend.Data;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Tests;

/// <summary>
/// Base class for all integration tests.
///
/// • Injects the shared <see cref="IntegrationTestWebAppFactory"/> (one SQL Server container per test class).
/// • Resets the database to a clean state before every test via Respawn.
/// • Exposes a pre-configured <see cref="HttpClient"/> and a helper to resolve scoped services.
/// </summary>
[Collection("IntegrationTests")]
public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestWebAppFactory Factory;
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    /// <summary>
    /// Called before each test — resets all table data so tests are fully isolated.
    /// </summary>
    public async Task InitializeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    /// <summary>No per-test teardown required.</summary>
    public Task DisposeAsync() => Task.CompletedTask;

    /// <summary>
    /// Helper to resolve a scoped <see cref="AppDbContext"/> (or any scoped service)
    /// for direct database assertions in tests.
    /// </summary>
    protected T GetService<T>() where T : notnull
    {
        var scope = Factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    /// <summary>
    /// Helper to get a fresh <see cref="AppDbContext"/> for database verification.
    /// </summary>
    protected AppDbContext GetDbContext() => GetService<AppDbContext>();
}
