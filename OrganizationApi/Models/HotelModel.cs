using OrganizationApi.Entities;

namespace OrganizationApi.Models
{
    public class HotelModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
        public Guid UserId { get; set; }
        public int CountRooms
        {
            get
            {
                if (Rooms == null) return 0;
                return Rooms.Count;
            }
        }
    }
}
