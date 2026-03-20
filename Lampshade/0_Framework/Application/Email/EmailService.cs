using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace _0_Framework.Application.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string title, string messageBody, string destination)
    {
        var message = new MimeMessage();
        var settings = _configuration.GetSection("EmailSettings");
        var from = new MailboxAddress(settings["SenderName"], settings["SenderAddress"]);
        message.From.Add(from);

        var to = new MailboxAddress("User", destination);
        message.To.Add(to);

        message.Subject = title;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"<h1>{messageBody}</h1>"
        };

        message.Body = bodyBuilder.ToMessageBody();

        var client = new SmtpClient();
        client.Connect(settings["Host"], int.Parse(settings["HostPort"]), SecureSocketOptions.StartTls);
        client.Authenticate(settings["Username"], settings["Password"]);
        client.Send(message);
        client.Disconnect(true);
        client.Dispose();
    }
}