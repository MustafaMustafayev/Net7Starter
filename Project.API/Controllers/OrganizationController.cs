using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.BLL.MediatR.Commands;
using Project.BLL.MediatR.Queries;
using Project.DTO.Organization;
using Project.DTO.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

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
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<OrganizationToListDto>))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] OrganizationToAddOrUpdateDto request)
    {
        return Ok(await _mediator.Send(new AddOrganizationCommand(request)));
    }

    // [SwaggerOperation(Summary = "get organization")]
    // [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<OrganizationToListDto>))]
    // [HttpGet("{id}")]
    // public async Task<IActionResult> Get([FromRoute] int id)
    // {
    //     return Ok(await _organizationService.GetAsync(id));
    // }
    //
    // [SwaggerOperation(Summary = "create organization")]
    // [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    // [HttpPost]
    // public async Task<IActionResult> Add([FromBody] OrganizationToAddOrUpdateDto dto)
    // {
    //     return Ok(await _organizationService.AddAsync(dto));
    // }
    //
    // [SwaggerOperation(Summary = "update organization")]
    // [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    // [HttpPut]
    // public async Task<IActionResult> Update([FromBody] OrganizationToAddOrUpdateDto dto)
    // {
    //     return Ok(await _organizationService.UpdateAsync(dto));
    // }
    //
    // [SwaggerOperation(Summary = "delete organization")]
    // [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete([FromRoute] int id)
    // {
    //     await _organizationService.DeleteAsync(id);
    //     return Ok();
    // }
}