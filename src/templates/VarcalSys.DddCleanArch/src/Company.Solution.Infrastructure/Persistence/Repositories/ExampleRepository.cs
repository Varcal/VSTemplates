using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Repositories;

namespace Company.Solution.Infrastructure.Persistence.Repositories;

internal sealed class ExampleRepository(AppDbContext dbContext) : IRepository<ExampleAggregate>
{
    public async Task<ExampleAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Examples.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IEnumerable<ExampleAggregate>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Examples.ToListAsync(cancellationToken);

    public async Task<IEnumerable<ExampleAggregate>> FindAsync(Expression<Func<ExampleAggregate, bool>> predicate, CancellationToken cancellationToken = default) =>
        await dbContext.Examples.Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(ExampleAggregate entity, CancellationToken cancellationToken = default) =>
        await dbContext.Examples.AddAsync(entity, cancellationToken);

    public void Update(ExampleAggregate entity) => dbContext.Examples.Update(entity);

    public void Remove(ExampleAggregate entity) => dbContext.Examples.Remove(entity);
}
