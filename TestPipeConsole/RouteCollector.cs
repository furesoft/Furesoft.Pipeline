using System;
using System.Net;
using System.Reflection;
using System.Web;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Scriban;
using System.IO;

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
                            ws.Get(attr.Path, (req, res) => TransferMethodResult(m.Invoke(Activator.CreateInstance(t), GetInputs(req, res, m)), res));
                        }
                        else if (attr.Method == "POST")
                        {
                            ws.Post(attr.Path, (req, res) => TransferMethodResult(m.Invoke(Activator.CreateInstance(t), GetInputs(req, res, m)), res));
                        }
                    }
                }
            }
        }

        private static void TransferMethodResult(object result, HttpListenerResponse res)
        {
            if (result is string || result is int | result is double)
            {
                res.AsText(result.ToString());
            }
            //ToDo: check for View
            else if (result is View<object> v)
            {
                var view = ViewLocator.FindView(v.Viewname);
                var template = Template.Parse(view);
                var r = template.Render(v.Model);

                res.AsText(r);
            }
            else
            {
                res.AsJson(result);
            }
        }

        private static object[] GetInputs(HttpListenerRequest req, HttpListenerResponse resp, MethodInfo mi)
        {
            var res = new List<object>();
            if (req.HttpMethod == "GET")
            {
                var query = HttpUtility.ParseQueryString(req.Url.Query);

                foreach (var pa in mi.GetParameters())
                {
                    if (pa.ParameterType is HttpListenerRequest)
                    {
                        res.Add(req);
                        continue;
                    }
                    if (pa.ParameterType is HttpListenerResponse)
                    {
                        res.Add(resp);
                        continue;
                    }

                    if (query.Count > 0)
                    {
                        var value = query[pa.Name];
                        if(value == null) continue;
                        var tc = TypeDescriptor.GetConverter(pa.ParameterType);
                        var convertet = tc.ConvertFromString(value);

                        if (value != null)
                        {
                            res.Add(convertet);
                        }
                    }
                }
            }
            else if (req.HttpMethod == "POST")
            {

                var parser = req.AsForm();

                foreach (var pa in mi.GetParameters())
                {
                    if (pa.ParameterType is HttpListenerRequest)
                    {
                        res.Add(req);
                        continue;
                    }
                    if (pa.ParameterType is HttpListenerResponse)
                    {
                        res.Add(resp);
                        continue;
                    }

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