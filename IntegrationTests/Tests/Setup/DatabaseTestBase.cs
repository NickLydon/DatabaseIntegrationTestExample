namespace IntegrationTests.Tests.Setup;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

public abstract class DatabaseTestBase : IAsyncLifetime, IClassFixture<ContainerFixture>
{
    protected readonly CancellationToken TestTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

    private readonly ContainerFixture containerFixture;
    private readonly string databaseName = Guid.NewGuid().ToString();

    protected DatabaseTestBase(ContainerFixture containerFixture)
    {
        this.containerFixture = containerFixture;
    }

    private string ConnectionString => containerFixture.ConnectionString.Replace(
        "Database=master",
        "Database=" + databaseName);

    public async Task InitializeAsync()
    {
        await using var context = new ExampleContext(new DbContextOptionsBuilder<ExampleContext>()
            .UseSqlServer(ConnectionString, options => options.EnableRetryOnFailure())
            .Options);
        await context.Database.MigrateAsync(TestTimeout);
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;
    
    internal DbContextOptions<T> DbContextOptions<T>()
        where T : DbContext =>
        new DbContextOptionsBuilder<T>()
            .UseSqlServer(ConnectionString, options => options.EnableRetryOnFailure())
            .Options;
}