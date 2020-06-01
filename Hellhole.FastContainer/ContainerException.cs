using System;

namespace Hellhole.FastContainer
{
    public class ContainerException
        : Exception
    {
        public ContainerException(string message)
            : base(message)
        {
        }
    }
}
