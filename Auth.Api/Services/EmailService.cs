using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Auth.Api.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string name, string email, string subject, string body)
    {
        var message = new MimeMessage();
        var from = new MailboxAddress("Admin", "admin@gmail.com");
        var to = new MailboxAddress(name, email);
        
        message.From.Add(from);
        message.To.Add(to);
        
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Plain)
        {
            Text = body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync("localhost", 1025);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}

public interface IEmailService
{
    Task SendEmailAsync(string name, string email, string subject, string body);
}