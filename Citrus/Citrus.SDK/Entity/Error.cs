using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class Error:IEntity
    {
        public Error()
        {
            
        }

        public Error(string response)
        {
            Response = response;
        }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        [JsonProperty("error")]
        public string Code { get; set; }

        [JsonProperty("error_description")]
        public string Message { get; set; }

        [JsonIgnore]
        public string Response { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}
