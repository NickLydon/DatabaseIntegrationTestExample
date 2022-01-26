namespace IntegrationTests.Tests;

using System.Threading.Tasks;
using IntegrationTests.Tests.Setup;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ExampleIntegrationTest : DatabaseTestBase
{
    public ExampleIntegrationTest(ContainerFixture containerFixture) : base(containerFixture)
    {
    }

    [Fact]
    public async Task ShouldAddARecord()
    {
        await using var context = new ExampleContext(DbContextOptions<ExampleContext>());
        await context.ExampleEntities.AddAsync(new ExampleEntity { Name = "Hello" }, TestTimeout);
        Assert.Equal(1, await context.SaveChangesAsync(TestTimeout));
    }
    
    [Fact]
    public async Task ShouldHaveAFreshDbPerTest()
    {
        await using var context = new ExampleContext(DbContextOptions<ExampleContext>());
        Assert.False(await context.ExampleEntities.AnyAsync(TestTimeout));
    }
}