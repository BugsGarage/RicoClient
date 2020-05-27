using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Exception signalizing about a lack of player's balance value
    /// </summary>
    public class NotEnoughBalanceException : RicoException
    {
        public NotEnoughBalanceException() : base()
        { }

        public NotEnoughBalanceException(string message) : base(message)
        { }

        public NotEnoughBalanceException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
