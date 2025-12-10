using Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string userName)
    {
        var emailMessage = new MimeMessage();
        
        // Use configuration for sender email, or fallback to a dummy for dev if missing
        var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "no-reply@talentoplus.com";
        var fromName = _configuration["EmailSettings:FromName"] ?? "TalentoPlus RRHH";
        var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
        var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
        var smtpUser = _configuration["EmailSettings:SmtpUser"];
        var smtpPass = _configuration["EmailSettings:SmtpPass"];

        // Log configuration for debugging
        _logger.LogInformation("[EmailService] Attempting to send email to: {ToEmail}", toEmail);
        _logger.LogInformation("[EmailService] SMTP Server: {SmtpServer}:{SmtpPort}", smtpServer, smtpPort);
        _logger.LogInformation("[EmailService] SMTP User: {SmtpUser}", smtpUser);
        _logger.LogInformation("[EmailService] From: {FromName} <{FromEmail}>", fromName, fromEmail);

        if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass))
        {
            _logger.LogError("[EmailService] ERROR: SMTP credentials not configured. Email not sent.");
            return;
        }

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
            try 
            {
                _logger.LogInformation("[EmailService] Connecting to SMTP server...");
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                _logger.LogInformation("[EmailService] Connected. Authenticating...");
                
                await client.AuthenticateAsync(smtpUser, smtpPass);
                _logger.LogInformation("[EmailService] Authenticated. Sending email...");

                await client.SendAsync(emailMessage);
                _logger.LogInformation("[EmailService] Email sent successfully!");
                
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EmailService] ERROR sending email");
                // Re-throw to let caller know about the error
                throw;
            }
        }
    }
}
