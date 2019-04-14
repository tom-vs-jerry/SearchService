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
    /// 描述：Index(索引文件管理)
    /// 作者：Sofia
    /// 日期：2016年8月22日   
    /// </summary>
    public class IX : JsonSubCommand<Request>
    {
        protected override void ExecuteJsonCommand(WebSocketSession session, Request commandInfo)
        {
            SendJsonMessage(session, commandInfo);
        }
    }
}
