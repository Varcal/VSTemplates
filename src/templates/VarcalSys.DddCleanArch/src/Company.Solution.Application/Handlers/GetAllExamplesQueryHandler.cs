using MediatR;
using Company.Solution.Application.DTOs;
using Company.Solution.Application.Queries;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Repositories;

namespace Company.Solution.Application.Handlers;

internal sealed class GetAllExamplesQueryHandler(
    IRepository<ExampleAggregate> repository) : IRequestHandler<GetAllExamplesQuery, IEnumerable<ExampleDto>>
{
    public async Task<IEnumerable<ExampleDto>> Handle(GetAllExamplesQuery request, CancellationToken cancellationToken)
    {
        var examples = await repository.GetAllAsync(cancellationToken);

        return examples.Select(e => new ExampleDto(e.Id, e.Name, e.Description, e.CreatedAt));
    }
}
