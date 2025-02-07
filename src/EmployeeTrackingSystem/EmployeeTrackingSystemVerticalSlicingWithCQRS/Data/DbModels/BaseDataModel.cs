namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels
{
    public abstract class BaseDataModel
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
