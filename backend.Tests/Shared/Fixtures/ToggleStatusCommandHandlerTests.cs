using Backend.Data;
using Backend.Modules.TaskItem.Commands;
using backend.Modules.TaskItem.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Tests.Shared.Fixtures;

/// <summary>
/// Handler-level tests for <see cref="ToggleStatusCommandHandler"/>.
/// Uses the Testcontainers database fixture and verifies both the returned DTO
/// and the persisted entity (specifically UpdatedAt which is not on the DTO).
/// </summary>
public class ToggleStatusCommandHandlerTests : BaseIntegrationTest
{
    public ToggleStatusCommandHandlerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    // ── Helpers ────────────────────────────────────────────────────────

    private async Task<Todo> SeedTodoAsync(bool completed)
    {
        await using var scope = Factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var todo = new Todo
        {
            Text = "Handler test task",
            Description = "Seeded for toggle handler tests",
            Completed = completed,
            UpdatedAt = null,
        };

        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        return todo;
    }

    private ToggleStatusCommandHandler CreateHandler(IServiceScope scope)
    {
        return scope.ServiceProvider.GetRequiredService<ToggleStatusCommandHandler>();
    }

    // ── Scenario 1: undone → done ──────────────────────────────────────

    [Fact]
    public async Task Handle_ToggleUndoneToDone_ReturnsCompletedTrueAndSetsUpdatedAt()
    {
        // Arrange
        var seeded = await SeedTodoAsync(completed: false);
        var beforeToggle = DateTime.UtcNow;

        using var scope = Factory.Services.CreateScope();
        var handler = CreateHandler(scope);

        var command = new ToggleStatusCommand { Id = seeded.Id, IsDone = true };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert — returned DTO
        result.Should().NotBeNull();
        result.Id.Should().Be(seeded.Id);
        result.Completed.Should().BeTrue("we toggled from undone to done");

        // Assert — persisted UpdatedAt via a fresh DbContext
        var db = GetDbContext();
        var persisted = await db.Todos.FindAsync(seeded.Id);
        persisted.Should().NotBeNull();
        persisted!.UpdatedAt.Should().NotBeNull("UpdatedAt must be set after toggling")
            .And.BeOnOrAfter(beforeToggle, "UpdatedAt should be recent")
            .And.BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));
    }

    // ── Scenario 2: done → undone ──────────────────────────────────────

    [Fact]
    public async Task Handle_ToggleDoneToUndone_ReturnsCompletedFalseAndSetsUpdatedAt()
    {
        // Arrange
        var seeded = await SeedTodoAsync(completed: true);
        var beforeToggle = DateTime.UtcNow;

        using var scope = Factory.Services.CreateScope();
        var handler = CreateHandler(scope);

        var command = new ToggleStatusCommand { Id = seeded.Id, IsDone = false };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert — returned DTO
        result.Should().NotBeNull();
        result.Id.Should().Be(seeded.Id);
        result.Completed.Should().BeFalse("we toggled from done to undone");

        // Assert — persisted UpdatedAt via a fresh DbContext
        var db = GetDbContext();
        var persisted = await db.Todos.FindAsync(seeded.Id);
        persisted.Should().NotBeNull();
        persisted!.UpdatedAt.Should().NotBeNull("UpdatedAt must be set after toggling")
            .And.BeOnOrAfter(beforeToggle, "UpdatedAt should be recent")
            .And.BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));
    }
}
