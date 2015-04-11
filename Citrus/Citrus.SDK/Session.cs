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

        public static async Task<User> SignupUser(string email, string mobile)
        {
            await GetSignupToken();
            var user = new User { Email = email, Mobile = mobile };
            _user = user;
            RestWrapper rest = new RestWrapper();
            _user = (User)await rest.Post<User>(Service.Signup, user);

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

        /// <summary>
        /// Sign in the user account.
        /// </summary>
        /// <param name="userName">Citrus Pay UserName.</param>
        /// <param name="password">Citrus Pay Password.</param>
        /// <returns></returns>
        public static async Task<bool> SigninUser(string userName, string password)
        {
            var request = new SigninRequest();
            request.User = new User()
            {
                UserName = userName,
                Password = password
            };

            RestWrapper rest = new RestWrapper();
            var result = await rest.Post<object>(Service.Signin, request);

            return result != null;
        }

        /// <summary>
        /// Update your password with new password
        /// </summary>
        /// <param name="oldPassword">Your Old Password.</param>
        /// <param name="newPassword">Your New Password.</param>
        /// <returns></returns>
        public static async Task<bool> UpdatePassword(string oldPassword, string newPassword)
        {
            var request = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("old", oldPassword),
                new KeyValuePair<string, string>("new", oldPassword)
            };
            var rest = new RestWrapper();
            var result = await rest.Post<object>(Service.UpdatePassword, request);
            return result != null;
        }

        public static async Task<bool> ResetPassword()
        {
            RestWrapper rest = new RestWrapper();
            var result = await rest.Post<User>(Service.ResetPassword, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", _user.UserName)
            });

            return !string.IsNullOrEmpty(result.ToString());
        }
    }
}
