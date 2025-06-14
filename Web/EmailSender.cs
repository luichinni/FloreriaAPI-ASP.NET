using Microsoft.AspNetCore.Identity.UI.Services;

namespace FloreriaAPI_ASP.NET.Web;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine();
        Console.WriteLine("Email Confirmation Message");
        Console.WriteLine("--------------------------");
        Console.WriteLine($"TO: {email}");
        Console.WriteLine($"SUBJECT: {subject}");
        Console.WriteLine($"CONTENTS: {htmlMessage}");
        Console.WriteLine();

        return Task.CompletedTask;
    }
}