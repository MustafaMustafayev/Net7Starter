using API.Attributes;
using API.Filters;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Constants;
using CORE.Helpers;
using CORE.Localization;
using DTO.Responses;
using DTO.User;
using ENTITIES.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IUtilService _utilService;
    private readonly IAuthService _authService;
    public UserController(IUserService userService, IUtilService utilService, IAuthService authService)
    {
        _userService = userService;
        _utilService = utilService;
        _authService = authService;
    }

    [SwaggerOperation(Summary = "get users as paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet("paginate")]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> GetAsPaginated()
    {
        var response = await _userService.GetAsPaginatedListAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get users")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Get()
    {
        var response = await _userService.GetAsync();
        return Ok(response);
    }

    [ServiceFilter(typeof(LogActionFilter))]
    [SwaggerOperation(Summary = "get profile info")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfileInfo()
    {
        var userId = _utilService.GetUserIdFromToken();
        if (userId is null)
            return Unauthorized(new ErrorResult(Messages.CanNotFoundUserIdInYourAccessToken.Translate()));

        var response = await _userService.GetAsync(userId.Value);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<UserToListDto>))]
    [HttpGet("{id}")]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _userService.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Add([FromBody] UserToAddDto dto)
    {
        var response = await _userService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserToUpdateDto dto)
    {
        var response = await _userService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _userService.SoftDeleteAsync(id);
        await _authService.LogoutRemovedUserAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "upload profile file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("profile")]
    //[ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Upload (IFormFile file)
    {
        Guid userId = _utilService.GetUserIdFromToken().GetValueOrDefault() ;
        
        return await Upload(userId, file);
    }

    [SwaggerOperation(Summary = "upload profile file by user id")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("profile/{id}")]
    //[ServiceFilter(typeof(LogActionFilter))]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Upload([FromRoute] Guid id, IFormFile file)
    {
        var existFile = await _userService.GetProfileAsync(id);

        if (existFile is null || !existFile.Success)
        {
            return BadRequest(new ErrorDataResult<string>(Messages.UserIsNotExist.Translate()));
        }

        string fileName = System.IO.Path.GetFileName(file.FileName);
        string fileExtension = System.IO.Path.GetExtension(file.FileName);
        Guid fileNewName = Guid.NewGuid();

        if (!Constants.AllowedImageExtensions.Contains(fileExtension))
            return BadRequest(new ErrorDataResult<string>(Messages.ThisFileTypeIsNotAllowed.Translate()));

        var path = _utilService.GetEnvFolderPath(_utilService.GetFolderName(FileType.UserProfile));
        await FileHelper.WriteFile(file, $"{fileNewName}{fileExtension}", path);

        if (existFile!.Data is not null)
        {
            var fullPath = System.IO.Path.Combine(path, existFile!.Data);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        await _userService.AddProfileAsync(id,$"{fileNewName}{fileExtension}");       

        return Ok(new SuccessResult(Messages.Success.Translate()));
    }

    [SwaggerOperation(Summary = "upload profile file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("profile")]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> DeleteFile()
    {
        Guid userId = _utilService.GetUserIdFromToken().GetValueOrDefault();

        return await DeleteFile(userId);
    }

    [SwaggerOperation(Summary = "upload profile file")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("profile/{id}")]
    [ServiceFilter(typeof(LogActionFilter))]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFile(Guid id)
    {
        var existFile = await _userService.GetProfileAsync(id);

        if (existFile is null || !existFile.Success)
        {
            return BadRequest(new ErrorDataResult<string>(Messages.UserIsNotExist.Translate()));
        }

        if (existFile!.Data is null)
        {
            return Ok(new SuccessResult(Messages.Success.Translate()));
        }

        var path = _utilService.GetEnvFolderPath(_utilService.GetFolderName(FileType.UserProfile));
        var fullPath = System.IO.Path.Combine(path, existFile!.Data);

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }

        _userService.AddProfileAsync(id, null);

        return Ok(new SuccessResult(Messages.Success.Translate()));
    }
}