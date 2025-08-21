namespace Auth.Api.Dtos;

public class SendEmailDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}