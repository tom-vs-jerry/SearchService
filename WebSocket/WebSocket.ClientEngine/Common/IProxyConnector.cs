using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace WebSocket.ClientEngine
{
    public interface IProxyConnector
    {
        void Connect(EndPoint remoteEndPoint);

        event EventHandler<ProxyEventArgs> Completed;
    }
}
