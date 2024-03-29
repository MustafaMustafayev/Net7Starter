using API.Attributes;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Constants;
using CORE.Helpers;
using CORE.Localization;
using DTO.File;
using DTO.Responses;
using ENTITIES.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;
using Path = System.IO.Path;

namespace API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class FilesController : Controller
{
    private readonly IFileService _fileService;
    private readonly IUtilService _utilService;

    public FilesController(
        IFileService fileService,
        IUtilService utilService
    )
    {
        _fileService = fileService;
        _utilService = utilService;
    }

    [SwaggerOperation(Summary = "upload file")]
    [Produces(typeof(IDataResult<string>))]
    [HttpPost]
    public async Task<IActionResult> Upload([FromBody] FileUploadRequestDto dto)
    {
        // create file
        var originalFileName = Path.GetFileName(dto.File.FileName);
        var hashFileName = Guid.NewGuid().ToString();
        var fileExtension = Path.GetExtension(dto.File.FileName);

        // check extension
        if (!Constants.AllowedFileExtensions.Contains(fileExtension))
            return BadRequest(new ErrorDataResult<string>(Messages.ThisFileTypeIsNotAllowed.Translate()));

        var path = _utilService.GetEnvFolderPath(_utilService.GetFolderName(dto.Type));
        await FileHelper.WriteFile(dto.File, $"{hashFileName}{fileExtension}", path);

        // add to database
        var result = await _fileService.AddFileAsync(
            new FileToAddDto() { 
                OriginalName = originalFileName,
                HashName = hashFileName,
                Extension = fileExtension,
                Length = dto.File.Length,
                Path = path,
                Type = dto.Type},
            dto);
        if (!result.Success) return BadRequest(new ErrorDataResult<string>(Messages.InvalidModel.Translate()));

        return Ok(new SuccessDataResult<string>(hashFileName, Messages.Success.Translate()));
    }

    [SwaggerOperation(Summary = "delete file")]
    [Produces(typeof(IResult))]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] FileRemoveRequestDto dto)
    {
        // delete file
        var path = Path.Combine(_utilService.GetEnvFolderPath(_utilService.GetFolderName(dto.Type)), dto.HashName);
        FileHelper.DeleteFile(path);

        // remove from database
        var result = await _fileService.RemoveFileAsync(dto);
        return Ok(result);
    }


    [SwaggerOperation(Summary = "download file")]
    [Produces(typeof(void))]
    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string hashName, [FromQuery] FileType type)
    {
        // get file from database
        var file = await _fileService.GetAsync(hashName);

        // read file as stream
        var path = Path.Combine(_utilService.GetEnvFolderPath(_utilService.GetFolderName(type)),
            $"{hashName}{file.Data!.Extension}");

        return PhysicalFile(path, "APPLICATION/octet-stream", Path.GetFileName(hashName));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "get file")]
    [Produces(typeof(void))]
    public async Task<IActionResult> Get([FromQuery] string hashName, [FromQuery] FileType type)
    {
        // get file from database
        var file = await _fileService.GetAsync(hashName);

        // read file as stream
        var path = Path.Combine(_utilService.GetEnvFolderPath(_utilService.GetFolderName(type)),
            $"{hashName}{file.Data!.Extension}");
        var fileStream = System.IO.File.OpenRead(path);

        if (fileStream is null) return BadRequest(new ErrorResult(Messages.FileIsNotFound.Translate()));

        return File(fileStream, "image/png");
    }
}