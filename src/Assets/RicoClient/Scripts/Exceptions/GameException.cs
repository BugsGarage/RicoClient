using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Exception signalizing about some errors with game service
    /// </summary>
    public class GameException : RicoException
    {
        public GameException() : base()
        { }

        public GameException(string message) : base(message)
        { }

        public GameException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
