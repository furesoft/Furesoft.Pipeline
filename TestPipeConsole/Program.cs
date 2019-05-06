using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Furesoft.Pipeline;

namespace TestPipeConsole
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");

            var ws = new Webserver();
            RouteCollector.CollectRoutes(ws);

            ws.Get("/", (req, resp) => {
                resp.AsJson(new {status = "Routing was success!!"});
            });
            ws.Get("/hello", (req, resp) => {

                resp.Error(404, "Resource not found!");
            });

            ws.Use(new DisallowGET());
            ws.Start();
        }
    }

public class Main {
    [Route("/collection", "POST")]
    public object Index(string req) {
        return "returning with routing successfully! :D :P ... " + req;
    }

    [Route("/app/")]
    public View<object> AppIndex() {
        return new AppIndexViewModel { Name = "hello world" };
    }
}

public class Auth {
    [Route("/auth")]
    public object Auths(string username, string pw, System.Net.Http.HttpListenerResponse resp) {
        if(Thread.CurrentPrincipal.Identity.IsAuthenticated) {
            resp.RedirectAsync(new Uri("/app/"));
        }
        else {
            return "login";
        }
        return "login";
    }
}

public class AppIndexViewModel {
    public string Name { get; set; }
}
    public class DisallowGET : IFilter
    {
        public object Execute(object input)
        {
            var arg = (System.Net.HttpListenerContext) input;

            if(arg.Request.HttpMethod == "GET") {
                if(arg.Request.Url.LocalPath == "/api/users") {
                    arg.Response.AsJson(new {username = arg.Request.Cookies["username"].Value});
                }
                else {
                    arg.Response.SetCookie(new Cookie("username", "filmee24"));
                }
            }
            if(arg.Request.HttpMethod == "POST") {
                if(arg.Request.Url.LocalPath == "/api/users") {
                    var obj = arg.Request.AsObject<User>();
                    arg.Response.AsJson(new {status = "success", user = obj});
                }
                if(arg.Request.Url.LocalPath == "/api/add") {
                    var obj = arg.Request.AsForm();
                    arg.Response.AsJson(new {status = "success", user = obj.GetParameterValue("username")});
                }
                return null;
            }
            
            return arg;
        }
    }
    public class User {
        public string username { get; set; }      
    }
}