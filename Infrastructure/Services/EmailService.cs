using Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string userName)
    {
        var emailMessage = new MimeMessage();
        
        // Use configuration for sender email, or fallback to a dummy for dev if missing
        var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "no-reply@talentoplus.com";
        var fromName = _configuration["EmailSettings:FromName"] ?? "TalentoPlus RRHH";

        emailMessage.From.Add(new MailboxAddress(fromName, fromEmail));
        emailMessage.To.Add(new MailboxAddress(userName, toEmail));
        emailMessage.Subject = "Bienvenido a TalentoPlus S.A.S.";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <h1>¡Bienvenido, {userName}!</h1>
                <p>Tu registro en la plataforma de gestión de empleados de TalentoPlus ha sido exitoso.</p>
                <p>Ya puedes ingresar al sistema con tus credenciales.</p>
                <br>
                <p>Atentamente,</p>
                <p>El equipo de Recursos Humanos</p>"
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            // Use configuration for SMTP settings
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var smtpPass = _configuration["EmailSettings:SmtpPass"];

            try 
            {
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                
                if (!string.IsNullOrEmpty(smtpUser) && !string.IsNullOrEmpty(smtpPass))
                {
                    await client.AuthenticateAsync(smtpUser, smtpPass);
                }

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log error (Console for now)
                Console.WriteLine($"Error sending email: {ex.Message}");
                // In production, you might want to throw or handle gracefully
            }
        }
    }
}
