using System.Security.Claims;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers
{
    public class LoggedInUserHelper 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoggedInUserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public long GetLoggedInUserId()
        {
            try
            {
                string claim = _httpContextAccessor?.HttpContext?.User?.FindFirstValue("LoggedInUserId") ?? "";
                if (!string.IsNullOrWhiteSpace(claim))
                {
                    return Convert.ToInt64(claim);
                }

                return 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string GetIdentityUserId()
        {
            try
            {
                return _httpContextAccessor?.HttpContext?.User?.FindFirstValue("AspNetUserId") ?? "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string GetLoggedInUserName()
        {
            try
            {
                return _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string GetLoggedInUserRoles()
        {
            try
            {
                var roles = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) ?? "";
                return _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) ?? "";

            }
            catch (Exception ex)
            {
                return "";
            }
        }


    }
}
