using HotelsBlazor.Entities;

namespace HotelsBlazor.Managers;

public class HotelsManager
{
    private readonly RequestManager _manager;

    public HotelsManager(RequestManager manager)
    {
        _manager = manager;
    }

    public async Task<List<Hotel>?> GetHotels()
    {
        return await _manager.Get<List<Hotel>>("/api/hotels");
    }
}
