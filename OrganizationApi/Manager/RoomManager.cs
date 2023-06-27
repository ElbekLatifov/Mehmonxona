using MongoDB.Driver;
using OrganizationApi.Entities;
using OrganizationApi.Models;
using OrganizationApi.Services;
using System.Data;

namespace OrganizationApi.Manager;

public class RoomManager
{
    private MongoService _mongoService;

    public RoomManager(MongoService mongoService)
    {
        _mongoService = mongoService;
    }
    private IMongoCollection<Room> _roomCollection => _mongoService._roomCollection;


    public async Task<Room> CreateRoom(Guid hotelid, CreateRoomModel model)
    {
        var room = new Room()
        {
            Id = Guid.NewGuid(),
            Tariff = model.Description,
            Type = model.ForWho,
            IsEmpty = true,
            Volume = model.Volume,
            Number = model.Number,
            HotelId = hotelid,
            PriceOneDay = model.PriceOneDay
        };

        await _roomCollection.InsertOneAsync(room);
        return room;
    }

    public async Task<List<Room>> GetRooms()
    {
        var rooms = await(await _roomCollection.FindAsync(_ => true)).ToListAsync();

        return rooms;
    }

    public async Task<List<Room>> GetRooms(Guid hotelId)
    {
        var rooms = await(_roomCollection.Find(_=>_.HotelId == hotelId)).ToListAsync();

        return rooms;
    }

    public async Task<Room> GetRoomByNumber(Guid hotelId, int roomnumber)
    {
        var room = await (await _roomCollection.FindAsync(p =>p.HotelId == hotelId && p.Number == roomnumber)).FirstOrDefaultAsync();
        if (room == null)
        {
            throw new Exception("Takoy room not exist");
        }
        return room;
    }

    public async Task<List<Room>> GetRoomByTariff(Guid hotelid, Role role)
    {
        var rooms = await (await _roomCollection.FindAsync(p => p.HotelId == hotelid && p.Tariff == role)).ToListAsync();
        if (rooms == null)
        {
            throw new Exception("Takoy rooms not exist");
        }

        return rooms;
    }

    public async Task<List<Room>> GetRoomByTariffGroup(Role role, List<Room> roomlar)
    {
        if (roomlar == null)
        {
            throw new Exception("Rooms are null");
        }
        var rooms = roomlar.Where(p=>p.Tariff == role).ToList();
        if (rooms == null)
        {
            throw new Exception("Takoy rooms not exist");
        }

        return rooms;
    }

    public async Task<List<Room>> GetRoomByType(Guid hotelid, EClass eClass)
    {
        var rooms = await (await _roomCollection.FindAsync(p => p.HotelId == hotelid && p.Type == eClass)).ToListAsync();
        if (rooms == null)
        {
            throw new Exception("Takoy room not exist");
        }
        return rooms;
    }

    public async Task<List<Room>> GetRoomByTypeGroup(EClass eClass, List<Room> roomlar)
    {
        if (roomlar == null)
        {
            throw new Exception("Rooms are null");
        }
        var rooms = roomlar.FindAll(p =>p.Type == eClass);
        if (rooms == null)
        {
            throw new Exception("Takoy room not exist");
        }
        return rooms;
    }

    
    public async Task DeleteRoom(Guid hotelid, Guid roomid)
    {
        var filter = Builders<Room>.Filter.Eq(p => p.Id, roomid);

        var room = await(await _roomCollection.FindAsync(p =>p.HotelId == hotelid && p.Id == roomid)).FirstOrDefaultAsync();

        await _roomCollection.DeleteOneAsync(filter);
    }
}
