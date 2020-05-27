using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Exception signalizing about some errors with players service
    /// </summary>
    public class PlayersException : RicoException
    {
        public PlayersException() : base()
        { }

        public PlayersException(string message) : base(message)
        { }

        public PlayersException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
