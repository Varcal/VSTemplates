using MediatR;
using Company.Solution.Application.DTOs;

namespace Company.Solution.Application.Commands;

public record CreateExampleCommand(string Name, string Description) : IRequest<ExampleDto>;
