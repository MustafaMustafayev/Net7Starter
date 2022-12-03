using System.Net;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.ActionFilters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class ValidateHttpAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
{
    public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext,
        CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
    {
        try
        {
            AntiForgery.Validate();
        }
        catch (Exception ex)
        {
            // log if antiforgery token is invalid

            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden
            };

            return FromResult(actionContext.Response);
        }

        return continuation();
    }

    private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
    {
        var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();

        taskCompletionSource.SetResult(result);

        return taskCompletionSource.Task;
    }
}