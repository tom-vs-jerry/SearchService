using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using WebSocket.SocketBase.Command;
using WebSocket;
using WebSocket.SubProtocol;

using SearchService.Model;
using SearchService.Common;

namespace SearchService.Server.Command
{
    public class ADDX : SubCommandBase<WebSocketSession>
    {
        public override void ExecuteCommand(WebSocketSession session, SubRequestInfo requestInfo)
        {
            
            session.Send( requestInfo.Body);
        }
    }
}
