using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class User : IEntity
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        
        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("email", Email),
                new KeyValuePair<string, string>("mobile", Mobile)
            };
        }
    }
}
