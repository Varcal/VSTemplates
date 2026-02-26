using MediatR;
using Company.Solution.Application.DTOs;

namespace Company.Solution.Application.Queries;

public record GetAllExamplesQuery : IRequest<IEnumerable<ExampleDto>>;
