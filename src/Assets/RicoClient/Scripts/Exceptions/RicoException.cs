using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Base exception for client
    /// </summary>
    public class RicoException : Exception
    {
        public RicoException() : base()
        { }

        public RicoException(string message) : base(message)
        { }

        public RicoException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
