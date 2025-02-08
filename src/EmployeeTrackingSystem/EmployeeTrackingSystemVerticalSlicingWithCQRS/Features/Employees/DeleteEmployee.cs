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
    public static class DeleteEmployee
    {
        public record Query(Guid Id) : IRequest<Result<string>>;
        public class Handler : IRequestHandler<Query, Result<string>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                var employee = await _context.Employees
                    .Include(e => e.Department)
                    .Include(e => e.Designation)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                if (employee == null)
                {
                    return Result<string>.Failure(["Employee not found"]);
                }
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return Result<string>.Success($"employee record for the EmployeeId '{request.Id}' is deleted");
            }
        }
    }

    public class DeleteEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

            app.MapDelete("/api/v{version:apiVersion}/employees/{id}",  async (Guid id, ISender sender) =>
            {
                var query = new DeleteEmployee.Query(id);

                var result = await sender.Send(query);
                return new { Id = result };
            }).WithName("DeleteEmployee").WithApiVersionSet(apiVersionSet).MapToApiVersion(1);
        }
    }
}
