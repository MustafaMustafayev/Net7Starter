using System.Net;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class ValidateForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
{
    public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
        HttpActionContext actionContext,
        CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
    {
        try
        {
            AntiForgery.Validate();
        }
        catch (Exception)
        {
            // log if antiforgery token is invalid
            actionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.Forbidden };

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