﻿using System;
using System.Collections.Generic;
using System.Text;
//using WebSocket.ClientEngine;
//using WebSocket.ClientEngine.Protocol;
using WebSocket4Net.Common;

namespace WebSocket4Net.Command
{
    public abstract class WebSocketCommandBase : ICommand<WebSocket, WebSocketCommandInfo>
    {
        public abstract void ExecuteCommand(WebSocket session, WebSocketCommandInfo commandInfo);

        public abstract string Name { get; }
    }
}
