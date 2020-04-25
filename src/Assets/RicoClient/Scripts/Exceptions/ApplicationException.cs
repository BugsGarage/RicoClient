﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Base exception for client
    /// </summary>
    public class ApplicationException : Exception
    {
        public ApplicationException() : base()
        { }

        public ApplicationException(string message) : base(message)
        { }

        public ApplicationException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
