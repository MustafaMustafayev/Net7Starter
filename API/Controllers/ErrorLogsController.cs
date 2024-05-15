using API.Attributes;
using API.Filters;
using BLL.Abstract;
using DTO.ErrorLog;
using DTO.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class ErrorLogsController : Controller
{
    private readonly IErrorLogService _errorLogService;
    public ErrorLogsController(IErrorLogService errorLogService)
    {
        _errorLogService = errorLogService;
    }

    [SwaggerOperation(Summary = "get paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<ErrorLogResponseDto>>))]
    [HttpGet("{pageIndex}/{pageSize}")]
    public async Task<IActionResult> GetAsPaginated([FromRoute] int pageIndex, int pageSize)
    {
        var response = await _errorLogService.GetAsPaginatedListAsync(pageIndex, pageSize);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<ErrorLogResponseDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _errorLogService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get data")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<ErrorLogResponseDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _errorLogService.GetAsync(id);
        return Ok(response);
    }
}