﻿using System;

namespace RicoClient.Scripts.Exceptions
{
    /// <summary>
    /// Exception signalizing about some core game exception (in case if someone would try to break the whole game)
    /// </summary>
    public class GameCoreException : RicoException
    {
        public GameCoreException() : base()
        { }

        public GameCoreException(string message) : base(message)
        { }

        public GameCoreException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
