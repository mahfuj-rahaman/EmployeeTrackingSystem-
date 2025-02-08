using Carter;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbContexts;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels;
using FluentValidation;
using MediatR;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Contracts.Employee;
using Mapster;
using Asp.Versioning.Builder;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Features.Employees
{
    public static class UpdateEmployee
    {
        public class Command : IRequest<Result<Guid>>
        {
            public Guid Id { get; set; } = default!;
            public string Name { get; set; } = default!;
            public string Email { get; set; } = default!;
            public string Phone { get; set; } = default!;
            public string Address { get; set; } = default!;
            public Guid DepartmentId { get; set; } = default!;
            public Guid DesignationId { get; set; } = default!;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Phone).NotEmpty();
                RuleFor(x => x.Address).NotEmpty();
                RuleFor(x => x.DepartmentId).NotEmpty();
                RuleFor(x => x.DesignationId).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IValidator<Command> _validator;
            public Handler(ApplicationDbContext context, IValidator<Command> validator)
            {
                _context = context;
                _validator = validator;
            }
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {                  
                    return Result<Guid>.Failure(validationResult);
                }

                var employee = await _context.Employees.FindAsync(request.Id);

                if (employee==null)
                {
                    Result<Guid>.Failure(["no employee found for the provided Id"]);
                }

                employee = new Employee
                {
                    Id = request.Id,
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address,
                    DepartmentId = request.DepartmentId,
                    DesignationId = request.DesignationId
                };

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Guid>.Success(employee.Id);
            }
        }

    }

    public class UpdateEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

            app.MapPut("/api/v{version:apiVersion}/employees",[Authorize] async (CreateEmployeeRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateEmployee.Command>();

                var result = await sender.Send(command);
                return new { Id = result };
            }).WithName("UpdateEmployee").WithApiVersionSet(apiVersionSet).MapToApiVersion(1);
        }
    }
}