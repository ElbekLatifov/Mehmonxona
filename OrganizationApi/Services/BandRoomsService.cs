using OrganizationApi.Entities;

namespace OrganizationApi.Services;

public class BandRoomsService
{
    public static List<BandRoom> BandRooms { get; set; } = new List<BandRoom>();
}

public class BandRoom
{
    public Guid HotelId { get; set; }
    public Room Room { get; set; }
}
