using API.Filters;
using API.Attributes;
using BLL.Abstract;
using DTO.Responses;
using DTO.ErrorLog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class ErrorLogController : Controller
{
    private readonly IErrorLogService _errorLogService;
    public ErrorLogController(IErrorLogService errorLogService)
    {
        _errorLogService = errorLogService;
    }

    [SwaggerOperation(Summary = "get paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<ErrorLogToListDto>>))]
    [HttpGet("paginate")]
    public async Task<IActionResult> GetAsPaginated()
    {
        var response = await _errorLogService.GetAsPaginatedListAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<ErrorLogToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _errorLogService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get data")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<ErrorLogToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _errorLogService.GetAsync(id);
        return Ok(response);
    }
}
