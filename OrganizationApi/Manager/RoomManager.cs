using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrganizationApi.Entities;
using OrganizationApi.Models;

namespace OrganizationApi.Manager;

public class RoomManager
{
    private MongoClient MongoClient = new MongoClient("mongodb://elbek:elbek@localhost:27017");
    private IMongoDatabase Database => MongoClient.GetDatabase("hotels_db");
    private IMongoCollection<Hotel> _hotelCollection => Database.GetCollection<Hotel>("hotels");
    private IMongoCollection<Room> _roomCollection => Database.GetCollection<Room>("rooms");

    public async Task<Room> CreateRoom(Guid hotelid, CreateRoomModel model)
    {
        var filter = Builders<Hotel>.Filter.Eq(p => p.Id, hotelid);
        var hotel = (await _hotelCollection.FindAsync(x => x.Id == hotelid)).FirstOrDefault();

        if(hotel == null)
        {
            return null;
        }

        var room = new Room()
        {
            Id = Guid.NewGuid(),
            Description = model.Description,
            ForWho = model.ForWho,
            IsEmpty = true,
            Volume = model.Volume,
            Number = model.Number,
            HotelId = hotelid
        };

        hotel.Rooms.Add(room);

        await _hotelCollection.ReplaceOneAsync(filter, hotel);
        await _roomCollection.InsertOneAsync(room);
        return room;
    }

    public List<Room> GetRooms()
    {
        var rooms = _roomCollection.Find(_ => true);

        return rooms.ToList();
    }

    public async Task<List<Room>> GetRooms(Guid hotelId)
    {
        var hotel = (await _hotelCollection.FindAsync(p=>p.Id == hotelId)).FirstOrDefault();

        var rooms = hotel.Rooms.ToList();

        return rooms;
    }

    public async Task<Room> GetRoomById(Guid roomid)
    {
        var room = await (await _roomCollection.FindAsync(p => p.Id == roomid)).FirstOrDefaultAsync();
        if (room == null)
        {
            return null;
        }
        return room;
    }

    public async Task<Room> UpdateRoom(Guid hotelid, Guid roomid, CreateRoomModel model)
    {
        var filter = Builders<Room>.Filter.Eq(p => p.Id, roomid);
        var hotel = await (await _hotelCollection.FindAsync(p => p.Id == hotelid)).FirstOrDefaultAsync();
        var room = hotel.Rooms.FirstOrDefault(p=>p.Id == roomid);

        if (room == null)
        {
            return null;
        }
        var indexRoom = hotel.Rooms.IndexOf(room);
        hotel.Rooms[indexRoom] = room;

        room.Number = model.Number;
        room.Description = model.Description;
        room.ForWho = model.ForWho;
        room.Volume = model.Volume;

        await _roomCollection.ReplaceOneAsync(filter, room);

        return room;
    }

    public async Task DeleteRoom(Guid hotelid, Guid roomid)
    {
        var filter = Builders<Room>.Filter.Eq(p => p.Id, roomid);
        var hotel = await (await _hotelCollection.FindAsync(p => p.Id == hotelid)).FirstOrDefaultAsync();
        var room = hotel.Rooms.FirstOrDefault(p => p.Id == roomid);

        hotel.Rooms.Remove(room);
        await _roomCollection.DeleteOneAsync(filter);
    }

    public async Task<string> Buyurtma(Guid hotelid, Guid roomid, DateTime start, DateTime end)
    {

        var filter = Builders<Room>.Filter.Eq(p => p.Id, roomid);
        var filter2 = Builders<Hotel>.Filter.Eq(p => p.Id, hotelid);

        var hotel = await (await _hotelCollection.FindAsync(p => p.Id == hotelid)).FirstOrDefaultAsync();
        var room = hotel.Rooms.FirstOrDefault(p => p.Id == roomid);

        var indexRoom = hotel.Rooms.IndexOf(room);

        if(!room.IsEmpty)
        {
            if (DateTime.Now < room.EndTime)
            {
                return $"Bo'sh emas {room.StartTime} dan {room.EndTime} gacha, uzr boshqa xona buyurtma qiling!";
            }
            else
            {
                room.IsEmpty = true ;
                return "qayta urinib ko'ring!";
            }

        }
        room.IsEmpty = false;
        room.StartTime = start;
        room.EndTime = end;

        hotel.Rooms[indexRoom] = room;

        await _hotelCollection.ReplaceOneAsync(filter2, hotel);
        _roomCollection.ReplaceOne(filter, room);

        return "Buyurtma qabul qilindi";
    }
}
