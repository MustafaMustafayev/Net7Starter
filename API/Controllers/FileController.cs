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
public class FileController : Controller
{
    private readonly IFileService _fileService;
    private readonly IUserService _userService;
    private readonly IUtilService _utilService;

    public FileController(
        IUserService userService,
        IFileService fileService,
        IUtilService utilService
    )
    {
        _userService = userService;
        _fileService = fileService;
        _utilService = utilService;
    }

    [SwaggerOperation(Summary = "upload file")]
    [Produces(typeof(IDataResult<string>))]
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] FileType type)
    {
        // validation
        var userId = _utilService.GetUserIdFromToken();
        if (file.Length > Constants.AllowedLength)
            return BadRequest(new ErrorDataResult<string>(Messages.FileIsLargeThan2Mb.Translate()));

        switch (type)
        {
            case FileType.UserProfile when userId is null:
                return BadRequest(new ErrorDataResult<string>(Messages.CanNotFoundUserIdInYourAccessToken.Translate()));
        }

        // create file
        var originalFileName = Path.GetFileName(file.FileName);
        var hashFileName = Guid.NewGuid().ToString();
        var fileExtension = Path.GetExtension(file.FileName);

        var path = _utilService.GetEnvFolderPath(_utilService.GetFolderName(type));
        await FileHelper.WriteFile(file, $"{hashFileName}{fileExtension}", path);

        // add to database
        var fileId = await _fileService.AddAsync(new FileToAddDto(originalFileName, hashFileName, fileExtension, file.Length, path, type));

        // join with model
        switch (type)
        {
            case FileType.UserProfile:
                await _userService.AddProfileAsync(userId!.Value, fileId.Data);
                break;
            default:
                return BadRequest(new ErrorDataResult<string>(Messages.InvalidModel.Translate()));
        }

        return Ok(new SuccessDataResult<string>(hashFileName, Messages.Success.Translate()));
    }

    [SwaggerOperation(Summary = "delete file")]
    [Produces(typeof(IResult))]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string hashName, [FromQuery] FileType type)
    {
        // validation
        var userId = _utilService.GetUserIdFromToken();
        if (string.IsNullOrEmpty(hashName.Trim())) return BadRequest(new ErrorResult(Messages.InvalidModel.Translate()));

        switch (type)
        {
            case FileType.UserProfile when userId is null:
                return BadRequest(new ErrorDataResult<string>(Messages.InvalidModel.Translate()));
        }

        // delete file
        var path = Path.Combine(_utilService.GetEnvFolderPath(_utilService.GetFolderName(type)), hashName);
        FileHelper.DeleteFile(path);

        // remove from database
        await _fileService.SoftDeleteAsync(hashName);

        // join with model
        switch (type)
        {
            case FileType.UserProfile:
                await _userService.AddProfileAsync(userId!.Value, null);
                break;
            default:
                return BadRequest(new ErrorDataResult<string>(Messages.InvalidModel.Translate()));
        }

        return Ok(new SuccessResult(Messages.Success.Translate()));
    }


    [SwaggerOperation(Summary = "download file")]
    [Produces(typeof(void))]
    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string hashName, [FromQuery] FileType type)
    {
        // get file from database
        var file = await _fileService.GetAsync(hashName);

        // read file as stream
        var path = Path.Combine(_utilService.GetEnvFolderPath(_utilService.GetFolderName(type)), $"{hashName}{file.Data!.Extension}");

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
        var path = Path.Combine(_utilService.GetEnvFolderPath(_utilService.GetFolderName(type)), $"{hashName}{file.Data!.Extension}");
        var fileStream = System.IO.File.OpenRead(path);

        if (fileStream is null) return BadRequest(new ErrorResult(Messages.FileIsNotFound.Translate()));

        return File(fileStream, "image/png");
    }
}