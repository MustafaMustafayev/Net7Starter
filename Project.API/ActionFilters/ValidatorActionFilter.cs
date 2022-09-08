using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Project.Core.Constants;
using Project.DTO.DTOs.Responses;

namespace Project.API.ActionFilters
{
	public class ValidatorActionFilter : IActionFilter
	{
		public ValidatorActionFilter()
		{
		}

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ErrorDataResult<ModelStateDictionary>(context.ModelState, Messages.InvalidModel));
            }
        }
    }
}

