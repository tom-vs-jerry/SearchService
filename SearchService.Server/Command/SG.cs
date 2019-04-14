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
    /// <summary>
    /// 描述：分词服务（Segment）
    /// 作者：Sofia
    /// 日期：2016年8月22日
    /// </summary>
    public class SG: JsonSubCommand<Request>
    {
        protected override void ExecuteJsonCommand(WebSocketSession session, Request commandInfo)
        {
            SendJsonMessage(session, commandInfo);
        }
    }
}
