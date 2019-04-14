using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocket;

namespace SearchService.Server.Command
{
    /// <summary>
    /// 描述：写索引的信息实体
    /// 作者：Sofia
    /// 日期：2016-09-10
    /// </summary>
    class InforSession<T>
    {
        WebSocketSession session;
        /// <summary>
        /// webSocket会话
        /// </summary>
        public WebSocketSession Session
        {
            get { return session; }
            set { session = value; }
        }

        T infor;
        /// <summary>
        /// 传入参数
        /// </summary>
        public T Infor
        {
            get { return infor; }
            set { infor = value; }
        }
    }

}
