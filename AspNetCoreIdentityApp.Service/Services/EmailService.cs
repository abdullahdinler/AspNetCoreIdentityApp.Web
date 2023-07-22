using System.Net;
using System.Net.Mail;
using AspNetCoreIdentityApp.Core.OptionModels;
using AspNetCoreIdentityApp.Service.Services;
using Microsoft.Extensions.Options;

namespace AspNetCoreIdentityApp.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }
        // Burada mail gönderimi için gerekli ayarlar yapılır.
        public async Task ResetPasswordEmail(string toEmail, string link)
        {
            //Burada mail gönderimi için gerekli ayarlar yapılır.
            var smtpClient = new SmtpClient(); // smtp.gmail.com
            smtpClient.Host = _emailSettings.Host; // Burada gmail smtp host adresi belirlenir.
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; // Burada Network kullanıldı. Ve bu sayede gmail üzerinden mail gönderildi.
            smtpClient.UseDefaultCredentials = false; // Default credential kullanılmayacak. Ve bu şu anlama geliyor. Network üzerinden mail gönderilecek. Ve bu mail gönderimi için kullanıcı adı ve şifre gerekli.
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password); // Burada gmail kullanıcı adı ve şifresi girildi.
            smtpClient.EnableSsl = true; // Güvenli bağlantı için ssl kullanılacak.


            // Mesaj oluşturulur.
            var message = new MailMessage(); // MailMessage sınıfı ile mail gönderimi yapılır.
            message.From = new MailAddress(_emailSettings.Email); // Mail gönderen adresi belirlenir.
            message.To.Add(toEmail); // Mail alıcı adresi belirlenir.
            message.Subject = "Şifre Sıfırlama"; // Mail konusu belirlenir.
            message.IsBodyHtml = true; // Mail içeriği html formatında olacak.
            message.Body = $"<a href='{link}'>Şifrenizi sıfırlamak için tıklayınız.</a>"; // Mail içeriği belirlenir.
            await smtpClient.SendMailAsync(message); // Mail gönderilir.

        }
    }
}
