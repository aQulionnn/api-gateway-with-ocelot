using Auth.Api.Dtos;
using Auth.Api.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Auth.Api.Services;

public class EmailService(IOptions<EmailSettings> emailSettings) 
    : IEmailService
{
    private readonly IOptions<EmailSettings> _emailSettings = emailSettings;
    
    public async Task SendEmailAsync(SendEmailDto request)
    {
        var message = new MimeMessage();
        var from = new MailboxAddress(_emailSettings.Value.Username, _emailSettings.Value.Address);
        var to = new MailboxAddress(request.Name, request.Email);
        
        message.From.Add(from);
        message.To.Add(to);
        
        message.Subject = request.Subject;
        message.Body = new TextPart(TextFormat.Plain)
        {
            Text = request.Body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(_emailSettings.Value.Host, _emailSettings.Value.Port);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}

public interface IEmailService
{
    Task SendEmailAsync(SendEmailDto request);
}