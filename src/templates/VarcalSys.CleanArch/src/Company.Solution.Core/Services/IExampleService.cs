using Company.Solution.Core.Entities;

namespace Company.Solution.Core.Services;

public interface IExampleService
{
    Task<ExampleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExampleEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ExampleEntity> CreateAsync(string name, string description, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, string name, string description, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
