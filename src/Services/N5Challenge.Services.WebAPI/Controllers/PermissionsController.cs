using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5Challenge.Application.UseCases.Permission.Commands.DeletePermission;
using N5Challenge.Application.UseCases.Permission.Commands.ModifyPermission;
using N5Challenge.Application.UseCases.Permission.Commands.RequestPermission;
using N5Challenge.Application.UseCases.Permission.Queries.GetAllPermissions;

namespace N5Challenge.Services.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ModifyPermission(int id, [FromBody] ModifyPermissionCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID de la URL no coincide con el del cuerpo");

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPermissions()
    {
        var result = await _mediator.Send(new GetAllPermissionsQuery());
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(int id)
    {
        var result = await _mediator.Send(new DeletePermissionCommand { Id = id });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}