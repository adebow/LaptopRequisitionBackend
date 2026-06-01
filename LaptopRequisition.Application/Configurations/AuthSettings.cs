namespace LaptopRequisition.Application.Configurations
{
    public class AuthSettings
    {
        public int MaxFailedLoginAttempts { get; set; } = 5; // Default to 5 attempts
        public int LockoutDurationMinutes { get; set; } = 30; // Default to 30 minutes
    }
}