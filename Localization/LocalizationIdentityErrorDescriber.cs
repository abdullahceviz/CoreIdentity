using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Localization
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName",Description =$"{userName} kullancıı adı daha önce başka bir kullancı tarafından alınmıştır" }; 
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateUserName", Description = $"{email} email daha önce başka bir kullancı tarafından alınmıştır" };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "DuplicateUserName", Description = $"Şifre 6 karakterden kısa olmamalıdır." };
        }
    }
}
