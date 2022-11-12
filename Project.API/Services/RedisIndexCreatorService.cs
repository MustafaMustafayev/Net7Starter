using Project.Entity.Entities.Redis;
using Redis.OM;

namespace Project.API.Services;

public class RedisIndexCreatorService : IHostedService
{
    private readonly RedisConnectionProvider _provider;

    public RedisIndexCreatorService(RedisConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _provider.Connection.CreateIndexAsync(typeof(Person));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}