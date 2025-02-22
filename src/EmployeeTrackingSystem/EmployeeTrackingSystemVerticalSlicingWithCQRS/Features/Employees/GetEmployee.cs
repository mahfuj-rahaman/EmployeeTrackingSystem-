﻿using Asp.Versioning;
using Asp.Versioning.Builder;
using Carter;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Contracts.Employee;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbContexts;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Features.Employees
{
    public static class GetEmployee
    {
        public record Query : IRequest<Result<List<EmployeeResponse>>>;

        public class Handler : IRequestHandler<Query, Result<List<EmployeeResponse>>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<List<EmployeeResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var employee = await _context.Employees
                    .Include(e => e.Department)
                    .Include(e => e.Designation)
                    .ToListAsync();
                if (!employee.Any())
                {
                    return Result<List<EmployeeResponse>>.Failure(new List<string> { "no Employee found" });
                }
                var response = employee.Adapt<List<EmployeeResponse>>();
                return Result<List<EmployeeResponse>>.Success(response);
            }
        }
    }

    public class GetEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();
            app.MapGet("/api/v{version:apiVersion}/employees", [Authorize] async (ISender sender) =>
            {
                var query = new GetEmployee.Query();
                var result = await sender.Send(query);
                return new { Employees = result };
            }).WithName("GetEmployee").WithApiVersionSet(apiVersionSet).MapToApiVersion(1);
        }
    }
}
