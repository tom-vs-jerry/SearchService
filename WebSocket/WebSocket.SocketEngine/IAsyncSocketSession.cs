using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using WebSocket.Common;
using WebSocket.SocketBase;
using WebSocket.SocketBase.Logging;
using WebSocket.SocketEngine.AsyncSocket;

namespace WebSocket.SocketEngine
{
    interface IAsyncSocketSessionBase : ILoggerProvider
    {
        SocketAsyncEventArgsProxy SocketAsyncProxy { get; }
        
        Socket Client { get; }
    }

    interface IAsyncSocketSession : IAsyncSocketSessionBase
    {
        void ProcessReceive(SocketAsyncEventArgs e);
    }
}
