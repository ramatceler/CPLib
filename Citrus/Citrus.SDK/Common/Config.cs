using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Citrus.SDK.Common
{
    public class Config
    {
        public static EnvironmentType Environment { get; set; }

        public static string SignInId { get; set; }

        public static string SignInSecret { get; set; }

        public static string SignUpId { get; set; }

        public static string SignUpSecret { get; set; }
    }
}
