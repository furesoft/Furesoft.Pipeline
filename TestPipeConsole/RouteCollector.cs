using System;
using System.Net;
using System.Reflection;
using System.Web;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace TestPipeConsole
{
    public static class RouteCollector
    {
        public static void CollectRoutes(Webserver ws)
        {
            var a = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var t in a)
            {
                foreach (var m in t.GetMethods())
                {
                    var attr = m.GetCustomAttribute<RouteAttribute>();

                    if (attr != null)
                    {
                        if (attr.Method == "GET")
                        {
                            ws.Get(attr.Path, (req, res) => TransferMethodResult(m.Invoke(Activator.CreateInstance(t), GetInputs(req, m)), res));
                        }
                        else if (attr.Method == "POST")
                        {
                            ws.Post(attr.Path, (req, res) => TransferMethodResult(m.Invoke(Activator.CreateInstance(t), GetInputs(req, m)), res));
                        }
                    }
                }
            }
        }

        private static void TransferMethodResult(object result, HttpListenerResponse res)
        {
            if (result is string s)
            {
                res.AsText(s);
            }
            //ToDo: check for View

            else
            {
                res.AsJson(result);
            }
        }

        private static object[] GetInputs(HttpListenerRequest req, MethodInfo mi)
        {
            var res = new List<object>();
            if (req.HttpMethod == "GET")
            {
                var query = HttpUtility.ParseQueryString(req.Url.Query);

                foreach (var pa in mi.GetParameters())
                {
                    var value = query[pa.Name];
                    var tc = TypeDescriptor.GetConverter(pa.ParameterType);
                    var convertet = tc.ConvertFromString(value);

                    if (value != null)
                    {
                        res.Add(convertet);
                    }
                }
            }
            else if (req.HttpMethod == "POST")
            {

                var parser = req.AsForm();

                foreach (var pa in mi.GetParameters())
                {
                    var value = parser.GetParameterValue(pa.Name);
                    var tc = TypeDescriptor.GetConverter(pa.ParameterType);
                    var convertet = tc.ConvertFromString(value);

                    if (value != null)
                    {
                        res.Add(convertet);
                    }
                }
            }

            return res.ToArray();
        }
    }
}