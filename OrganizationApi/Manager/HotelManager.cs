using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using OrganizationApi.Accessor;
using OrganizationApi.Entities;
using OrganizationApi.Models;
using OrganizationApi.Services;

namespace OrganizationApi.Manager;

public class HotelManager
{
    private readonly UserAcessor acessor;
    private MongoService MongoService { get; set; }

    public HotelManager(MongoService mongoService, UserAcessor acessor)
    {
        MongoService = mongoService;
        this.acessor = acessor;
    }

    private IMongoCollection<Hotel> _hotelCollection => MongoService._hotelCollection;
    private IMongoCollection<Room> _roomCollection => MongoService._roomCollection;

    public async Task<HotelModel> CreateHotel(CreateHotelModel model)
    {
        var hotel = new Hotel()
        {
            Name = model.Name,
            Description = model.Description,
            Address = model.Address,
            UserId = acessor.UserId
        };

        await _hotelCollection.InsertOneAsync(hotel);
        var mod = ParseToHotelModel(hotel);
        return mod;
    }

    public async Task<List<HotelModel>> GetHotels()
    {
        var hotels = await (await _hotelCollection.FindAsync(_ => true)).ToListAsync();
        var models = new List<HotelModel>();
        foreach (var hotel in hotels)
        {
            models.Add(ParseToHotelModel(hotel));
        }
        return models;
    }

    public async Task<HotelModel> GetHotelById(Guid id)
    {
        var hotel = await (await _hotelCollection.FindAsync(p=>p.Id == id)).FirstOrDefaultAsync();
        if(hotel == null)
        {
            throw new Exception("Hotel not exist");
        }
        var model = ParseToHotelModel(hotel);
        return model;
    }

    public async Task<HotelModel> UpdateHotel(Guid id, CreateHotelModel model)
    {
        var filter = Builders<Hotel>.Filter.Eq(p=>p.Id, id);
        var hotel = await (await _hotelCollection.FindAsync(p => p.Id == id)).FirstOrDefaultAsync();
        if (hotel == null)
        {
            throw new Exception("Hotel not exist");
        }

        hotel.Name = model.Name;
        hotel.Description = model.Description;
        hotel.Address = model.Address;

        await _hotelCollection.ReplaceOneAsync(filter, hotel);

        var modell = ParseToHotelModel(hotel);

        return modell;
    }

    public async Task DeleteHotel(Guid id)
    {
        var filter = Builders<Hotel>.Filter.Eq(p => p.Id, id);
        await _hotelCollection.DeleteOneAsync(filter);
    }

    private  HotelModel ParseToHotelModel(Hotel hotel)
    {
        var model = new HotelModel()
        {
            Name = hotel.Name,
            Description = hotel.Description,
            Address = hotel.Address,
            UserId = hotel.UserId
        };

        var rooms = _roomCollection.Find(p=>p.HotelId == hotel.Id).ToList();

        hotel.Rooms = rooms;

        return model;
    }

    public async Task<HotelModel> GetHotelByName(string name)
    {
        var hotel = await (await _hotelCollection.FindAsync(p => p.Name.Contains(name))).FirstOrDefaultAsync();
        if (hotel == null)
        {
            throw new Exception("Hotel not exist");
        }
        var model = ParseToHotelModel(hotel);
        return model;
    }
}
