namespace Company.Solution.Models;

public record ValidationErrorResponse(IEnumerable<ValidationError> Errors);
public record ValidationError(string Field, string Message);
