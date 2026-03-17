using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace backend.Tests.Shared.Fixtures;

/// <summary>
/// Integration tests for PATCH /api/todos/{id}/toggle-status?isDone={bool}
/// </summary>
public class ToggleStatusTests : BaseIntegrationTest
{
    public ToggleStatusTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    // ── Helper ─────────────────────────────────────────────────────────

    private async Task<int> SeedTodoAsync(bool completed = false)
    {
        var payload = new { Text = "Test task", Description = "For toggle tests" };
        var response = await Client.PostAsJsonAsync("/api/todos", payload);
        response.EnsureSuccessStatusCode();

        // If we need it pre-completed, toggle it first
        if (completed)
        {
            var created = await response.Content.ReadFromJsonAsync<CreatedResponse>();
            var toggleResponse = await Client.PatchAsync(
                $"/api/todos/{created!.Id}/toggle-status?isDone=true", null);
            toggleResponse.EnsureSuccessStatusCode();
            return created.Id;
        }

        var result = await response.Content.ReadFromJsonAsync<CreatedResponse>();
        return result!.Id;
    }

    // ── Happy path ─────────────────────────────────────────────────────

    [Fact]
    public async Task ToggleStatus_MarkAsDone_ReturnsOkWithUpdatedTodo()
    {
        // Arrange
        var id = await SeedTodoAsync(completed: false);

        // Act
        var response = await Client.PatchAsync(
            $"/api/todos/{id}/toggle-status?isDone=true", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await response.Content.ReadFromJsonAsync<TodoResponse>();
        todo.Should().NotBeNull();
        todo!.Id.Should().Be(id);
        todo.Completed.Should().BeTrue();
    }

    [Fact]
    public async Task ToggleStatus_MarkAsNotDone_ReturnsOkWithUpdatedTodo()
    {
        // Arrange — seed a completed task
        var id = await SeedTodoAsync(completed: true);

        // Act
        var response = await Client.PatchAsync(
            $"/api/todos/{id}/toggle-status?isDone=false", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await response.Content.ReadFromJsonAsync<TodoResponse>();
        todo!.Completed.Should().BeFalse();
    }

    // ── Not found ──────────────────────────────────────────────────────

    [Fact]
    public async Task ToggleStatus_NonExistentId_Returns404()
    {
        // Act
        var response = await Client.PatchAsync(
            "/api/todos/99999/toggle-status?isDone=true", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Conflict (already in requested status) ─────────────────────────

    [Fact]
    public async Task ToggleStatus_AlreadyDone_Returns409Conflict()
    {
        // Arrange — seed a completed task
        var id = await SeedTodoAsync(completed: true);

        // Act — try to mark as done again
        var response = await Client.PatchAsync(
            $"/api/todos/{id}/toggle-status?isDone=true", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ToggleStatus_AlreadyNotDone_Returns409Conflict()
    {
        // Arrange — seed a not-completed task
        var id = await SeedTodoAsync(completed: false);

        // Act — try to mark as not done again
        var response = await Client.PatchAsync(
            $"/api/todos/{id}/toggle-status?isDone=false", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ── DTOs for deserialization ────────────────────────────────────────

    private sealed class CreatedResponse
    {
        public int Id { get; set; }
    }

    private sealed class TodoResponse
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? Description { get; set; }
        public bool Completed { get; set; }
    }
}
