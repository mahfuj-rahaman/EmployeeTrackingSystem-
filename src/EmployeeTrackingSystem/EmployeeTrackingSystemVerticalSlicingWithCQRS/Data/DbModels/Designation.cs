using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels
{
    public class Designation: BaseDataModel
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;

        ICollection<Employee> Employees { get; set; } = default!;
    }
}
