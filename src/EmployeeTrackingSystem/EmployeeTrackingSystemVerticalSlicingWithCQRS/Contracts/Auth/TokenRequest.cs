﻿namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Contracts.Auth
{
    public class TokenRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
