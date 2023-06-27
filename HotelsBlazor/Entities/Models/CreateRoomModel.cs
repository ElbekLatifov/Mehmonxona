using HotelsBlazor.Entities;

namespace HotelsBlazor.Entities.Models;

public class CreateRoomModel
{
    public int Number { get; set; }
    public Role Description { get; set; }
    public int Volume { get; set; }
    public EClass ForWho { get; set; }
    public int PriceOneDay { get; set; }
}
