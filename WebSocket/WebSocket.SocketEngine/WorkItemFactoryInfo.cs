using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket.SocketBase.Config;
using WebSocket.SocketBase.Provider;
using WebSocket.SocketBase.Metadata;

namespace WebSocket.SocketEngine
{
    class WorkItemFactoryInfo
    {
        public string ServerType { get; set; }

        public bool IsServerManager { get; set; }

        public StatusInfoAttribute[] StatusInfoMetadata { get; set; }

        public IServerConfig Config { get; set; }

        public IEnumerable<ProviderFactoryInfo> ProviderFactories { get; set; }

        public ProviderFactoryInfo LogFactory { get; set; }

        public ProviderFactoryInfo SocketServerFactory { get; set; }
    }
}
