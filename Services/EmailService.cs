using AspNetCoreIdentityApp.Core.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail)
        {
            SmtpClient smtpClinet = new()
            {
                Host = _emailSettings.Host!,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password),
                EnableSsl = true
            };
            MailMessage mailMessage = new()
            {
                From = new MailAddress(_emailSettings.Email!)
            };
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Localhost | Şifre sıfırlama linki";
            mailMessage.Body = @$"
                                <h4>Şifrenizi yenilemek iiçin aşağıdaki linke tıklayınız!</h4>
                                <p><a href='{resetPasswordEmailLink}'>Şifre yenileme link!</a></p>";
            mailMessage.IsBodyHtml = true;
             await smtpClinet.SendMailAsync(mailMessage);
        }
    }
}
