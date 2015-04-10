using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citrus.SDK.Common;
using Citrus.SDK.Entity;

namespace Citrus.SDK
{
    public static class Session
    {
        private static OAuthToken _signUpToken;
        private static User _user;

        private static async Task GetSignupToken()
        {
            if (_signUpToken != null) return;

            RestWrapper rest = new RestWrapper();
            _signUpToken = (OAuthToken)await rest.Post<OAuthToken>(Service.SignUpToken, new SignupTokenRequest());
        }

        public static async Task<bool> SignupUser(string email, string mobile)
        {
            await GetSignupToken();
            var user = new User { Email = email, Mobile = mobile };

            RestWrapper rest = new RestWrapper();
            _user = (User)await rest.Post<User>(Service.Signup, user);

            return !string.IsNullOrEmpty(user.UserName);
        }

        public static string GetAuthToken()
        {
            return _signUpToken != null ? _signUpToken.AccessToken : string.Empty;
        }

        public static bool SigninUser()
        {
            var request = new SigninRequest();
            request.User = _user;

            RestWrapper rest = new RestWrapper();
            var result = rest.Post<object>(Service.Signin,)

            return result != null;
        }
    }
}
