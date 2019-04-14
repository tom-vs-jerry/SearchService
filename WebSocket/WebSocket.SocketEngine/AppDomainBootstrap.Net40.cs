using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.SocketBase;
using WebSocket.SocketBase.Config;

namespace WebSocket.SocketEngine
{
    partial class AppDomainBootstrap : IDynamicBootstrap
    {
        bool IDynamicBootstrap.Add(IServerConfig config)
        {
            var dynamicBootstrap = m_InnerBootstrap as IDynamicBootstrap;
            return dynamicBootstrap.Add(config);
        }

        bool IDynamicBootstrap.AddAndStart(IServerConfig config)
        {
            var dynamicBootstrap = m_InnerBootstrap as IDynamicBootstrap;
            return dynamicBootstrap.AddAndStart(config);
        }

        void IDynamicBootstrap.Remove(string name)
        {
            var dynamicBootstrap = m_InnerBootstrap as IDynamicBootstrap;
            dynamicBootstrap.Remove(name);
        }
    }
}
