using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Company.Solution.Core.Entities;
using Company.Solution.Core.Interfaces;

namespace Company.Solution.Infrastructure.Persistence.Repositories;

internal sealed class ExampleRepository(AppDbContext dbContext) : IRepository<ExampleEntity>
{
    public async Task<ExampleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Examples.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IEnumerable<ExampleEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Examples.ToListAsync(cancellationToken);

    public async Task<IEnumerable<ExampleEntity>> FindAsync(Expression<Func<ExampleEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
        await dbContext.Examples.Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(ExampleEntity entity, CancellationToken cancellationToken = default) =>
        await dbContext.Examples.AddAsync(entity, cancellationToken);

    public void Update(ExampleEntity entity) => dbContext.Examples.Update(entity);
    public void Remove(ExampleEntity entity) => dbContext.Examples.Remove(entity);
}
