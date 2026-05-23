using LaptopRequisition.Application.Interfaces;
using System.Diagnostics; 

namespace LaptopRequisition.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string message)
        {
            Debug.WriteLine($"Sending email to: {toEmail}");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
            return Task.CompletedTask;
        }
    }
}