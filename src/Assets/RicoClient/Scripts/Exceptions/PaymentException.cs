using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Exception signalizing about some errors with payment service
    /// </summary>
    public class PaymentException : RicoException
    {
        public PaymentException() : base()
        { }

        public PaymentException(string message) : base(message)
        { }

        public PaymentException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
