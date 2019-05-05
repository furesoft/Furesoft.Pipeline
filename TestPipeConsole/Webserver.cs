using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Furesoft.Pipeline;

namespace TestPipeConsole
{
    public class Webserver : IPipeable
    {
        private readonly Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>> _gets = new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>();
        private readonly Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>> _posts = new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>();
        public void Start() {
            var listener = new System.Net.HttpListener();
            listener.Prefixes.Add("http://localhost:1234/");

            listener.Start();

            var t = new Thread(_=> {
                while (true)
                {
                    var context = listener.GetContext(); // get te context 

                    this.ExecutePipe(context);

                    if(context.Request.HttpMethod == "GET") {
                        _gets.GetOrIgnore(context.Request.Url.AbsolutePath)?.Invoke(context.Request, context.Response);
                    }
                    if(context.Request.HttpMethod == "POST") {
                        _posts.GetOrIgnore(context.Request.Url.AbsolutePath)?.Invoke(context.Request, context.Response);
                    }

                    if(File.Exists(Environment.CurrentDirectory + "/" + context.Request.Url.AbsolutePath)) {
                        context.Response.Close(File.ReadAllBytes(Environment.CurrentDirectory + "/" + context.Request.Url.AbsolutePath), false);
                    }
                    else {
                        context.Response.Error(404, "Resource not found!");
                    }
                }
            });
            t.Start();
        }

        public void Get(string route, Action<HttpListenerRequest, HttpListenerResponse> callback) {
            
            _gets.Add(route, callback);
        }
        public void Post(string route, Action<HttpListenerRequest, HttpListenerResponse> callback) {
            
            _posts.Add(route, callback);
        }
    }
}