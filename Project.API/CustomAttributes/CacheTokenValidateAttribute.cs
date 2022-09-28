using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Project.Core.Abstract;
using Project.Core.Concrete;
using Project.Core.CustomMiddlewares.Translation;
using Project.DTO.DTOs.Responses;

namespace Project.API.CustomAttributes;

[AttributeUsage(AttributeTargets.Method)]
public class CacheTokenValidateAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.RequestServices.GetService(typeof(IUtilService)) is not UtilService utilService ||
            !utilService.IsTokenExistsInCache(context.HttpContext.Request.Headers.Authorization))
            context.Result = new UnauthorizedObjectResult(new ErrorResult(Localization.Translate(Messages.YourSessionIsClosed)));
    }
}