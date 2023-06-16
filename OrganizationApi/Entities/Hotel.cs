using System.ComponentModel.DataAnnotations;

namespace OrganizationApi.Entities;

public class Hotel
{
    [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Address? Address { get; set; }
    public int PriceOneDay { get; set; }
    public List<Room> Rooms { get; set; } = new List<Room>();
    public Guid? UserId { get; set; }
    public int CountRooms
    {
        get
        {
            if(Rooms == null) return 0;
            return Rooms.Count;
        }
    }
}

public class Address
{
    public string City { get; set; }
    public string Street { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
}


