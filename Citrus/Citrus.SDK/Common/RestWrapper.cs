using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Citrus.SDK.Entity;
using Citrus.SDK.Common;
using Newtonsoft.Json;

namespace Citrus.SDK.Common
{
    internal class RestWrapper
    {
        public IEntity Get(string relativeServicePath)
        {
            HttpClient client = new HttpClient();

            return null;
        }

        public async Task<IEntity> Post<T>(string relativeServicePath, IEntity objectToPost = null)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                Session.GetAuthToken());

            if (objectToPost != null)
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(objectToPost.ToKeyValuePair());
                response =
                    await client.PostAsync(Config.Environment.GetEnumDescription() + relativeServicePath, content);
            }
            else
            {
                response =
                    await client.PostAsync(Config.Environment.GetEnumDescription() + relativeServicePath, new StringContent(""));
            }

            if (response.IsSuccessStatusCode)
            {
                JsonSerializer serializer = new JsonSerializer();
                return
                    (IEntity)
                        serializer.Deserialize<T>(
                            new JsonTextReader(new StringReader(await response.Content.ReadAsStringAsync())));
            }

            return await ReturnError(response);
        }

        public async Task<IEntity> Post<T>(string relativeServicePath, IEnumerable<KeyValuePair<string, string>> urlParams)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.GetAuthToken());

            FormUrlEncodedContent content = new FormUrlEncodedContent(urlParams);
            response = await client.PostAsync(Config.Environment.GetEnumDescription() + relativeServicePath, content);

            if (response.IsSuccessStatusCode)
            {
                JsonSerializer serializer = new JsonSerializer();
                return
                    (IEntity)
                        serializer.Deserialize<T>(
                            new JsonTextReader(new StringReader(await response.Content.ReadAsStringAsync())));
            }

            return await ReturnError(response);
        }

        private async Task<IEntity> ReturnError(HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.BadRequest ? new Error(await response.Content.ReadAsStringAsync()) : new Error();
        }
    }
}
