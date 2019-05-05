using System.Linq;

namespace Furesoft.Pipeline
{
    public class GenericPipeline : Pipe
    {
        /// <summary>
        /// Method which executes the filter on a given Input
        /// </summary>
        /// <param name="input">Input on which filtering
        /// needs to happen as implementing in individual filters</param>
        /// <returns></returns>
        public override object Process(object input)
        {
            if(filters.Any()) {
                foreach (var filter in filters)
                {
                    if(input != null) {
                        input = filter.Execute(input);
                    }
                }
            }

            return input;
        }
    }
}