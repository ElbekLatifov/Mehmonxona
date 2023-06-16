using System.Security.Claims;

namespace OrganizationApi.Accessor;

public class UserAcessor
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserAcessor(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string UserName => _contextAccessor.HttpContext.User.FindFirstValue(claimType: ClaimTypes.Name)!;
    public Guid UserId => Guid.Parse(_contextAccessor.HttpContext!.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)!);
}
