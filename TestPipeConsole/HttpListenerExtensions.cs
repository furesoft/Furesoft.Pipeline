using System.Collections.Generic;
using System.IO;
using System.Net;
using HttpMultipartParser;
using Newtonsoft.Json;

namespace TestPipeConsole
{
    public static class HttpListenerExtensions
    {
        public static void AsJson(this HttpListenerResponse resp, object target) {
            
            resp.AddHeader("Accept", "application/json");
            resp.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            
            AsText(resp, JsonConvert.SerializeObject(target));
        }

        public static void AsText(this HttpListenerResponse resp, string target) {
            resp.Close(System.Text.Encoding.ASCII.GetBytes(target), false);
        }

        public static void Error(this HttpListenerResponse resp, int errorcode, string description) {
            resp.StatusCode = errorcode;
            resp.StatusDescription = description;

            resp.Close();
        }

        public static T AsObject<T>(this HttpListenerRequest req) {
            if(req.ContentType != "application/json") return default;

            if(req.HasEntityBody) {
                var sr = new StreamReader(req.InputStream);
                var body = sr.ReadToEnd();

                return JsonConvert.DeserializeObject<T>(body);
            }

            return default;
        }
        public static MultipartFormDataParser AsForm(this HttpListenerRequest req) {
            var stream = req.InputStream;
            var parser = new MultipartFormDataParser(stream);

            return parser;
        }
    }
    public static class DictionaryExtensions {
        public static TValue GetOrIgnore<TKey, TValue>(this Dictionary<TKey, TValue> target, TKey key) {
            if(target.ContainsKey(key)) {
                return target[key];
            }

            return default;
        }
    }
}