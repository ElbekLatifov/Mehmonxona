using System.ComponentModel.DataAnnotations;

namespace OrganizationApi.Entities;

public class Room
{
    [Key]
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Role Description { get; set; }
    public int Volume { get; set; }
    public bool IsEmpty { get; set; }
    public EClass ForWho { get; set; }
    public Guid HotelId { get; set; }
    public Hotel? Hotel { get; set;}
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
