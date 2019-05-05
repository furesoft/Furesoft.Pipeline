using System.Collections.Generic;

namespace Furesoft.Pipeline
{
    public static class PipeStorage
    {
        private static readonly Dictionary<IPipeable, Pipe> _pipes = new Dictionary<IPipeable, Pipe>();

        public static Pipe GetOrAdd(IPipeable p)
        {
            if(_pipes.ContainsKey(p))
            {
                return _pipes[p];
            }
            else
            {
                _pipes.Add(p, new GenericPipeline());
                return _pipes[p];
            }
        }
    }
}