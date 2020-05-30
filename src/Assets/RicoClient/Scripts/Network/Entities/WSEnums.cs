using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Network.Entities
{
    public enum RequestCommandType
    {
        Connect,
        Ping,
        Disconnect,
    }

    public enum ResponseCommandType
    {
        Error,
        Ping,
        Started,
        Finished
    }
}
