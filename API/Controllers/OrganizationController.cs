using API.ActionFilters;
using API.Attributes;
using DTO.Organization;
using DTO.Responses;
using MediatR;
using MEDIATRS.MediatR.OrganizationCQRS.Commands;
using MEDIATRS.MediatR.OrganizationCQRS.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class OrganizationController : Controller
{
    private readonly IMediator _mediator;

    public OrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "get organizations")]
    [SwaggerResponse(StatusCodes.Status200OK,
        type: typeof(IDataResult<List<OrganizationToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _mediator.Send(new GetOrganizationListQuery());
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get organization")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<OrganizationToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetOrganizationByIdQuery(id));
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create organization")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] OrganizationToAddDto request)
    {
        var response = await _mediator.Send(new AddOrganizationCommand(request));
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update organization")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OrganizationToUpdateDto request)
    {
        var response = await _mediator.Send(new UpdateOrganizationCommand(id, request));
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete organization")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _mediator.Send(new DeleteOrganizationCommand(id));
        return Ok(response);
    }
}