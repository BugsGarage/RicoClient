using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Exception signalizing about some errors with cards service
    /// </summary>
    public class CardsException : RicoException
    {
        public CardsException() : base()
        { }

        public CardsException(string message) : base(message)
        { }

        public CardsException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
