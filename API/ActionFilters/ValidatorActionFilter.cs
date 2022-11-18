using CORE.Middlewares.Translation;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.ActionFilters;

public class ValidatorActionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
            context.Result =
                new BadRequestObjectResult(
                    new ErrorDataResult<ModelStateDictionary>(context.ModelState,
                        Localization.Translate(Messages.InvalidModel)));
    }
}