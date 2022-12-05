using API.Attributes;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Constants;
using CORE.Helper;
using CORE.Middlewares.Translation;
using DTO.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;
using Messages = CORE.Middlewares.Translation.Messages;
using Path = System.IO.Path;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class FileController : Controller
{
    private readonly IWebHostEnvironment _environment;
    private readonly IUserService _userService;
    private readonly IUtilService _utilService;

    public FileController(IWebHostEnvironment environment, IUserService userService, IUtilService utilService)
    {
        _environment = environment;
        _userService = userService;
        _utilService = utilService;
    }

    [SwaggerOperation(Summary = "upload file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<string>))]
    [HttpPost("upload")]
    [CacheToken]
    public async Task<IActionResult> UploadUserProfilePhoto([FromForm] IFormFile file)
    {
        var orijinalFileName = Path.GetFileName(file.FileName);
        var fileExtension = Path.GetExtension(file.FileName);
        if (!Constants.AllowedFileExtensions.Contains(fileExtension))
            return Ok(new ErrorDataResult<string>(Localization.Translate(Messages.ForbidFileExtension)));

        if (file.Length > Constants.AllowedLength)
            return Ok(new ErrorDataResult<string>(Localization.Translate(Messages.FileIsLargeThan2Mb)));

        var path = Path.Combine(_environment.WebRootPath, "files");
        var fileName = $"{Guid.NewGuid()}-{orijinalFileName}";

        await FileHelper.WriteFile(file, fileName, path);

        return Ok(new SuccessDataResult<string>(fileName, Localization.Translate(Messages.Success)));
    }

    [SwaggerOperation(Summary = "delete file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{fileName}")]
    [CacheToken]
    public IActionResult DeleteUserPhoto([FromRoute] string fileName)
    {
        var filePath = Path.Combine(_environment.WebRootPath, "files", fileName);
        FileHelper.DeleteFile(filePath);

        return Ok(new SuccessResult());
    }

    [SwaggerOperation(Summary = "download file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpGet("{fileName}/download")]
    [CacheToken]
    public IActionResult Download([FromRoute] string fileName)
    {
        var filePath = Path.Combine(_environment.WebRootPath, "files", fileName);
        return PhysicalFile(filePath, "APPLICATION/octet-stream", Path.GetFileName(fileName));
    }
}