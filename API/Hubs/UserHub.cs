using System.Security.Claims;
using CORE.Config;
using CORE.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize]
[EnableCors(Constants.EnableAllCorsName)]
public class UserHub : Hub
{
    private readonly ConfigSettings _configSettings;

    public UserHub(ConfigSettings configSettings)
    {
        _configSettings = configSettings;
    }

    public async Task JoinGroup(string? optionalNotificationMessage)
    {
        var companyId = Context.User?.FindFirst(_configSettings.AuthSettings.TokenCompanyIdKey)
            ?.Value;

        if (companyId is null) return;

        await Groups.AddToGroupAsync(Context.ConnectionId, companyId);

        await Clients.Groups(companyId).SendAsync("UserJoined",
            Context.User?.FindFirst(ClaimTypes.Name)?.Value,
            optionalNotificationMessage);
    }

    public override async Task OnConnectedAsync()
    {
        //await JoinGroup();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}