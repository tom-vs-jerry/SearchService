using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocket;
using WebSocket.SubProtocol;

using SearchService.Common.Indexs;

namespace SearchService.Server.Command
{
    /// <summary>
    /// 描述:重启所有的搜索
    /// 作者：段进雄
    /// 日期：2016-09-08
    /// </summary>
    public class RS : SubCommandBase<WebSocketSession>
    {
        public override void ExecuteCommand(WebSocketSession session, SubRequestInfo requestInfo)
        {
            if (requestInfo.Body == "Search" && requestInfo.Key == "RS")
            {
                AllDocIndex.RebuildSearch();
                //AllDocIndex.Rebuild();

            }
        }
    }
}
