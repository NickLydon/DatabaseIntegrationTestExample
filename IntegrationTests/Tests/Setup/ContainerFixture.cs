namespace IntegrationTests.Tests.Setup;

using System;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;
using Xunit;

public class ContainerFixture : IAsyncLifetime
{
    private static readonly Lazy<MsSqlTestcontainer> Container = new(
        () =>
            new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration { Password = Guid.NewGuid().ToString() })
                .WithCleanUp(true)
                .Build());

    private static int runningTests;

    public string ConnectionString => Container.Value.ConnectionString;

    public Task InitializeAsync()
    {
        Interlocked.Increment(ref runningTests);
        return Container.Value.StartAsync(Timeout());
    }

    public Task DisposeAsync() =>
        Interlocked.Decrement(ref runningTests) == 0
            ? Container.Value.StopAsync(Timeout())
            : Task.CompletedTask;

    private static CancellationToken Timeout() => new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token;
}