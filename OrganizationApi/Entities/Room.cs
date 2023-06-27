using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OrganizationApi.Entities;

public class Room
{
    [BsonId]
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Role Tariff { get; set; }
    public int Volume { get; set; }
    public bool IsEmpty { get; set; }
    public EClass Type { get; set; }
    public Guid HotelId { get; set; }
    public int PriceOneDay { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public enum Role
{
    Lux,
    Econom,
    Free,
    Special
}

public enum EClass
{
    Family,
    Friends,
    BrideGroom,
    Alone,
    Colleague
}
