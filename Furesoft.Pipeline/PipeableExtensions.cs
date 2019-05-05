namespace Furesoft.Pipeline
{
    public static class PipeableExtensions
    {
        public static void Use(this IPipeable pipe, IFilter filter)
        {
            var p = PipeStorage.GetOrAdd(pipe);
            p.Register(filter);
        }

        public static void ExecutePipe(this IPipeable pipe, object input) {
            var p = PipeStorage.GetOrAdd(pipe);
            p.Process(input);
        }
    }
}