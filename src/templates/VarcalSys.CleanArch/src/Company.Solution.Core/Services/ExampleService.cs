using Company.Solution.Core.Entities;
using Company.Solution.Core.Exceptions;
using Company.Solution.Core.Interfaces;

namespace Company.Solution.Core.Services;

public class ExampleService(IRepository<ExampleEntity> repository, IUnitOfWork unitOfWork) : IExampleService
{
    public async Task<ExampleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await repository.GetByIdAsync(id, cancellationToken);

    public async Task<IEnumerable<ExampleEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await repository.GetAllAsync(cancellationToken);

    public async Task<ExampleEntity> CreateAsync(string name, string description, CancellationToken cancellationToken = default)
    {
        var entity = ExampleEntity.Create(name, description);
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Guid id, string name, string description, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(ExampleEntity), id);
        entity.Update(name, description);
        repository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(ExampleEntity), id);
        repository.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
