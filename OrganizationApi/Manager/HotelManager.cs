using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using OrganizationApi.Accessor;
using OrganizationApi.Entities;
using OrganizationApi.Models;
using OrganizationApi.Services;

namespace OrganizationApi.Manager;

public class HotelManager
{
    private MongoService MongoService { get; set; }

    public HotelManager(MongoService mongoService)
    {
        MongoService = mongoService;
    }

    private IMongoCollection<Hotel> _hotelCollection => MongoService._hotelCollection;

    public async Task<Hotel> CreateHotel(CreateHotelModel model)
    {
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
            throw new Exception("Hotel not exist");
        }
        return hotel;
    }

    public async Task<Hotel> UpdateHotel(Guid id, CreateHotelModel model)
    {
        var filter = Builders<Hotel>.Filter.Eq(p=>p.Id, id);
        var hotel = await (await _hotelCollection.FindAsync(p => p.Id == id)).FirstOrDefaultAsync();
        if (hotel == null)
        {
            throw new Exception("Hotel not exist");
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
