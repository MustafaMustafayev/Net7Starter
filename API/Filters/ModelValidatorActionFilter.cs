using CORE.Localization;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Filters;

public class ModelValidatorActionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;

        var result = new ErrorDataResult<ModelStateDictionary>(context.ModelState, Messages.InvalidModel.Translate());
        context.Result = new BadRequestObjectResult(result);
    }
}