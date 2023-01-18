using System.Text;
using SOURCE.Models;

namespace SOURCE.Helpers;

public static class TextBuilder
{
    public static string BuildTextForIEntityService(Entity entity)
    {
        var text = @"
using DTO.{entityName};
using DTO.Responses;

namespace BLL.Abstract;

public interface I{entityName}Service
{
    Task<IDataResult<List<{entityName}ToListDto>>> GetAsync();

    Task<IDataResult<{entityName}ToListDto>> GetAsync(int id);

    Task<IResult> AddAsync({entityName}ToAddDto dto);

    Task<IResult> UpdateAsync(int id, {entityName}ToUpdateDto dto);

    Task<IResult> SoftDeleteAsync(int id);
}
";

        text = text.Replace("{entityName}", entity.Name);
        return text;
    }

    public static string BuildTextForEntityService(Entity entity)
    {
        var text = @"
using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Responses;
using DTO.{entityName};
using ENTITIES;

namespace BLL.Concrete;

public class {entityName}Service : I{entityName}Service
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public {entityName}Service(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> AddAsync({entityName}ToAddDto dto)
    {
        var data = _mapper.Map<{entityName}>(dto);

        await _unitOfWork.{entityName}Repository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(int id)
    {
        var data = await _unitOfWork.{entityName}Repository.GetAsync(m => m.{entityName}Id == id);

        _unitOfWork.{entityName}Repository.SoftDelete(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<List<{entityName}ToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<{entityName}ToListDto>>(await _unitOfWork.{entityName}Repository.GetListAsync());

        return new SuccessDataResult<List<{entityName}ToListDto>>(datas, Messages.Success.Translate());
    }

    public async Task<IDataResult<{entityName}ToListDto>> GetAsync(int id)
    {
        var data = _mapper.Map<{entityName}ToListDto>(await _unitOfWork.{entityName}Repository.GetAsync(m => m.{entityName}Id == id));

        return new SuccessDataResult<{entityName}ToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(int id, {entityName}ToUpdateDto dto)
    {
        var data = _mapper.Map<{entityName}>(dto);
        data.{entityName}Id = id;

        _unitOfWork.{entityName}Repository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}
";

        text = text.Replace("{entityName}", entity.Name);
        return text;
    }

    public static string BuildTextForEntityController(Entity entity)
    {
        var text = @"
using API.ActionFilters;
using API.Attributes;
using API.Enums;
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

    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<{entityName}ToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _{entityNameLower}Service.GetAsync();
        return Ok(response);
    }

    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<{entityName}ToListDto>))]
    [HttpGet(""{id}"")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var response = await _{entityNameLower}Service.GetAsync(id);
        return Ok(response);
    }

    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] {entityName}ToAddDto dto)
    {
        var response = await _{entityNameLower}Service.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut(""{id}"")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] {entityName}ToUpdateDto dto)
    {
        var response = await _{entityNameLower}Service.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete(""{id}"")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _{entityNameLower}Service.SoftDeleteAsync(id);
        return Ok(response);
    }
}
";

        text = text.Replace("{entityName}", entity.Name);
        text = text.Replace("{entityNameLower}", entity.Name.FirstCharToLowerCase());
        return text;
    }

    public static string BuildTextForIUnitOfWork(List<Entity> entities)
    {
        var properties = new StringBuilder();
        entities.ForEach(e =>
            properties.AppendLine($"    public I{e.Name}Repository {e.Name}Repository {{ get; set; }}"));

        var text = $@"
using DAL.Abstract;

namespace DAL.UnitOfWorks.Abstract;

public interface IUnitOfWork : IAsyncDisposable
{{
{properties}
    public Task CommitAsync();
}}
";


        return text;
    }

    public static string BuildTextForUnitOfWork(List<Entity> entities)
    {
        var constructorArguments = new StringBuilder();
        entities.ForEach(e =>
        {
            if (entities.Last() == e)
                constructorArguments.Append(
                    $"        I{e.Name}Repository {e.Name.FirstCharToLowerCase()}Repository");
            else
                constructorArguments.AppendLine(
                    $"        I{e.Name}Repository {e.Name.FirstCharToLowerCase()}Repository,");
        });

        var constructorSetters = new StringBuilder();
        entities.ForEach(e =>
        {
            if (entities.Last() == e)
                constructorSetters.Append(
                    $"        {e.Name}Repository = {e.Name.FirstCharToLowerCase()}Repository;");
            else
                constructorSetters.AppendLine(
                    $"        {e.Name}Repository = {e.Name.FirstCharToLowerCase()}Repository;");
        });

        var properties = new StringBuilder();
        entities.ForEach(e =>
            properties.AppendLine($"    public I{e.Name}Repository {e.Name}Repository {{ get; set; }}"));


        var text = $@"
using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.UnitOfWorks.Abstract;

namespace DAL.UnitOfWorks.Concrete;

public class UnitOfWork : IUnitOfWork
{{
    private readonly DataContext _dataContext;

    private bool _isDisposed;

    public UnitOfWork(
        DataContext dataContext,
{constructorArguments}
    )
    {{
        _dataContext = dataContext;
{constructorSetters}
    }}

{properties}
    public async Task CommitAsync()
    {{
        await _dataContext.SaveChangesAsync();
    }}

    public async ValueTask DisposeAsync()
    {{
        if (!_isDisposed)
        {{
            _isDisposed = true;
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }}
    }}

    public void Dispose()
    {{
        if (_isDisposed) return;
        _isDisposed = true;
        Dispose(true);
        GC.SuppressFinalize(this);
    }}

    protected virtual void Dispose(bool disposing)
    {{
        if (disposing) _dataContext.Dispose();
    }}

    private async ValueTask DisposeAsync(bool disposing)
    {{
        if (disposing) await _dataContext.DisposeAsync();
    }}
}}
";

        return text;
    }

    public static string BuildTextForRepository(Entity entity)
    {
        var text = @"
using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class {entityName}Repository : GenericRepository<{entityName}>, I{entityName}Repository
{
    private readonly DataContext _dataContext;

    public {entityName}Repository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}
";
        text = text.Replace("{entityName}", entity.Name);

        return text;
    }

    public static string BuildTextForIRepository(Entity entity)
    {
        var text = @"
using DAL.GenericRepositories.Abstract;
using ENTITIES.Entities;

namespace DAL.Abstract;

public interface I{entityName}Repository : IGenericRepository<{entityName}>
{
}
";
        text = text.Replace("{entityName}", entity.Name);

        return text;
    }
}