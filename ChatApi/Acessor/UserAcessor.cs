using System.Security.Claims;

namespace IdentityApi.Acessor
{
    public class UserAcessor 
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserAcessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected HttpContext? HttpContext => _contextAccessor.HttpContext;

        public string UserName => HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;
        public Guid UserId => Guid.Parse(HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
