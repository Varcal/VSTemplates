using MediatR;
using Company.Solution.Application.Commands;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Repositories;

namespace Company.Solution.Application.Handlers;

internal sealed class UpdateExampleCommandHandler(
    IRepository<ExampleAggregate> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateExampleCommand>
{
    public async Task Handle(UpdateExampleCommand request, CancellationToken cancellationToken)
    {
        var example = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Example {request.Id} not found.");

        example.Update(request.Name, request.Description);
        repository.Update(example);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
