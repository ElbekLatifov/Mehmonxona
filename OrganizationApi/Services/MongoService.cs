using MongoDB.Driver;
using OrganizationApi.Entities;

namespace OrganizationApi.Services;

public class MongoService
{
    private MongoClient MongoClient = new MongoClient("mongodb://elbek:elbek@mongo_db:27017");
    private IMongoDatabase Database => MongoClient.GetDatabase("hotels_db");
    public IMongoCollection<Hotel> _hotelCollection => Database.GetCollection<Hotel>("hotels");
    public IMongoCollection<Room> _roomCollection => Database.GetCollection<Room>("rooms");
}
