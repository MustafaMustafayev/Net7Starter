
using API.ActionFilters;
using API.Attributes;
using BLL.Abstract;
using DTO.Responses;
using DTO.ITMIM;
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
public class ITMIMController : Controller
{
    private readonly IITMIMService _iTMIMService;
    public ITMIMController(IITMIMService iTMIMService)
    {
        _iTMIMService = iTMIMService;
    }

    [SwaggerOperation(Summary = "get paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<ITMIMToListDto>>))]
    [HttpGet("paginate")]
    public async Task<IActionResult> GetAsPaginated()
    {
        var response = await _iTMIMService.GetAsPaginatedListAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<ITMIMToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _iTMIMService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get data")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<ITMIMToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var response = await _iTMIMService.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ITMIMToAddDto dto)
    {
        var response = await _iTMIMService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ITMIMToUpdateDto dto)
    {
        var response = await _iTMIMService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _iTMIMService.SoftDeleteAsync(id);
        return Ok(response);
    }
}
