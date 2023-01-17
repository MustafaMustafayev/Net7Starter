using API.Attributes;
using CORE.Constants;
using CORE.Helper;
using CORE.Localization;
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
    private readonly IWebHostEnvironment _environment;

    public FileController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [SwaggerOperation(Summary = "upload file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<string>))]
    [HttpPost("upload")]
    [ValidateToken]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        var orijinalFileName = Path.GetFileName(file.FileName);
        var fileExtension = Path.GetExtension(file.FileName);
        if (!Constants.AllowedFileExtensions.Contains(fileExtension))
            return Ok(new ErrorDataResult<string>(Messages.ForbidFileExtension.Translate()));

        if (file.Length > Constants.AllowedLength)
            return Ok(new ErrorDataResult<string>(Messages.FileIsLargeThan2Mb.Translate()));

        var path = Path.Combine(_environment.WebRootPath, "files");
        var fileName = $"{Guid.NewGuid()}-{orijinalFileName}";

        await FileHelper.WriteFile(file, fileName, path);

        return Ok(new SuccessDataResult<string>(fileName, Messages.Success.Translate()));
    }

    [SwaggerOperation(Summary = "delete file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{fileName}")]
    [ValidateToken]
    public IActionResult Delete([FromRoute] string fileName)
    {
        var filePath = Path.Combine(_environment.WebRootPath, "files", fileName);
        FileHelper.DeleteFile(filePath);

        return Ok(new SuccessResult());
    }

    [SwaggerOperation(Summary = "download file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpGet("{fileName}/download")]
    [ValidateToken]
    public IActionResult Download([FromRoute] string fileName)
    {
        var filePath = Path.Combine(_environment.WebRootPath, "files", fileName);
        return PhysicalFile(filePath, "APPLICATION/octet-stream", Path.GetFileName(fileName));
    }
}