
namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels
{
    public class Department : BaseDataModel
    {       
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        ICollection<Employee> Employees { get; set; } = default!;
    }
}
