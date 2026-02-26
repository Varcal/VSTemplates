namespace Company.Solution.Models;

public record ExampleModel(Guid Id, string Name, string Description, DateTime CreatedAt);
public record CreateExampleRequest(string Name, string Description);
public record UpdateExampleRequest(string Name, string Description);
