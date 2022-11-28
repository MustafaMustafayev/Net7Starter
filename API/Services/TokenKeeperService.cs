using CORE.Config;
using CORE.Constants;
using CORE.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services;

public class TokenKeeperHostedService : IHostedService, IDisposable
{
    private readonly ConfigSettings _configSettings;
    private readonly ILoggerManager _logger;
    private readonly IMemoryCache _memoryCache;
    private int _executionCounter;
    private Timer? _timer;

    public TokenKeeperHostedService(ILoggerManager logger, IMemoryCache memoryCache, ConfigSettings configSettings)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _configSettings = configSettings;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInfo("TokenKeeperHostedService started running.");
        _timer = new Timer(ClearExpiredTokensFromMemory, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _memoryCache.Remove(Constants.CacheTokensKey);
        _timer?.Change(Timeout.Infinite, 0);
        _logger.LogInfo("TokenKeeperHostedService is stopping.");

        return Task.CompletedTask;
    }

    private void ClearExpiredTokensFromMemory(object? state)
    {
        _executionCounter++;

        _memoryCache.TryGetValue(Constants.CacheTokensKey, out Dictionary<string, DateTime>? tokens);
        if (tokens is null || !tokens.Any()) return;

        foreach (var token in tokens.Where(token => DateTime.Now > token.Value)) tokens.Remove(token.Key);

        _memoryCache.Set(Constants.CacheTokensKey, tokens,
            TimeSpan.FromHours(_configSettings.AuthSettings.TokenExpirationTimeInHours));
        _logger.LogInfo($"TokenKeeperHostedService is working. Count: {_executionCounter}.");
    }
}