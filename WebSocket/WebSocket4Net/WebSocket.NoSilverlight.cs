using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Text;
using WebSocket.ClientEngine;

namespace WebSocket4Net
{
    public partial class WebSocket
    {
#if NETCORE
       private SslProtocols m_SecureProtocols = SslProtocols.Tls11 | SslProtocols.Tls12;
#else
        private SslProtocols m_SecureProtocols = SslProtocols.Tls;//.Default;
#endif

        private TcpClientSession CreateSecureTcpSession()
        {
            var client = new SslStreamTcpSession();
            client.Security.EnabledSslProtocols = m_SecureProtocols;
            return client;
        }
    }
}
