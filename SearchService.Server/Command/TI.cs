﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using WebSocket;
using WebSocket.SubProtocol;

using SearchService.Model;
using SearchService.Common.Indexs;
using SearchService.Common;

using SearchService.Common.Log;

namespace SearchService.Server.Command
{
    /// <summary>
    /// 描述：获取tif文件编号
    /// 作者：Sofia
    /// 日期：2016-09-14
    /// </summary>
    public class TI : JsonSubCommand<TiffInfor>
    {

        #region 属性
        static WebSocketSession tifServerSession = null;
        static Dictionary<string, WebSocketSession> ClientList = null;//new Dictionary<string,WebSocketSession>();
        //队列管理
        QueueManager<InforSession<TiffInfor>> InforQueue = null;// new QueueManager<InforSession>();
        //处理队列数据线程
        private Thread processQueueThread = null;
        //是否已启动队列管理
        static bool IsStar = false;
        #endregion

        #region 方法
        /// <summary>
        /// 启动队列及队列处理线程
        /// </summary>
        private void star()
        {
            if (InforQueue == null)
            {
                InforQueue = new QueueManager<InforSession<TiffInfor>>();
                InforQueue.EnQueueEvent += InforQueue_EnQueueEvent;
            }

            if (ClientList == null)
            {
                ClientList = new Dictionary<string, WebSocketSession>();
            }

            if (processQueueThread == null)
            {
                //启动接包线程
                this.processQueueThread = new Thread(ThreadStartPort);
                this.processQueueThread.IsBackground = true;
                this.processQueueThread.Priority = ThreadPriority.Normal;
                this.processQueueThread.Start();
            }
        }

        /// <summary>
        /// 队列线程处理入口
        /// </summary>        
        private void ThreadStartPort()
        {
            while (true)
            {
                while (this.InforQueue.QueueCount > 0)
                {
                    try
                    {
                        //队列中获取最早的数据
                        InforSession<TiffInfor> sData = this.InforQueue.DeQueueItem();
                        if (sData != null)
                        {
                            //处理队列数据
                            ProcessQueueData(sData);
                        }
                    }
                    catch (Exception ex)
                    {
                        string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                        Logger.WriteError(funName, ex);
                    }
                }
                //Monitor.Wait(this.InforQueue);
                Thread.CurrentThread.Suspend();
            }
        }

        /// <summary>
        /// 处理队列中最早的数据
        /// </summary>
        /// <param name="IS">处理队列中最早的数据</param>
        void ProcessQueueData(InforSession<TiffInfor> IS)
        {
            //
            WebSocketSession session = IS.Session;
            TiffInfor commandInfo = IS.Infor;
            if (session != null && commandInfo != null)
            {
                try
                {
                    List<Infors> resultList = new List<Infors>();
                    int recCount = 0;
                    int pageCount = 0;


                    //if (commandInfo != 0 && commandInfo.PageNo != 0)//!string.IsNullOrWhiteSpace(commandInfo.InPutWord) && 
                    //{
                    //    resultList = AllDocIndex.AllSearchByInput(commandInfo, out recCount);
                    //}

                    //string html = "";
                    //if (resultList.Count > 0)
                    //{
                    //    SearchPage sp = new SearchPage();
                    //    html = sp.GetAllResultPage(resultList);
                    //}

                    //string inforID = string.Join(",", resultList.Select(p => p.InformationID));

                    ////string html = sp.GetSensResultPage(news);
                    //List<string> words = new List<string>();
                    //words.Add(html);
                    //words.Add(inforID);
                    //words.Add(commandInfo.PageSize.ToString());
                    //words.Add(commandInfo.PageNo.ToString());
                    //words.Add(recCount.ToString());
                    //if (recCount > 0 && commandInfo.PageSize > 0)
                    //{
                    //    int mod = recCount % commandInfo.PageSize;
                    //    if (mod > 0)
                    //    {
                    //        pageCount = (recCount / commandInfo.PageSize) + 1;
                    //    }
                    //    else
                    //    {
                    //        pageCount = recCount / commandInfo.PageSize;
                    //    }
                    //}

                    //words.Add(pageCount.ToString());
                    Response rs = new Response();
                    //Response res = new Response();
                    rs.Q = "RI";
                    //rs.R = words;
                    SendJsonMessage(session, rs);
                }
                catch (Exception ex)
                {
                    string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                    Logger.WriteError(funName, ex);
                }
                finally
                {
                    //AllDocIndex.ReOpenSearch();
                }
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 队列增加数据后处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InforQueue_EnQueueEvent(object sender, EventArgs e)
        {
            if ((this.processQueueThread.ThreadState == (ThreadState.Background | ThreadState.Suspended)) ||
               (this.processQueueThread.ThreadState == (ThreadState.Background | ThreadState.SuspendRequested)))
            {

                //this.processQueueThread.Resume();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="commandInfo"></param>
        protected override void ExecuteJsonCommand(WebSocketSession session, TiffInfor commandInfo)
        {
            //if (!IsStar)
            //{
            //    star();
            //    IsStar = true;
            //}

            if (commandInfo.Type == 1)
            {
                if (ClientList == null)
                {
                    ClientList = new Dictionary<string, WebSocketSession>();
                }
                //添加客户端列表
                ClientList.Add(session.SessionID, session);
                
                //添加会话
                commandInfo.ClientSessionID = session.SessionID;

                string sendstring = "TI " + JsonHelper.SerializeObject(commandInfo);

                if (tifServerSession != null)
                {
                    if (tifServerSession.Connected)
                    {
                        tifServerSession.Send(sendstring);
                    }
                }
                else
                {
                    Response rs = new Response();
                    //Response res = new Response();
                    rs.Q = "TI";
                    rs.R = new List<string>() { "0000000000000" };
                    SendJsonMessage(session, rs);
                    //string sendstr = JsonHelper.SerializeObject(rs);
                    //session.Send(sendstr);
                }
                //InforSession<TiffInfor> infs = new InforSession<TiffInfor>();
                //infs.Session = session;
                //infs.Infor = commandInfo;

                //InforQueue.EnQueueItem(infs);
            }
            else if (commandInfo.Type == 0)
            {
                
                tifServerSession = session;
                
                if (!string.IsNullOrWhiteSpace(commandInfo.TiffNum))
                {
                    Response rs = new Response();
                    //Response res = new Response();
                    rs.Q = "TI";
                    rs.R = new List<string>() { commandInfo.TiffNum };
                    string sendstring = JsonHelper.SerializeObject(rs);
                    if (ClientList.ContainsKey(commandInfo.ClientSessionID))
                    {
                        if (ClientList[commandInfo.ClientSessionID].Connected)
                        {
                            ClientList[commandInfo.ClientSessionID].Send(sendstring);
                            ClientList.Remove(commandInfo.ClientSessionID);
                        }
                    }
                }
            }
            
        }

        #endregion
    }
}
