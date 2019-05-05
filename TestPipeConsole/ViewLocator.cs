using System;
using System.IO;

namespace TestPipeConsole
{
    public class ViewLocator
    {
        public static string FindView(string name) {

            foreach(var file in Directory.GetFiles(Environment.CurrentDirectory, name, SearchOption.AllDirectories)) {
                return File.ReadAllText(file);
            }

            return $"View for '{name}' not found!";
        } 
    }
}