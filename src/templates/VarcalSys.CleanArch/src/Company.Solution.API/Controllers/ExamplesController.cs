using Microsoft.AspNetCore.Mvc;
using Company.Solution.Core.Services;

namespace Company.Solution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamplesController(IExampleService exampleService) : ControllerBase
{
    public record CreateExampleRequest(string Name, string Description);
    public record UpdateExampleRequest(string Name, string Description);

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await exampleService.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await exampleService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExampleRequest request, CancellationToken cancellationToken)
    {
        var result = await exampleService.CreateAsync(request.Name, request.Description, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExampleRequest request, CancellationToken cancellationToken)
    {
        await exampleService.UpdateAsync(id, request.Name, request.Description, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await exampleService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
