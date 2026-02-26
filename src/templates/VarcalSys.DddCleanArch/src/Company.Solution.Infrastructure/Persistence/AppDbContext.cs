using Microsoft.EntityFrameworkCore;
using Company.Solution.Application.Events;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Repositories;

namespace Company.Solution.Infrastructure.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher domainEventDispatcher) : DbContext(options), IUnitOfWork
{
    public DbSet<ExampleAggregate> Examples => Set<ExampleAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        await domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);

        return result;
    }
}
