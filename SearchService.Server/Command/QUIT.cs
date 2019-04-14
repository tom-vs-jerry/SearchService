using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocket;
using WebSocket.SubProtocol;

namespace SearchService.Server.Command
{
    /// <summary>
    /// 描述：关闭会话
    /// 日期：2016-09-08
    /// 作者：段进雄
    /// </summary>
    public class QUIT : SubCommandBase<WebSocketSession>
    {
        /// <summary>
        /// 关闭会话
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(WebSocketSession session, SubRequestInfo requestInfo)
        {
            if (requestInfo.Key == "QUIT" && requestInfo.Body == "bye")
            {
                session.Send("bye");
                session.Close();
            }
            
        }
    }
}
