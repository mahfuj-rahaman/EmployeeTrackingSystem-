using Asp.Versioning;
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
    public static class GetEmployeeById
    {
        public record Query(Guid Id) : IRequest<Result<EmployeeResponse>>;
        public class Handler : IRequestHandler<Query, Result<EmployeeResponse>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<EmployeeResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var employee = await _context.Employees
                    .Include(e => e.Department)
                    .Include(e => e.Designation)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                if (employee == null)
                {
                    return Result<EmployeeResponse>.Failure(new List<string> { "Employee not found" });
                }
                var response = employee.Adapt<EmployeeResponse>();
                return Result<EmployeeResponse>.Success(response);
            }
        }
    }

    public class GetEmployeeByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

            app.MapGet("/api/v{version:apiVersion}/employees/{id}",[Authorize] async (Guid id, ISender sender) =>
            {
                var query = new GetEmployeeById.Query(id);

                var result = await sender.Send(query);
                return new { Id = result };
            }).WithName("GetEmployeeById").WithApiVersionSet(apiVersionSet).MapToApiVersion(1);
        }
    }
}
