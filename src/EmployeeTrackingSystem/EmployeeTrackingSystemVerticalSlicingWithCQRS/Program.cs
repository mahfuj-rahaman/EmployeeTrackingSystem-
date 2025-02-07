using FluentValidation;

using Carter;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbContexts;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.HostedServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region DbContext
            Console.WriteLine(builder.Configuration.GetConnectionString("db"));
            builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("db")));
            builder.Services.AddDbContext<AuthDBContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("identity")));
            #endregion

            #region Initializers
            builder.Services.AddTransient<DataInitailizer>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            #endregion

            #region HostedServices
            builder.Services.AddHostedService<DataSeedService>();
            #endregion


            #region Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 6;
                option.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<AuthDBContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region JWT
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.SaveToken = true;
                    option.RequireHttpsMetadata = false;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                    };
                });
            #endregion

            #region Helpers


            builder.Services.AddTransient<AuthHelper>();
            builder.Services.AddTransient<LoggedInUserHelper>();

            #endregion

            var assembly = typeof(Program).Assembly;
            builder.Services.AddMediatR(config =>
                config.RegisterServicesFromAssemblies(assembly)
            );

            builder.Services.AddCarter();

            builder.Services.AddValidatorsFromAssemblies([assembly]);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();



            // Configure middleware
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Register endpoints using the extension method
            app.MapCarter();

            app.Run();
        }
    }
}
