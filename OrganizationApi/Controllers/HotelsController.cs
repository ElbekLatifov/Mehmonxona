﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<HotelModel> CreateHotel(CreateHotelModel model)
    {
        return await hotelManager.CreateHotel(model);
    }

    [HttpGet]
    public async Task<List<HotelModel>> GetHotels()
    {
        return await hotelManager.GetHotels();
    }

    [HttpGet("id")]
    public async Task<HotelModel> GetHotelById(Guid id)
    {
        return await hotelManager.GetHotelById(id);
    }

    [HttpPut("id")]
    [Authorize]
    public async Task<HotelModel> UpdateHotel(Guid id, [FromBody] CreateHotelModel hotel)
    {
        return await hotelManager.UpdateHotel(id, hotel);
    }

    [HttpDelete("id")]
    [Authorize]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await hotelManager.DeleteHotel(id);

        return Ok("Deleted");
    }
}
