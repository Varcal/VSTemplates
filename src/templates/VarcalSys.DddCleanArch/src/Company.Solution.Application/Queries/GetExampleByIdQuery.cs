using MediatR;
using Company.Solution.Application.DTOs;

namespace Company.Solution.Application.Queries;

public record GetExampleByIdQuery(Guid Id) : IRequest<ExampleDto?>;
