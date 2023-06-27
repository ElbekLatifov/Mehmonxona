
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrganizationApi.Entities;

namespace OrganizationApi.Services;

public class IsEmptyRoomsService : BackgroundService
{
    //private MongoService _mongoService;
    private IServiceProvider _serviceProvider;

    public IsEmptyRoomsService(IServiceProvider mongoService)
    {
        _serviceProvider = mongoService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new Timer(Tick, null, 0, 5000);
    }

    private async void Tick(object? sender )
    {
        try
        {
            using var scpe = _serviceProvider.CreateAsyncScope();

            var mongos = scpe.ServiceProvider.GetRequiredService<ForBackground>();

            await mongos.Updated();

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        
    }
}
