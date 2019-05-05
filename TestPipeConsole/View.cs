namespace TestPipeConsole
{
    public class View<T>
    {
        public string Viewname { get; set; }
        public T Model { get; set; }


        public static implicit operator View<T>(T obj)
        {
            var name = obj.GetType().Name.Replace("Model", "");

            return new View<T> { Viewname = name + ".sbnhtm", Model = obj };
        }
    }
}