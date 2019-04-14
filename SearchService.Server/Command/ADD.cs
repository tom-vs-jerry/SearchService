using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocket;
using WebSocket.SubProtocol;

using SearchService.Model;

namespace SearchService.Server.Command
{
    public class ADD : JsonSubCommand<Request>
    {
        protected override void ExecuteJsonCommand(WebSocketSession session, Request commandInfo)
        {
            SendJsonMessage(session, commandInfo);
        }
    }
}
