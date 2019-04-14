using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocket.SocketBase;
using WebSocket.SocketBase.Command;
using WebSocket.SocketBase.Protocol;

namespace SearchService.Server.Command
{
    public class ECHO:StringCommandBase<AppSession>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            if (requestInfo.Parameters == null || requestInfo.Parameters.Length != 2)
                return;

            var username = requestInfo.Parameters[0];
            var password = requestInfo.Parameters[1];

            if ("kerry".Equals(username) && "123456".Equals(password))
            {
                //session.IsLoggedIn = true;
            }
        }
    }
}
