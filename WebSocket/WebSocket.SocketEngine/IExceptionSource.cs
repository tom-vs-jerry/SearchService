using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket.Common;

namespace WebSocket.SocketEngine
{
    interface IExceptionSource
    {
        event EventHandler<ErrorEventArgs> ExceptionThrown;
    }
}
