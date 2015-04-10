using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Common
{
    internal class Service
    {
        private const string ServiceVersion = "service/v2/";
        public const string Signup = ServiceVersion + "identity/new";
        public const string ResetPassword = ServiceVersion + "identity/passwords/reset";
        public const string SignUpToken = "oauth/token";
        public const string Signin = "oauth/token";
        public const string UpdatePassword = ServiceVersion + "identity/me/password";
        public const string RandonPassword = "oauth/token";
    }
}
