using System;

namespace Template1.API.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class Template1DomainException : Exception
    {
        public Template1DomainException()
        { }

        public Template1DomainException(string message)
            : base(message)
        { }

        public Template1DomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
