using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class ControllerBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities
            .Where(w =>
                w.Options.BuildController
                && w.Options.BuildService
                && w.Options.BuildUnitOfWork
                && w.Options.BuildRepository
                && w.Options.BuildDto)
            .ToList()
            .ForEach(model => SourceBuilder
                .Instance.AddSourceFile(
                    Constants.CONTROLLER_PATH,
                    $"{model.Name}sController.cs",
                    BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using API.Attributes;
                   using API.Filters;
                   using BLL.Abstract;
                   using DTO.Responses;
                   using DTO.{entityName};
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
                   public class {entityName}sController : Controller
                   {
                       private readonly I{entityName}Service _{entityNameLower}Service;
                       public {entityName}sController(I{entityName}Service {entityNameLower}Service)
                       {
                           _{entityNameLower}Service = {entityNameLower}Service;
                       }
                   
                       [SwaggerOperation(Summary = "get paginated list")]
                       [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<{entityName}ResponseDto>>))]
                       [HttpGet("{pageIndex}/{pageSize}")]
                       public async Task<IActionResult> GetAsPaginated([FromRoute] int pageIndex, int pageSize)
                       {
                           var response = await _{entityNameLower}Service.GetAsPaginatedListAsync(pageIndex, pageSize);
                           return Ok(response);
                       }
                   
                       [SwaggerOperation(Summary = "get list")]
                       [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<{entityName}ResponseDto>>))]
                       [HttpGet]
                       public async Task<IActionResult> Get()
                       {
                           var response = await _{entityNameLower}Service.GetAsync();
                           return Ok(response);
                       }
                   
                       [SwaggerOperation(Summary = "get data")]
                       [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<{entityName}ByIdResponseDto>))]
                       [HttpGet("{id}")]
                       public async Task<IActionResult> Get([FromRoute] Guid id)
                       {
                           var response = await _{entityNameLower}Service.GetAsync(id);
                           return Ok(response);
                       }
                   
                       [SwaggerOperation(Summary = "create")]
                       [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
                       [HttpPost]
                       public async Task<IActionResult> Add([FromBody] {entityName}CreateRequestDto dto)
                       {
                           var response = await _{entityNameLower}Service.AddAsync(dto);
                           return Ok(response);
                       }
                   
                       [SwaggerOperation(Summary = "update")]
                       [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
                       [HttpPut("{id}")]
                       public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] {entityName}UpdateRequestDto dto)
                       {
                           var response = await _{entityNameLower}Service.UpdateAsync(id, dto);
                           return Ok(response);
                       }
                   
                       [SwaggerOperation(Summary = "delete")]
                       [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
                       [HttpDelete("{id}")]
                       public async Task<IActionResult> Delete([FromRoute] Guid id)
                       {
                           var response = await _{entityNameLower}Service.SoftDeleteAsync(id);
                           return Ok(response);
                       }
                   }
                   """;

        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityNameLower}", entity.Name.FirstCharToLowerCase());
        return text;
    }
}