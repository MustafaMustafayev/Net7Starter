using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.CustomAttributes;
using Project.BLL.Abstract;
using Project.Core.Abstract;
using Project.Core.Constants;
using Project.Core.Helper;
using Project.Core.Middlewares.Translation;
using Project.DTO.Responses;
using Project.DTO.User;
using Swashbuckle.AspNetCore.Annotations;
using IResult = Project.DTO.Responses.IResult;
using Messages = Project.Core.Middlewares.Translation.Messages;

namespace Project.API.Controllers;

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

    [SwaggerOperation(Summary = "upload user profile photo")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<string>))]
    [HttpPost("user/profile")]
    [CacheTokenValidate]
    public async Task<IActionResult> UploadUserProfilePhoto([FromForm] IFormFile file)
    {
        var orijinalFileName = Path.GetFileName(file.FileName);
        var fileExtension = Path.GetExtension(file.FileName);

        if (fileExtension is not ".png" && fileExtension is not ".jpg" && fileExtension is not ".jpeg")
            return Ok(new ErrorResult(Localization.Translate(Messages.FileIsNotImage)));

        if (file.Length > Constants.MaxProfilePhotoBytes) return Ok(new ErrorDataResult<string>(Localization.Translate(Messages.FileIsLargeThan2Mb)));

        var path = Path.Combine(_environment.WebRootPath, "user_data", "photos", "profile");
        var fileName = $"{Guid.NewGuid()}-{orijinalFileName}";

        await FileHelper.WriteFile(file, fileName, path);

        var userId = _utilService.GetUserIdFromToken(HttpContext.Request.Headers.Authorization);
        if (userId is null) return Ok(new ErrorDataResult<string>(Localization.Translate(Messages.CanNotFoundUserIdInYourAccessToken)));

        await _userService.UpdateProfilePhotoAsync(userId.Value, fileName);

        var userPhotoDto = new UserPhotoDto();

        var photoData = await FileHelper.ReadFileAsByte64(fileName, path);

        userPhotoDto.ProfilePhotoExtension = photoData.Success ? "image/" + fileExtension.Split(".")[1] : null;
        userPhotoDto.ProfilePhotoBase64 = photoData.Success ? photoData.Data : null;
        userPhotoDto.ProfilePhotoFileName = fileName;

        return Ok(new SuccessDataResult<UserPhotoDto>(userPhotoDto));
    }

    [SwaggerOperation(Summary = "delete user profile photo")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("user/profile")]
    [CacheTokenValidate]
    public async Task<IActionResult> DeleteUserPhoto()
    {
        var userId = _utilService.GetUserIdFromToken(HttpContext.Request.Headers.Authorization);
        if (userId is null) return BadRequest(new ErrorResult(Localization.Translate(Messages.CanNotFoundUserIdInYourAccessToken)));

        var user = await _userService.GetAsync(userId.Value);
        if (user.Data is null) return Ok(new ErrorResult(Localization.Translate(Messages.InvalidUserCredentials)));

        if (user.Data.ProfilePhotoFileName is null) return Ok(new SuccessResult());

        var path = Path.Combine(_environment.WebRootPath, "user_data", "photos", "profile");
        FileHelper.DeleteFile(user.Data.ProfilePhotoFileName, path);

        return Ok(await _userService.DeleteProfilePhotoAsync(userId.Value));
    }

    [SwaggerOperation(Summary = "download file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpGet("download/{fileName}")]
    public IActionResult Download([FromRoute] string fileName)
    {
        var uploads = Path.Combine(_environment.WebRootPath, "files");
        var filePath = uploads + "/" + fileName;
        return PhysicalFile(filePath, "APPLICATION/octet-stream", Path.GetFileName(fileName));
    }
}