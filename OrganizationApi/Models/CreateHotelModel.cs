using OrganizationApi.Entities;

namespace OrganizationApi.Models;

public class CreateHotelModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Address Address { get; set; }
}
