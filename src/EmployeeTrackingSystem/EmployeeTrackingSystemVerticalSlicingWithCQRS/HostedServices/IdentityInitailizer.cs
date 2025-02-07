using Microsoft.AspNetCore.Identity;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.HostedServices
{
    public class IdentityInitailizer
    {
        private string[] roles = new string[] { "Super Admin", "Admin", "User" };
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public IdentityInitailizer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public async Task SeedAsync()
        {
            if (roles?.Count() > 0)
            {
                foreach (var role in roles)
                {
                    if (_roleManager != null)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            await this.CreateRoleAsync(role);
                        }
                    }

                }
                await CreateUserAsync();
            }
        }
        private async Task CreateRoleAsync(string role)
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }
        private async Task CreateUserAsync()
        {
            var user = new IdentityUser { UserName = "SuperAdmin" };
            await _userManager.CreateAsync(user, "@Open1234");
            await _userManager.AddToRolesAsync(user, new string[] { "Super Admin" });
        }
    }
}
