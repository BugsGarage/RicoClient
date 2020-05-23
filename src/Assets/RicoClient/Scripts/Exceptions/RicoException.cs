using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
