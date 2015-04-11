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
            var result = await rest.Post<OAuthToken>(Service.SignUpToken, new SignupTokenRequest());
            if (!(result is Error))
            {
                _signUpToken = (OAuthToken)result;
            }
            else
            {
                Utility.ParseAndThrowError((result as Error).Response);
            }
        }

        public static async Task<User> SignupUser(string email, string mobile)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Invalid parameter", "email");
            }

            if (string.IsNullOrEmpty(mobile))
            {
                throw new ArgumentException("Invalid parameter", "mobile");
            }

            await GetSignupToken();
            var user = new User { Email = email, Mobile = mobile };
            _user = user;
            RestWrapper rest = new RestWrapper();
            var result = await rest.Post<User>(Service.Signup, user);
            if (!(result is Error))
            {
                _user = (User)result;
            }
            else
            {
                Utility.ParseAndThrowError((result as Error).Response);
            }

            if (!string.IsNullOrEmpty(_user.UserName))
            {
                RandomPasswordGenerator randomPasswordGenerator = new RandomPasswordGenerator();
                _user.Password = randomPasswordGenerator.Generate(_user.Email, _user.Mobile);
                return _user;
            }

            return new User();
        }

        public static string GetAuthToken()
        {
            return _signUpToken != null ? _signUpToken.AccessToken : string.Empty;
        }

        public static async Task<bool> SigninUser()
        {
            var request = new SigninRequest();
            request.User = _user;

            RestWrapper rest = new RestWrapper();
            var result = await rest.Post<object>(Service.Signin, request);

            return result != null;
        }

        public static async Task<bool> ResetPassword()
        {
            if (string.IsNullOrEmpty(_user.UserName))
            {
                throw new UnauthorizedAccessException("User is not logged to perform reset password");
            }

            RestWrapper rest = new RestWrapper();
            var result = await rest.Post<User>(Service.ResetPassword, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", _user.UserName)
            });

            if (!(result is Error))
            {
                return !string.IsNullOrEmpty(result.ToString());
            }

            Utility.ParseAndThrowError((result as Error).Response);
        }
    }
}
