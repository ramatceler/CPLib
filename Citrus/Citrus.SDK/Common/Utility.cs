using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citrus.SDK.Entity;
using Newtonsoft.Json;

namespace Citrus.SDK.Common
{
    internal class Utility
    {
        public static void ParseAndThrowError(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return;
            }

            var serializer = new JsonSerializer();
            var error = serializer.Deserialize<Error>(new JsonTextReader(new StringReader(response)));
            throw new Exception(error.Message);
        }
    }
}
