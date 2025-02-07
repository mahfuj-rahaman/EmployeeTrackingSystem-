namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Contracts.Employee
{
    public class EmployeeResponse
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Address { get; set; } = default!;
        public Guid DepartmentId { get; set; } = default!;
        public Guid DesignationId { get; set; } = default!;
    }
}
