using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Exception signalizing about errors in the OAuth flow
    /// </summary>
    public class OAuthException : RicoException
    {
        public OAuthException() : base()
        { }

        public OAuthException(string message) : base(message)
        { }

        public OAuthException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
