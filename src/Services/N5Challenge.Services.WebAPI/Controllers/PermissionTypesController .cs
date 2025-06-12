using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5Challenge.Application.UseCases.PermissionType.Queries.GetAllPermissionsTypes;

namespace N5Challenge.Services.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPermissionsTypes()
    {
        var result = await _mediator.Send(new GetAllPermissionsTypesQuery());
        return Ok(result);
    }
}