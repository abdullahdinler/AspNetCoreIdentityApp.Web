namespace AspNetCoreIdentityApp.Service.Services
{
    public interface IEmailService
    {
        Task ResetPasswordEmail(string toEmail, string link); // Şifre sıfırlama maili gönderir.
    }
}
