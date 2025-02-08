using Carter;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Contracts.Employee;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbContexts;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Features.Employees
{
    public static class GetEmployee
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

    //public class GetEmployeeEndpoint : ICarterModule
    //{
    //    public void AddRoutes(IEndpointRouteBuilder app)
    //    {
    //        app.MapGet("/api/employees/{id}", async (CreateEmployeeRequest request, ISender sender) =>
    //        {
    //            var command = request.Adapt<CreateEmployee.Command>();

    //            var result = await app.ServiceProvider.GetRequiredService<IMediator>().Send(command);
    //            return new { Id = result };
    //        });
    //    }
    //}
}
