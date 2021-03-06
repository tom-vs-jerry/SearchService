﻿using System;
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
    /// 描述：优化索引
    /// 作者：Sofia
    /// 日期：2016-09-08
    /// </summary>
    public class WIO : SubCommandBase<WebSocketSession>
    {
        
        public override void ExecuteCommand(WebSocketSession session, SubRequestInfo requestInfo)
        {
            if (requestInfo.Body == "Commit" && requestInfo.Key == "WIO")
            {
                AllDocIndex.OptimizeWriter();
                AllDocIndex.Rebuild();
                    
                //AllDocIndex.RebuildSearch();                
            }
        }
    }
}
