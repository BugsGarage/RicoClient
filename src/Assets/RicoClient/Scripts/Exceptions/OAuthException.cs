using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
