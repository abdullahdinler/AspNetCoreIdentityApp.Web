using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Localizations
{
    public class LocalizationsIdentityErrorDescriber : IdentityErrorDescriber
    {
        // Burada IdentityError sınıfindaki default error mezajlarını ezereke mesajları özelleştirdik.
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName", 
                Description = $"{userName} daha önce başka bir kullanıcı tarafından alınmıştır."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {

            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $"{email} daha önce başka bir kullanıcı tarafından alınmıştır."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Şifreniz en az {length} karekter olması gerekmektedir."
            };
        }

        
    }
}
