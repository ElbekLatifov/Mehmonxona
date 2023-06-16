using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizationApi.Entities;
using OrganizationApi.Manager;
using OrganizationApi.Models;

namespace OrganizationApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly RoomManager roomManager;

    public RoomsController(RoomManager roomManager)
    {
        this.roomManager = roomManager;
    }

    [HttpGet("ex")]
    public List<Room> GetRooms()
    {
        return roomManager.GetRooms();
    }

    [HttpGet]
    public async Task<List<Room>> GetRooms(Guid hotelid)
    {
        return await roomManager.GetRooms(hotelid);
    }

    [HttpGet("id")]
    public async Task<Room> GetRoomById(Guid id)
    {
        return await roomManager.GetRoomById(id);
    }

    [HttpPost]
    public async Task<Room> CreateRoom(Guid id, CreateRoomModel model)
    {
        return await roomManager.CreateRoom(id, model);
    }

    [HttpPut]
    public async Task<Room> UpdateRoom(Guid hotelid, Guid id, [FromBody] CreateRoomModel model)
    {
        return await roomManager.UpdateRoom(hotelid, id, model);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRoom(Guid hotelid, Guid id)
    {
        await roomManager.DeleteRoom(hotelid, id);

        return Ok("deleted room");
    }

    [HttpPost("Buyurtma")]
    public async Task<string> Buyurtma(Guid hotelid, Guid roomid, DateTime start, DateTime end)
    {
        return await roomManager.Buyurtma(hotelid, roomid, start, end);
    }
}
