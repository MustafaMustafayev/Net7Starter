using API.ActionFilters;
using BLL.MediatR.OrganizationCQRS.Commands;
using BLL.MediatR.OrganizationCQRS.Queries;
using DTO.Organization;
using DTO.Responses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[AllowAnonymous]
public class OrganizationController : Controller
{
    private readonly IMediator _mediator;

    public OrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "get organizations")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<OrganizationToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new GetOrganizationListQuery()));
    }

    [SwaggerOperation(Summary = "get organization")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<OrganizationToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        return Ok(await _mediator.Send(new GetOrganizationByIdQuery(id)));
    }

    [SwaggerOperation(Summary = "create organization")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] OrganizationToAddDto request)
    {
        return Ok(await _mediator.Send(new AddOrganizationCommand(request)));
    }

    [SwaggerOperation(Summary = "update organization")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OrganizationToUpdateDto request)
    {
        return Ok(await _mediator.Send(new UpdateOrganizationCommand(request)));
    }

    [SwaggerOperation(Summary = "delete organization")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return Ok(await _mediator.Send(new DeleteOrganizationCommand(id)));
    }
}