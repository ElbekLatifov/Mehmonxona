using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using OrganizationApi.Accessor;
using OrganizationApi.Entities;
using OrganizationApi.Models;

namespace OrganizationApi.Manager;

public class HotelManager
{
    //private readonly UserAcessor userAcessor;

    //public HotelManager(UserAcessor userAcessor)
    //{
    //    this.userAcessor = userAcessor;
    //}

    private MongoClient MongoClient = new MongoClient("mongodb://elbek:elbek@localhost:27017");
    private IMongoDatabase Database => MongoClient.GetDatabase("hotels_db");
    private IMongoCollection<Hotel> _hotelCollection => Database.GetCollection<Hotel>("hotels");

    public async Task<Hotel> CreateHotel(CreateHotelModel model)
    {
        //var mongo = new MongoClient("mongodb://elbek:elbek@localhost:27017");

        //var database = mongo.GetDatabase("hotels_db");
        //var collection = database.GetCollection<Hotel>("hotels");

        var hotel = new Hotel()
        {
            Name = model.Name,
            Description = model.Description,
            PriceOneDay = model.PriceOneDay
        };

        if(model.Address != null) 
        {
            hotel.Address = model.Address;
        }

        await _hotelCollection.InsertOneAsync(hotel);
        return hotel;
    }

    public async Task<List<Hotel>> GetHotels()
    {
        var hotels = await _hotelCollection.FindAsync(_ => true);

        return hotels.ToList();
    }

    public async Task<Hotel> GetHotelById(Guid id)
    {
        var hotel = await (await _hotelCollection.FindAsync(p=>p.Id == id)).FirstOrDefaultAsync();
        if(hotel == null)
        {
            return null;
        }
        return hotel;
    }

    public async Task<Hotel> UpdateHotel(Guid id, CreateHotelModel model)
    {
        var filter = Builders<Hotel>.Filter.Eq(p=>p.Id, id);
        var hotel = await (await _hotelCollection.FindAsync(p => p.Id == id)).FirstOrDefaultAsync();
        if (hotel == null)
        {
            return null;
        }

        hotel.Name = model.Name;
        hotel.Description = model.Description;
        if(model.Address != null)
        {
            hotel.Address = model.Address;
        }
        hotel.PriceOneDay = model.PriceOneDay;

        await _hotelCollection.ReplaceOneAsync(filter, hotel);

        return hotel;
    }

    public async Task DeleteHotel(Guid id)
    {
        var filter = Builders<Hotel>.Filter.Eq(p => p.Id, id);
        await _hotelCollection.DeleteOneAsync(filter);
    }
}
