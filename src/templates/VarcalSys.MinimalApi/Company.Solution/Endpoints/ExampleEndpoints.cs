using FluentValidation;
using Company.Solution.Models;
using Company.Solution.Services;

namespace Company.Solution.Endpoints;

public static class ExampleEndpoints
{
    public static IEndpointRouteBuilder MapExampleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/examples").WithTags("Examples");

        group.MapGet("/", async (IExampleService service, CancellationToken ct) =>
            Results.Ok(await service.GetAllAsync(ct)))
            .WithName("GetAllExamples")
            .Produces<IEnumerable<ExampleModel>>();

        group.MapGet("/{id:guid}", async (Guid id, IExampleService service, CancellationToken ct) =>
        {
            var result = await service.GetByIdAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetExampleById")
        .Produces<ExampleModel>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
            CreateExampleRequest request,
            IValidator<CreateExampleRequest> validator,
            IExampleService service,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));
                return Results.BadRequest(new ValidationErrorResponse(errors));
            }
            var result = await service.CreateAsync(request, ct);
            return Results.Created($"/api/examples/{result.Id}", result);
        })
        .WithName("CreateExample")
        .Produces<ExampleModel>(StatusCodes.Status201Created)
        .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateExampleRequest request,
            IValidator<UpdateExampleRequest> validator,
            IExampleService service,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));
                return Results.BadRequest(new ValidationErrorResponse(errors));
            }
            var result = await service.UpdateAsync(id, request, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("UpdateExample")
        .Produces<ExampleModel>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:guid}", async (Guid id, IExampleService service, CancellationToken ct) =>
        {
            var deleted = await service.DeleteAsync(id, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteExample")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
