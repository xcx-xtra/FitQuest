using System.Threading.Tasks;

public class MockEmailSender : IEmailSender
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
        // Mock implementation: Log the email details to the console
        Console.WriteLine($"Sending email to {to} with subject '{subject}' and body: {body}");
        return Task.CompletedTask;
    }
}