using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace backend.Tests.Shared.Fixtures;

/// <summary>
/// Smoke tests that verify the Testcontainers database fixture is wired up correctly.
/// Each test starts with a clean, migrated SQL Server database.
/// </summary>
public class TodosEndpointTests : BaseIntegrationTest
{
    public TodosEndpointTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAll_OnEmptyDatabase_ReturnsOkWithEmptyList()
    {
        // Act
        var response = await Client.GetAsync("/api/todos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var todos = await response.Content.ReadFromJsonAsync<List<TodoResponse>>();
        todos.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task Create_ReturnsCreatedAndPersists()
    {
        // Arrange
        var payload = new { Text = "Integration test task", Description = "Created by Testcontainers" };

        // Act
        var createResponse = await Client.PostAsJsonAsync("/api/todos", payload);

        // Assert — HTTP 201 Created
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert — persisted in the database
        var getAllResponse = await Client.GetAsync("/api/todos");
        var todos = await getAllResponse.Content.ReadFromJsonAsync<List<TodoResponse>>();
        todos.Should().ContainSingle()
            .Which.Text.Should().Be("Integration test task");
    }

    // Simple DTO for deserialization inside tests
    private sealed class TodoResponse
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? Description { get; set; }
        public bool Completed { get; set; }
    }
}
