using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Exception signalizing about some errors with shop service
    /// </summary>
    public class ShopException : RicoException
    {
        public ShopException() : base()
        { }

        public ShopException(string message) : base(message)
        { }

        public ShopException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
