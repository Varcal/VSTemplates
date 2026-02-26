using Company.Solution.Models;

namespace Company.Solution.Services;

public interface IExampleService
{
    Task<IEnumerable<ExampleModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ExampleModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ExampleModel> CreateAsync(CreateExampleRequest request, CancellationToken cancellationToken = default);
    Task<ExampleModel?> UpdateAsync(Guid id, UpdateExampleRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
