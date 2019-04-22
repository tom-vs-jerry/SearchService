using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

using WebSocket;
using WebSocket.SocketBase;
using SearchService.Model;

namespace SearchService.Common.WebSocket
{
    public class WebSocket
    {
        #region WebSocket属性

        //private Queue queRequest = new Queue();
        /// <summary>
        /// WebSocket客户端请求
        /// </summary>
        //public Queue QueRequest
        //{
        //    get { return queRequest; }
        //    set { queRequest = value; }
        //}

        private Hashtable hasWebSocketClient = new Hashtable();
        /// <summary>
        /// WebSocket客户端请求表（Key：SessionID）
        /// </summary>
        public Hashtable HasWebSocketClient
        {
            get { return hasWebSocketClient; }
            set { hasWebSocketClient = value; }
        }

        private WebSocketServer webSocServer = new WebSocketServer();
        /// <summary>
        /// WebSocket服务
        /// </summary>
        public WebSocketServer WebSocServer
        {
            get { return webSocServer; }
            set { webSocServer = value; }
        }

        private int webSocketClientCount = 0;
        /// <summary>
        /// WebSocket客户端总数
        /// </summary>
        public int WebSocketClientCount
        {
            get { return webSocketClientCount; }
            set { webSocketClientCount = value; }
        }

        //private int intPort = 0;
        /// <summary>
        /// WebSocket服务端口
        /// </summary>
        public int IntPort
        {
            get
            {
                string port = ConfigurationManager.AppSettings["WebSocketPort"];
                return int.Parse(port);
            }

        }

        bool IsPrintClientConnectInfor = bool.Parse(ConfigurationManager.AppSettings["IsPrintClientConnectInfor"]);

        #endregion

        #region 消息事件
        //消息事件
        public delegate void WebSocketMessageEventHandler(object sender, EventArgs e);

        public event WebSocketMessageEventHandler WebSocketMessageEvent;
        protected virtual void OnMessages(object sender, EventArgs e)
        {
            if (WebSocketMessageEvent != null)
            {
                WebSocketMessageEvent(sender, e);
            }
        }

        #endregion

        #region WebSocket服务端，接收请求数据
        /// <summary>
        /// 开启供html5访问的websocket服务
        /// </summary>
        public void Start()
        {
            object message = "";
            if (WebSocServer.Setup(IntPort)) //Setup with listening port
            {
                message = string.Format("……本地WebSocket服务端口{0}侦听服务启动中……", IntPort.ToString());
                OnMessages(message, null);
            }
            if (WebSocServer.Start())
            {
                WebSocServer.NewSessionConnected += new SessionHandler<WebSocketSession>(webSocServer_NewSessionConnected);
                WebSocServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(webSocServer_SessionClosed);
                
                message = string.Format("——本地WebSocket服务端口{0}侦听服务已启动！——", IntPort.ToString());
                OnMessages(message, null);
            }
            else
            {
                message = string.Format("……本地WebSocket服务端口{0}侦听服务启动失败！……", IntPort.ToString());
                OnMessages(message, null);
            }
        }            
                
        /// <summary>
        /// HTML5 websocket服务器接收到新的回话连接事件
        /// </summary>
        /// <param name="session">回话</param>
        public void webSocServer_NewSessionConnected(WebSocketSession session)
        {
            string seID = session.SessionID;
            HasWebSocketClient.Add(seID, session);
            if (IsPrintClientConnectInfor)
            {
                object message = string.Format("IP地址：{0} 的用户,建立新连接！", session.RemoteEndPoint.ToString());
                OnMessages(message, null);
            }
        }

        /// <summary>
        ///  HTML5 websocket服务器接收到断开连接事件
        /// </summary>
        /// <param name="session">回话</param>
        /// <param name="value">关闭的原因</param>
        private void webSocServer_SessionClosed(WebSocketSession session, CloseReason value)
        {

            string sesID = session.SessionID;
            if (HasWebSocketClient.Contains(sesID))
            {
                HasWebSocketClient.Remove(sesID);
                object message = string.Format("IP地址：{0} 的用户断开连接！", session.RemoteEndPoint.Address);
                OnMessages(message, null);
            }

        }
        #endregion
    }
}
