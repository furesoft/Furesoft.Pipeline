using System.Collections.Generic;

namespace Furesoft.Pipeline
{
    public abstract class Pipe
    {
        /// <summary>
        /// List of filters in the pipeline
        /// </summary>
        protected readonly List<IFilter> filters = new List<IFilter>();

        /// <summary>
        /// To Register filter in the pipeline
        /// </summary>
        /// <param name="filter">A filter object implementing IFilter interface</param>
        /// <returns></returns>
        public Pipe Register(IFilter filter)
        {
            filters.Add(filter);
            return this;
        }

        /// <summary>
        /// To start processing on the Pipeline
        /// </summary>
        /// <param name="input">
        /// The input object on which filter processing would execute</param>
        /// <returns></returns>
        public abstract object Process(object input);
    }
}