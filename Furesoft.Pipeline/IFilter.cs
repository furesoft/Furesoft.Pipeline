using System;

namespace Furesoft.Pipeline
{
    public interface IFilter
    {
        object Execute(object input);
    }
}