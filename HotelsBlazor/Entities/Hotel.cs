namespace HotelsBlazor.Entities;

public class Hotel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Address? Address { get; set; }
    public int PriceOneDay { get; set; }
    public List<Room> Rooms { get; set; } //= new List<Room>();
    public Guid? UserId { get; set; }
    public int CountRooms { get; set; }
}

public class Address
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Phone { get; set; }
}


