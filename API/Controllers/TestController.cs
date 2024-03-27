using API.Filters;
using API.Attributes;
using BLL.Abstract;
using DTO.Responses;
using DTO.Test;
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
public class TestsController : Controller
{
    private readonly ITestService _testService;
    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [SwaggerOperation(Summary = "get paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<TestToListDto>>))]
    [HttpGet("paginate")]
    public async Task<IActionResult> GetAsPaginated()
    {
        var response = await _testService.GetAsPaginatedListAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<TestToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _testService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get data")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<TestToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _testService.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] TestToAddDto dto)
    {
        var response = await _testService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TestToUpdateDto dto)
    {
        var response = await _testService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _testService.SoftDeleteAsync(id);
        return Ok(response);
    }
}
