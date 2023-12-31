﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationApi.Entities;
using OrganizationApi.Manager;
using OrganizationApi.Models;

namespace OrganizationApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly RoomManager roomManager;

    public RoomsController(RoomManager roomManager)
    {
        this.roomManager = roomManager;
    }

    [HttpGet("Total")]
    public async Task<List<Room>> GetTotalRooms()
    {
        return await roomManager.GetRooms();
    }

    [HttpGet]
    public async Task<List<Room>> GetRooms(Guid hotelid)
    {
        return await roomManager.GetRooms(hotelid);
    }

    [HttpGet("tariff")]
    public async Task<List<Room>> GetRoomById(Guid hotelid, Role role)
    {
        return await roomManager.GetRoomByTariff(hotelid, role);
    }

    [HttpPost("tariffgroup")]
    public async Task<List<Room>> GetRoomByTariffGroup(List<Room> rooms, Role role)
    {
        return await roomManager.GetRoomByTariffGroup(role, rooms);
    }

    [HttpGet("type")]
    public async Task<List<Room>> GetRoomByType(Guid hotelid, EClass type)
    {
        return await roomManager.GetRoomByType(hotelid, type);
    }

    [HttpPost("typegroup")]
    public async Task<List<Room>> GetRoomByTypeGroup(List<Room> rooms, EClass type)
    {
        return await roomManager.GetRoomByTypeGroup(type, rooms);
    }

    [HttpPost("{id}")]
    public async Task<Room> CreateRoom(Guid id, CreateRoomModel model)
    {
        return await roomManager.CreateRoom(id, model);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRoom(Guid hotelid, Guid id)
    {
        await roomManager.DeleteRoom(hotelid, id);

        return Ok("deleted room");
    }

    [HttpGet("number")]
    public async Task<Room> GetByNumber(Guid hotelid, int number)
    {
        return await roomManager.GetRoomByNumber(hotelid, number);
    }   
}
