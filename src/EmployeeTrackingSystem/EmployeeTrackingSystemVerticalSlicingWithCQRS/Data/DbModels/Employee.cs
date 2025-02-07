
namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels
{
    public class Employee : BaseDataModel
    {
        public Guid Id { get; set; } = default!;
        public string Name { get;  set; } = default!;
        public string Email { get;  set; } = default!;
        public string Phone { get;  set; } = default!;
        public string Address { get;  set; } = default!;
        public string ImageUrl { get;  set; } = default!;
        public Guid DesignationId { get;  set; } = default!;
        public Guid DepartmentId { get;  set; } = default!;
        public Designation Designation { get;  set; } = default!;
        public Department Department { get;  set; } = default!;
    }
    
}
