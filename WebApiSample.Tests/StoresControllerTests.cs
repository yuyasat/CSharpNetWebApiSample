
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiSample.Data;
using WebApiSample.Models;
using Xunit;

namespace WebApiSample.Tests;

public class StoresControllerTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly ApiDbContext _context;

    public StoresControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        _context.Stores.RemoveRange(_context.Stores);
        await _context.Stores.AddRangeAsync(
            new Store { Name = "Test Store 1", Address = "123 Main St" },
            new Store { Name = "Test Store 2", Address = "456 Oak Ave" }
        );
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        _context.Stores.RemoveRange(_context.Stores);
        await _context.SaveChangesAsync();
        _scope.Dispose();
    }

    [Fact]
    public async Task GetStores_ReturnsSuccessStatusCodeAndStores()
    {
        // Act
        var response = await _client.GetAsync("/api/stores");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299

        var stringResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var actualStores = JsonSerializer.Deserialize<List<Store>>(stringResponse, options);

        Assert.Equal(2, actualStores.Count);
    }
}
