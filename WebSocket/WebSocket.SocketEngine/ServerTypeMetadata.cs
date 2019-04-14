using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket.SocketBase.Metadata;

namespace WebSocket.SocketEngine
{
    [Serializable]
    class ServerTypeMetadata
    {
        public StatusInfoAttribute[] StatusInfoMetadata { get; set; }

        public bool IsServerManager { get; set; } 
    }
}
