using MediatR;

namespace Company.Solution.Application.Commands;

public record DeleteExampleCommand(Guid Id) : IRequest;
