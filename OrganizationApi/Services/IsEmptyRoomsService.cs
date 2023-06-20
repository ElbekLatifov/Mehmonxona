
using MongoDB.Driver;
using OrganizationApi.Entities;

namespace OrganizationApi.Services;

public class IsEmptyRoomsService : BackgroundService
{
    private MongoService _mongoService;

    public IsEmptyRoomsService(MongoService mongoService)
    {
        _mongoService = mongoService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new Timer(Tick, null, 0, 5000);
    }

    private void Tick(object? sender )
    {
        var hotels = _mongoService._hotelCollection.Find(_ => true).ToList();

        foreach (var hotel in hotels)
        {
            var rooms = hotel.Rooms.FindAll(p => p.IsEmpty == false);

            foreach (var room in rooms)
            {
                if (room.EndTime < DateTime.Now)
                {
                    room.IsEmpty = true;
                }
            }

            var filter = Builders<Hotel>.Filter.Eq(p => p.Id, hotel.Id);
            _mongoService._hotelCollection.ReplaceOne(filter, hotel);
        }
    }
}
