using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OrganizationApi.Entities;

public class Hotel
{
    [BsonId]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Address Address { get; set; }
    public List<Room> Rooms { get; set; } = new List<Room>();
    public Guid? UserId { get; set; }
}

public class Address
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Phone { get; set; }
}


