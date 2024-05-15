using API.Attributes;
using API.Filters;
using BLL.Abstract;
using DTO.Department;
using DTO.Responses;
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
public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;
    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [SwaggerOperation(Summary = "get paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<DepartmentResponseDto>>))]
    [HttpGet("{pageIndex}/{pageSize}")]
    public async Task<IActionResult> GetAsPaginated([FromRoute] int pageIndex, int pageSize)
    {
        var response = await _departmentService.GetAsPaginatedListAsync(pageIndex, pageSize);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<DepartmentResponseDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _departmentService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get data")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<DepartmentByIdResponseDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _departmentService.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] DepartmentCreateRequestDto dto)
    {
        var response = await _departmentService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] DepartmentUpdateRequestDto dto)
    {
        var response = await _departmentService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _departmentService.SoftDeleteAsync(id);
        return Ok(response);
    }
}