using WebUtilities.Interfaces;

namespace Web.Common.Providers;

public class UserProvider: IUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated is true)
        {
            return _httpContextAccessor.HttpContext.User.Identities.First().Claims.Where(x => x.Type == "Id").Select(x => x.Value).FirstOrDefault() ?? "";
        }

        return "";
    }
}