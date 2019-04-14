﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.SocketBase;
using WebSocket.SocketBase.Metadata;

namespace WebSocket.SubProtocol
{
    /// <summary>
    /// SubCommandFilter Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class SubCommandFilterAttribute : CommandFilterAttribute
    {
        /// <summary>
        /// Gets or sets the sub protocol.
        /// </summary>
        /// <value>
        /// The sub protocol.
        /// </value>
        public string SubProtocol { get; set; }
    }
}
