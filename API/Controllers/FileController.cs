using API.Attributes;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Constants;
using CORE.Helper;
using CORE.Localization;
using DTO.File;
using DTO.Responses;
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

    public FileController(IFileService fileService, IUtilService utilService, IUserService userService)
    {
        _fileService = fileService;
        _utilService = utilService;
        _userService = userService;
    }

    [SwaggerOperation(Summary = "upload file")]
    [Produces(typeof(IDataResult<string>))]
    [HttpPost("user/image")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var originalName = Path.GetFileName(file.FileName);
        var hashName = _utilService.CreateGuid();

        // validation
        if (file.Length > Constants.AllowedLength)
            return BadRequest(new ErrorDataResult<string>(Messages.FileIsLargeThan2Mb.Translate()));
        if (!Constants.AllowedFileExtensions.Contains(extension))
            return BadRequest(new ErrorDataResult<string>(Messages.ThisFileTypeIsNotAllowed.Translate()));

        // create file
        var path = _utilService.GetEnvFolderPath("files");
        await FileHelper.WriteFile(file, $"{hashName}{extension}", path);

        // add to database
        var fileId =
            await _fileService.AddAsync(new FileToAddDto(originalName, hashName, extension, file.Length, path));

        // join with model
        var userId = _utilService.GetUserIdFromToken();
        if (userId is null)
            return Unauthorized(new ErrorDataResult<string>(Messages.CanNotFoundUserIdInYourAccessToken.Translate()));

        await _userService.UpdateImageAsync(userId.Value, fileId.Data);

        return Ok(new SuccessDataResult<string>(hashName, Messages.Success.Translate()));
    }

    [SwaggerOperation(Summary = "delete file")]
    [Produces(typeof(IResult))]
    [HttpDelete("user/image")]
    public async Task<IActionResult> Delete([FromQuery] string hashName)
    {
        // validation
        if (string.IsNullOrEmpty(hashName.Trim()))
            return BadRequest(new ErrorResult(Messages.InvalidModel.Translate()));

        // get file from database
        var file = await _fileService.GetAsync(hashName);

        // delete file
        var path = _utilService.GetEnvFolderPath(Path.Combine("files", $"{hashName}{file.Data!.Extension}"));
        FileHelper.DeleteFile(path);

        // remove from database
        await _fileService.SoftDeleteAsync(hashName);

        // join with model
        var userId = _utilService.GetUserIdFromToken();
        if (userId is null)
            return Unauthorized(new ErrorDataResult<string>(Messages.CanNotFoundUserIdInYourAccessToken.Translate()));

        await _userService.UpdateImageAsync(userId.Value, null);

        return Ok(new SuccessResult(Messages.Success.Translate()));
    }


    [SwaggerOperation(Summary = "download file")]
    [Produces(typeof(void))]
    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string hashName)
    {
        // validation
        if (string.IsNullOrEmpty(hashName.Trim()))
            return BadRequest(new ErrorResult(Messages.InvalidModel.Translate()));

        // get file from database
        var file = await _fileService.GetAsync(hashName);

        // read file as stream
        var path = _utilService.GetEnvFolderPath(Path.Combine("files", $"{hashName}{file.Data!.Extension}"));
        return PhysicalFile(path, "APPLICATION/octet-stream", Path.GetFileName(hashName));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "get file")]
    [Produces(typeof(void))]
    public async Task<IActionResult> Get([FromQuery] string hashName)
    {
        // validation
        if (string.IsNullOrEmpty(hashName.Trim()))
            return BadRequest(new ErrorResult(Messages.InvalidModel.Translate()));

        // get file from database
        var file = await _fileService.GetAsync(hashName);

        // read file as stream
        var path = _utilService.GetEnvFolderPath(Path.Combine("files", $"{hashName}{file.Data!.Extension}"));
        var fileStream = System.IO.File.OpenRead(path);

        if (fileStream is null) return BadRequest(new ErrorResult(Messages.FileIsNotFound.Translate()));

        return File(fileStream, "image/png");
    }
}