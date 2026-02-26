using MediatR;
using Company.Solution.Application.Commands;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Repositories;

namespace Company.Solution.Application.Handlers;

internal sealed class DeleteExampleCommandHandler(
    IRepository<ExampleAggregate> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteExampleCommand>
{
    public async Task Handle(DeleteExampleCommand request, CancellationToken cancellationToken)
    {
        var example = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Example {request.Id} not found.");

        repository.Remove(example);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
