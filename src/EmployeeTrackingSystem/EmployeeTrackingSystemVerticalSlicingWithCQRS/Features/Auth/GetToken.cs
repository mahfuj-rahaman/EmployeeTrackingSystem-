using Asp.Versioning.Builder;
using Asp.Versioning;
using Carter;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Contracts.Auth;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbContexts;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Features.Employees;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Features.Auth
{
    public static class GetToken
    {
        public class Command : IRequest<Result<TokenResponse>>
        {
            public string UserName { get; set; } = default!;
            public string Password { get; set; } = default!;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<TokenResponse>>
        {
            private readonly IConfiguration config;
            private readonly UserManager<IdentityUser> userManager;
            private readonly IValidator<Command> _validator;
            public Handler(UserManager<IdentityUser> context, IValidator<Command> validator, IConfiguration config)
            {
                userManager = context;
                _validator = validator;
                this.config = config;
            }
            public async Task<Result<TokenResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    var result = Result<TokenResponse>.Failure(validationResult);
                    return result;
                }

                var user = await this.userManager.FindByNameAsync(request.UserName.ToUpper());
                if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
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

                    return Result<TokenResponse>.Success(new TokenResponse() { AccessToken=token, UserName= user.UserName, UserTypeName=string.Empty });
                }

                return Result<TokenResponse>.Failure(["user name or password dosent match"]);
            }
        }
    }

    public class GetTokenEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

            app.MapPost("/api/v{version:apiVersion}/token", async (TokenRequest request, ISender sender) =>
            {
                var command = request.Adapt<GetToken.Command>();

                var result = await sender.Send(command);
                return new { Id = result };
            }).WithApiVersionSet(apiVersionSet).MapToApiVersion(1);
        }
    }

}
