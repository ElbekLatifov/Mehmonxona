using MongoDB.Driver;
using OrganizationApi.Entities;

namespace OrganizationApi.Services;

public class ForBackground
{
    private readonly MongoService mongoService;

    public ForBackground(MongoService mongoService)
    {
        this.mongoService = mongoService;
    }

    public async Task Updated()
    {
        var rooms = await (await mongoService._roomCollection.FindAsync(p => p.IsEmpty == false)).ToListAsync();

        if (rooms is not null)
        {
            foreach (var room in rooms)
            {
                if (room.EndTime < DateTime.Now)
                {
                    room.IsEmpty = true;
                }
                var filter = Builders<Room>.Filter.Lt(p => p.EndTime, DateTime.Now);
                mongoService._roomCollection.ReplaceOne(filter, room);
            }
        }
        
    }
}
