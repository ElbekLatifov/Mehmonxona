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
            Address = model.Address
        };

        await _hotelCollection.InsertOneAsync(hotel);
        var mod = await ParseToHotelModel(hotel);
        return mod;
    }

    public async Task<List<HotelModel>> GetHotels()
    {
        var hotels = await (await _hotelCollection.FindAsync(_ => true)).ToListAsync();
        var models = new List<HotelModel>();
        foreach (var hotel in hotels)
        {
            models.Add(await ParseToHotelModel(hotel));
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
        var model = await ParseToHotelModel(hotel);
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

        var modell = await ParseToHotelModel(hotel);

        return modell;
    }

    public async Task<Room> UpdateRoom(Guid hotelid, Guid roomid, CreateRoomModel model)
    {
        var filter = Builders<Room>.Filter.Eq(p => p.Id, roomid);

        var room = await (await _roomCollection.FindAsync(p => p.HotelId == hotelid && p.Id == roomid)).FirstOrDefaultAsync();

        if (room == null)
        {
            throw new Exception("Takoy room not exist");
        }

        room.Number = model.Number;
        room.Tariff = model.Description;
        room.Type = model.ForWho;
        room.Volume = model.Volume;
        room.PriceOneDay = model.PriceOneDay;

        await _roomCollection.ReplaceOneAsync(filter, room);

        return room;
    }


    public async Task DeleteHotel(Guid id)
    {
        var filter = Builders<Hotel>.Filter.Eq(p => p.Id, id);
        await _hotelCollection.DeleteOneAsync(filter);
    }

    private async  Task<HotelModel> ParseToHotelModel(Hotel hotel)
    {
        var model = new HotelModel()
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Description = hotel.Description,
            Address = hotel.Address
        };

        var rooms = await (await _roomCollection.FindAsync(p=>p.HotelId == hotel.Id)).ToListAsync();

        if(rooms is not null)
        {
            model.Rooms = rooms;
        }

        return model;
    }

    public async Task<HotelModel> GetHotelByName(string name)
    {
        var hotel = await (await _hotelCollection.FindAsync(p => p.Name.Contains(name))).FirstOrDefaultAsync();
        if (hotel == null)
        {
            throw new Exception("Hotel not exist");
        }
        var model = await ParseToHotelModel(hotel);
        return model;
    }

    public async Task<string> Buyurtma(Guid hotelid, Guid roomid, BandRoomModel band)
    {
        var filter = Builders<Room>.Filter.Eq(p => p.Id, roomid);

        var room = await (await _roomCollection.FindAsync(p => p.HotelId == hotelid && p.Id == roomid)).FirstOrDefaultAsync();

        if (room.EndTime > DateTime.Now)
        {
            return $"Bo'sh emas {room.StartTime} dan {room.EndTime} gacha, " +
                        $"uzr boshqa xona buyurtma qiling yoki keyinroq urinib ko'ring!";
        }
        room.IsEmpty = false;
        room.StartTime = band.StartTime;
        room.EndTime = band.EndTime;

        _roomCollection.ReplaceOne(filter, room);

        return "Buyurtma qabul qilindi";
    } 
}
