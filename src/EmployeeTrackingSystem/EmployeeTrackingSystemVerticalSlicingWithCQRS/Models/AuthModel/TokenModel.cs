namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Models.AuthModel
{
    public class TokenModel
    {
        public string UserName { get; set; } = string.Empty;
        public string UserTypeName { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;

    }
}
