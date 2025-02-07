﻿using Carter;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbContexts;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels;
using FluentValidation;
using MediatR;

using EmployeeTrackingSystemVerticalSlicingWithCQRS.Helpers;
using EmployeeTrackingSystemVerticalSlicingWithCQRS.Contracts.Employee;
using Mapster;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Features.Employees
{
    public class CreateEmployee
    {
        public class Command : IRequest<Result<Guid>>
        {
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

                }
                var result = Result<Guid>.Failure(validationResult);

                var employee = new Employee
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address,
                    DepartmentId = request.DepartmentId,
                    DesignationId = request.DesignationId
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync(cancellationToken);

                //return Result<gu> employee.Id;
                return result = Result<Guid>.Success(employee.Id);
            }
        }

    }

    public class CreateEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/employees", async (CreateEmployeeRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateEmployee.Command>();

                var result = await app.ServiceProvider.GetRequiredService<IMediator>().Send(command);
                return new { Id = result };
            });
        }
    }
}