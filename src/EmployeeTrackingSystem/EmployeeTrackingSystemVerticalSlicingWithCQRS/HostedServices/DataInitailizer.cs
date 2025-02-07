using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbContexts;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.HostedServices
{
    public class DataInitailizer
    {
        private string[] rolesDefault = new string[] { "Super Admin", "Admin", "User" };
        private string[] designationsDefault = new string[] { "Manager", "Sr Executive", "Jr Executive" };
        private string[] departmentsDefault = new string[] { "Sales", "HR" };

        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DataInitailizer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
        }
        public async Task SeedAsync()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                if (rolesDefault?.Count() > 0)
                {
                    foreach (var role in rolesDefault)
                    {
                        if (_roleManager != null)
                        {
                            if (!await _roleManager.RoleExistsAsync(role))
                            {
                                await this.CreateRoleAsync(role);
                            }
                        }

                    }
                    if (!await _userManager.Users.AnyAsync())
                    {
                        await CreateUserAsync();
                    }
                }
            }

            if (!await (_context.Departments.AnyAsync()))
            {
                if (departmentsDefault.Count() > 0)
                {
                    foreach (var departmentName in departmentsDefault)
                    {
                        await CreateDepartmentAsync(departmentName);
                    }
                }
            }

            if (!await (_context.Designations.AnyAsync()))
            {
                if (designationsDefault?.Count() > 0)
                {
                    foreach (var designationName in designationsDefault)
                    {
                        await CreateDesignationAsync(designationName);
                    }
                }
            }
        }
        private async Task CreateDepartmentAsync(string name)
        {

            var department = _context.Departments.Where(a => a.Name.ToLower() == name).FirstOrDefault();
            if (department == null)
            {
                department = new Department();
                department.Name = name;
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
            }

        }
        private async Task CreateDesignationAsync(string name)
        {

            var designation = _context.Designations.Where(a => a.Name.ToLower() == name).FirstOrDefault();
            if (designation == null)
            {
                designation = new Designation();
                designation.Name = name;
                await _context.Designations.AddAsync(designation);
                await _context.SaveChangesAsync();
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
