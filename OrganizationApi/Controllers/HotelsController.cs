using Microsoft.AspNetCore.Http;
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
    public async Task<Hotel> CreateHotel(CreateHotelModel model)
    {
        return await hotelManager.CreateHotel(model);
    }

    [HttpGet]
    public async Task<List<Hotel>> GetHotels()
    {
        return await hotelManager.GetHotels();
    }

    [HttpGet("id")]
    public async Task<Hotel> GetHotelById(Guid id)
    {
        return await hotelManager.GetHotelById(id);
    }

    [HttpPut("id")]
    public async Task<Hotel> UpdateHotel(Guid id, [FromBody] CreateHotelModel hotel)
    {
        return await hotelManager.UpdateHotel(id, hotel);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await hotelManager.DeleteHotel(id);

        return Ok("Deleted");
    }
}
