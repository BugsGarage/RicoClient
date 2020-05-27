using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// In-app authorization exception
    /// </summary>
    public class AuthorizeException : RicoException
    {
        public AuthorizeException() : base()
        { }

        public AuthorizeException(string message) : base(message)
        { }

        public AuthorizeException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
