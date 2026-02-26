using Company.Solution.Models;

namespace Company.Solution.Services;

public class ExampleService : IExampleService
{
    private readonly List<ExampleModel> _examples = [];

    public Task<IEnumerable<ExampleModel>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<ExampleModel>>(_examples);

    public Task<ExampleModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_examples.FirstOrDefault(e => e.Id == id));

    public Task<ExampleModel> CreateAsync(CreateExampleRequest request, CancellationToken cancellationToken = default)
    {
        var model = new ExampleModel(Guid.NewGuid(), request.Name, request.Description, DateTime.UtcNow);
        _examples.Add(model);
        return Task.FromResult(model);
    }

    public Task<ExampleModel?> UpdateAsync(Guid id, UpdateExampleRequest request, CancellationToken cancellationToken = default)
    {
        var index = _examples.FindIndex(e => e.Id == id);
        if (index < 0) return Task.FromResult<ExampleModel?>(null);
        var updated = _examples[index] with { Name = request.Name, Description = request.Description };
        _examples[index] = updated;
        return Task.FromResult<ExampleModel?>(updated);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var removed = _examples.RemoveAll(e => e.Id == id);
        return Task.FromResult(removed > 0);
    }
}
