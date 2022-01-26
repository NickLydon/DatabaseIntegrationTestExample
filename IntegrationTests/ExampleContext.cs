namespace IntegrationTests;

using Microsoft.EntityFrameworkCore;

public class ExampleContext : DbContext
{
    public ExampleContext()
    {
    }
    public ExampleContext(DbContextOptions<ExampleContext> options) : base(options)
    {
    }
    
    public DbSet<ExampleEntity> ExampleEntities => Set<ExampleEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
        base.OnConfiguring(optionsBuilder);
    }
}