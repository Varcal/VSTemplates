using MediatR;

namespace Company.Solution.Application.Commands;

public record UpdateExampleCommand(Guid Id, string Name, string Description) : IRequest;
