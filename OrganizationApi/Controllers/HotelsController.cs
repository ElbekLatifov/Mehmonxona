using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrganizationApi.Entities;
using OrganizationApi.Manager;
using OrganizationApi.Models;

namespace OrganizationApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly HotelManager hotelManager;

    public HotelsController(HotelManager hotelManager)
    {
        this.hotelManager = hotelManager;
    }

    [HttpPost]
    public async Task<HotelModel> CreateHotel(CreateHotelModel model)
    {
        return await hotelManager.CreateHotel(model);
    }

    [HttpGet("hotels")]
    public async Task<List<HotelModel>> GetHotels()
    {
        return await hotelManager.GetHotels();
    }

    [HttpGet("{id}")]
    public async Task<HotelModel> GetHotelById(Guid id)
    {
        return await hotelManager.GetHotelById(id);
    }

    [HttpGet("Name/{name}")]
    public async Task<HotelModel> GetHotelByName(string name)
    {
        return await hotelManager.GetHotelByName(name);
    }

    [HttpPut("{id}")]
    public async Task<HotelModel> UpdateHotel(Guid id, [FromBody] CreateHotelModel hotel)
    {
        return await hotelManager.UpdateHotel(id, hotel);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await hotelManager.DeleteHotel(id);

        return Ok("Deleted");
    }

    [HttpPost("hotels/{hotelId}/rooms/{roomid}/order")]
    public async Task<string> Order(Guid hotelid, Guid roomid, [FromBody] BandRoomModel model)
    {
        return await hotelManager.Buyurtma(hotelid, roomid, model);
    }

    [HttpPut("hotels/{hotelid}/rooms/{id}/update")]
    public async Task<Room> UpdateRoom(Guid hotelid, Guid id, [FromBody] CreateRoomModel model)
    {
        return await hotelManager.UpdateRoom(hotelid, id, model);
    }


}
