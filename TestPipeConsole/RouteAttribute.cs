using System;

namespace TestPipeConsole
{
    public class RouteAttribute : Attribute
    {

        public RouteAttribute(string p, string m)
        {
            this.Path = p;
            this.Method = m;
        }

        public string Path { get; set; } 
        public string Method { get; set; }
    }
}