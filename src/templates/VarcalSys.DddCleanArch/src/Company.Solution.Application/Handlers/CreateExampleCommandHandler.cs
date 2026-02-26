using MediatR;
using Company.Solution.Application.Commands;
using Company.Solution.Application.DTOs;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Repositories;

namespace Company.Solution.Application.Handlers;

internal sealed class CreateExampleCommandHandler(
    IRepository<ExampleAggregate> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateExampleCommand, ExampleDto>
{
    public async Task<ExampleDto> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
    {
        var example = ExampleAggregate.Create(request.Name, request.Description);

        await repository.AddAsync(example, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ExampleDto(example.Id, example.Name, example.Description, example.CreatedAt);
    }
}
