namespace Company.Solution.Application.DTOs;

public record ExampleDto(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt
);
