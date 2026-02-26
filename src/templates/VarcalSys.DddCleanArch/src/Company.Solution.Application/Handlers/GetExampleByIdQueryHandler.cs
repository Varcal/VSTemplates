using MediatR;
using Company.Solution.Application.DTOs;
using Company.Solution.Application.Queries;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Repositories;

namespace Company.Solution.Application.Handlers;

internal sealed class GetExampleByIdQueryHandler(
    IRepository<ExampleAggregate> repository) : IRequestHandler<GetExampleByIdQuery, ExampleDto?>
{
    public async Task<ExampleDto?> Handle(GetExampleByIdQuery request, CancellationToken cancellationToken)
    {
        var example = await repository.GetByIdAsync(request.Id, cancellationToken);

        return example is null
            ? null
            : new ExampleDto(example.Id, example.Name, example.Description, example.CreatedAt);
    }
}
