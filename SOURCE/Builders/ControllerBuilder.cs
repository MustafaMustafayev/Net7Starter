using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class ControllerBuilder : ISourceBuilder, ITextBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.ControllerPath, $"{model.Name}Controller.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = @"
using API.ActionFilters;
using API.Attributes;
using BLL.Abstract;
using DTO.Responses;
using DTO.{entityName};
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Route(""api/[controller]"")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class {entityName}Controller : Controller
{
    private readonly I{entityName}Service _{entityNameLower}Service;
    public {entityName}Controller(I{entityName}Service {entityNameLower}Service)
    {
        _{entityNameLower}Service = {entityNameLower}Service;
    }

    [SwaggerOperation(Summary = ""get paginated list"")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<{entityName}ToListDto>>))]
    [HttpGet(""paginate"")]
    public async Task<IActionResult> GetAsPaginated()
    {
        var response = await _{entityNameLower}Service.GetAsPaginatedListAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = ""get list"")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<{entityName}ToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _{entityNameLower}Service.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = ""get data"")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<{entityName}ToListDto>))]
    [HttpGet(""{id}"")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var response = await _{entityNameLower}Service.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = ""create"")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] {entityName}ToAddDto dto)
    {
        var response = await _{entityNameLower}Service.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = ""update"")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut(""{id}"")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] {entityName}ToUpdateDto dto)
    {
        var response = await _{entityNameLower}Service.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = ""delete"")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete(""{id}"")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _{entityNameLower}Service.SoftDeleteAsync(id);
        return Ok(response);
    }
}
";

        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityNameLower}", entity.Name.FirstCharToLowerCase());
        return text;
    }
}