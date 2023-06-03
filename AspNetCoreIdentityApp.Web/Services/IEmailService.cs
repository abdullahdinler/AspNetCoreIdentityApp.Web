namespace AspNetCoreIdentityApp.Web.Services
{
    public interface IEmailService
    {
        Task ResetPasswordEmail(string toEmail, string link); // Şifre sıfırlama maili gönderir.
    }
}
