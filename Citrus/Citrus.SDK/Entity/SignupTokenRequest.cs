using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citrus.SDK.Common;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    internal class SignupTokenRequest : IEntity
    {
        [JsonProperty("client_id")]
        public string ClientId
        {
            get { return Config.SignUpId; }
        }

        [JsonProperty("client_secret")]
        public string ClientSecret
        {
            get { return Config.SignUpSecret; }
        }

        [JsonProperty("grant_type")]
        public string GrantType
        {
            get { return "implicit"; }
        }


        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            return new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id",ClientId),
                new KeyValuePair<string, string>("client_secret",ClientSecret),
                new KeyValuePair<string, string>("grant_type",GrantType),
            };
        }
    }
}
