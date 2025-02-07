using EmployeeTrackingSystemVerticalSlicingWithCQRS.Models.AuthModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers
{
    public class AuthHelper
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration config;
        public AuthHelper(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        public async Task<(bool success, string message, string token)> InvokeLogin(LoginModel model)
        {
            try
            {
                var user = await this.userManager.FindByNameAsync(model.UserName.ToUpper());
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var roles = await userManager.GetRolesAsync(user);
                    string issuer = this.config["Jwt:Issuer"] ?? "";
                    string audience = this.config["Jwt:Audience"] ?? "";
                    int expiresInHours = int.Parse(this.config["Jwt:Expires"] ?? "0");
                    var secretKey = Encoding.UTF8.GetBytes(this.config["Jwt:SecretKey"] ?? "");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = issuer,
                        Audience = audience,
                        IssuedAt = DateTime.UtcNow,
                        NotBefore = DateTime.UtcNow,
                        Expires = DateTime.UtcNow.AddHours(expiresInHours),
                        Subject = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("AspNetUserId", user.Id),
                        new Claim(ClaimTypes.NameIdentifier, user.UserName),
                        new Claim(ClaimTypes.Role, string.Join(',', roles.ToArray()))
                    }
                        ),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var jwtTokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                    var token = jwtTokenHandler.WriteToken(jwtToken);

                    return (true, "login successful", token);

                }
                return (false, "invalid credentials", string.Empty);
            }
            catch (Exception ex)
            {
                return (false, "error occurred in login process", string.Empty);

            }
        }

        public async Task<(bool success, string message, TokenModel model)> InvokeLogin2(LoginModel model)
        {
            try
            {
                var user = await this.userManager.FindByNameAsync(model.UserName.ToUpper());
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var roles = await userManager.GetRolesAsync(user);
                    string issuer = this.config["Jwt:Issuer"] ?? "";
                    string audience = this.config["Jwt:Audience"] ?? "";
                    int expiresInHours = int.Parse(this.config["Jwt:Expires"] ?? "0");
                    var secretKey = Encoding.UTF8.GetBytes(this.config["Jwt:SecretKey"] ?? "");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = issuer,
                        Audience = audience,
                        IssuedAt = DateTime.UtcNow,
                        NotBefore = DateTime.UtcNow,
                        Expires = DateTime.UtcNow.AddHours(expiresInHours),
                        Subject = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("AspNetUserId", user.Id),
                        new Claim(ClaimTypes.NameIdentifier, user.UserName),
                        new Claim(ClaimTypes.Role, string.Join(',', roles.ToArray()))
                    }
                        ),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var jwtTokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                    var token = jwtTokenHandler.WriteToken(jwtToken);

                    return (true, "login successful", new TokenModel() { UserName = user.UserName, UserTypeName = string.Empty, AccessToken = token });

                }
                return (false, "invalid credentials", new TokenModel());
            }
            catch (Exception ex)
            {
                return (false, "error occurred in login process", new TokenModel());

            }
        }

    }
}
