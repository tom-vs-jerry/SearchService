using System;
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
    /// 描述：写索引
    /// 作者：段进雄
    /// 日期：2016-09-08
    /// </summary>
    public class WI : JsonSubCommand<Infors>
    {
        #region 属性
        //接受数据列表（用户处理数据量大于最大处理数据的）
        Dictionary<string, List<Infors>> ReceiveList = new Dictionary<string, List<Infors>>();

        //队列管理
        QueueManager<InforSession<Infors>> InforQueue = null;// new QueueManager<InforSession>();
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
                InforQueue = new QueueManager<InforSession<Infors>>();
                InforQueue.EnQueueEvent += InforQueue_EnQueueEvent;
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
                        InforSession<Infors> sData = this.InforQueue.DeQueueItem();
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
        /// <param name="IS">最早的数据</param>
        void ProcessQueueData(InforSession<Infors> IS)
        {
            //
            WebSocketSession session = IS.Session;
            Infors commandInfo = IS.Infor;
            if (session != null && commandInfo != null)
            {
                try
                {
                    if (commandInfo.PageCount != 1)
                    {
                        List<Infors> list = new List<Infors>();

                        if (!ReceiveList.Keys.Contains(commandInfo.InformationID))
                        {

                            list.Add(commandInfo);
                            ReceiveList[commandInfo.InformationID] = list;
                        }
                        else
                        {
                            list = ReceiveList[commandInfo.InformationID];

                            if (list.Count == (commandInfo.PageCount - 1))
                            {
                                //合并分页数据
                                StringBuilder sbContext = new StringBuilder();
                                for (int i = 1; i <= list.Count; i++)
                                {
                                    List<Infors> infor = list.Where(k => k.PageNo == i).ToList<Infors>();
                                    sbContext.Append(infor[0].Content);
                                }
                                sbContext.Append(commandInfo.Content);
                                commandInfo.Content = sbContext.ToString();
                                commandInfo.PageNo = 1;
                                commandInfo.PageCount = 1;
                                ReceiveList.Remove(commandInfo.InformationID);                               

                                //全文索引处理        
                                commandInfo.Participle = AllDocIndex.GetKeyWordsSplitBySlash(commandInfo.Content);//获取全文分词
                                int Count = AllDocIndex.AllIndexString(commandInfo);//添加索引
                                AllDocIndex.CommitAllWriter();//提交数据

                                //敏感词处理
                                commandInfo.Participle = AllDocIndex.GetSensSegementByInput(commandInfo.Content);//获取敏感词
                                //if (commandInfo.Advise != null)
                                //{
                                //    if (commandInfo.Advise.Count > 0)
                                //    {                                        
                                //        Count = AllDocIndex.SensIndexString(commandInfo);//添加敏感词索引
                                //        AllDocIndex.CommitSensWriter();//提交敏感词索引
                                //    }
                                //}

                                
                                if (session.Connected)
                                {
                                    //返回敏感词
                                    string returnString = commandInfo.Participle;//.Replace('/', ',');// AllDocIndex.GetSensSegementByInput(commandInfo.Content);
                                    Response res = new Response();
                                    res.Q = "WI";
                                    List<string> R = new List<string>();
                                    R.Add(commandInfo.InformationID);
                                    R.Add(returnString);
                                    res.R = R;
                                    string sendStr = JsonHelper.SerializeObject(res);
                                    session.Send(sendStr);
                                }
                                else
                                {
                                    Logger.WriteInfo(session.SessionID + "已断开连接！");
                                }
                                

                                string log = string.Format("文件：{0}；", commandInfo.InformationID);
                                Logger.WriteInfo(log);
                            }
                            else
                            {
                                list.Add(commandInfo);
                                ReceiveList[commandInfo.InformationID] = list;
                            }
                        }

                    }
                    else
                    {
                        //全文索引处理           
                        commandInfo.Participle = AllDocIndex.GetKeyWordsSplitBySlash(commandInfo.Content);//获取全文分词
                        int Count = AllDocIndex.AllIndexString(commandInfo);//添加索引
                        AllDocIndex.CommitAllWriter();//提交数据

                        //敏感词处理
                        commandInfo.Participle = AllDocIndex.GetSensSegementByInput(commandInfo.Content);//获取敏感词
                        //if (commandInfo.Advise != null)
                        //{
                        //    if (commandInfo.Advise.Count > 0)
                        //    {
                        //        Count = AllDocIndex.SensIndexString(commandInfo);//添加敏感词索引
                        //        AllDocIndex.CommitSensWriter();//提交敏感词索引
                        //    }
                        //}

                        
                        if (session.Connected)
                        {
                            //返回敏感词
                            string returnString = commandInfo.Participle;//.Replace('/', ',');//AllDocIndex.GetSensSegementByInput(commandInfo.Content);
                            Response res = new Response();
                            res.Q = "WI";
                            List<string> R = new List<string>();

                            R.Add(commandInfo.InformationID);
                            R.Add(returnString);
                            res.R = R;
                            string sendStr = JsonHelper.SerializeObject(res);
                            session.Send(sendStr);
                        }
                        else
                        {
                            Logger.WriteInfo(session.SessionID + "已断开连接！");
                        }

                        string log = string.Format("文件：{0}；", commandInfo.InformationID);
                        Logger.WriteInfo(log);
                    }

                    //string SR=  AllDocIndex.GetKeyWordsSplitBySpace(commandInfo.Content);
                    //SendJsonMessage(session, commandInfo);
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

                this.processQueueThread.Resume();
            }
        }

        /// <summary>
        /// 执行Websocket命令
        /// </summary>
        /// <param name="session">会话</param>
        /// <param name="commandInfo">请求数据</param>
        protected override void ExecuteJsonCommand(WebSocketSession session, Infors commandInfo)
        {
            if (!IsStar)
            {
                star();
                IsStar = true;
            }
            InforSession<Infors> infs = new InforSession<Infors>();
            infs.Session = session;
            infs.Infor = commandInfo;

            InforQueue.EnQueueItem(infs);

            #region 注释原始代码
            //try
            //{
            //    if (commandInfo.PageCount != 1)
            //    {
            //        List<Infors> list = new List<Infors>();

            //        if (!ReceiveList.Keys.Contains(commandInfo.InformationID))
            //        {

            //            list.Add(commandInfo);
            //            ReceiveList[commandInfo.InformationID] = list;
            //        }
            //        else
            //        {
            //            list = ReceiveList[commandInfo.InformationID];

            //            if (list.Count == (commandInfo.PageCount - 1))
            //            {
            //                //list.Add(commandInfo);
            //                StringBuilder sbContext = new StringBuilder();
            //                for (int i = 1; i <= list.Count; i++)
            //                {

            //                    List<Infors> infor = list.Where(k => k.PageNo == i).ToList<Infors>();
            //                    sbContext.Append(infor[0].Content);

            //                }
            //                sbContext.Append(commandInfo.Content);
            //                commandInfo.Content = sbContext.ToString();
            //                commandInfo.PageNo = 1;
            //                commandInfo.PageCount = 1;
            //                ReceiveList.Remove(commandInfo.InformationID);
            //                //sbContext.Append(commandInfo)
            //                string log = string.Format("文件：{0}；", commandInfo.InformationID);
            //                Logger.WriteInfo(log);

            //                commandInfo.Participle = AllDocIndex.GetKeyWordsSplitBySlash(commandInfo.Content);
            //                //int Count = AllDocIndex.AllIndexString(commandInfo);

            //                if (commandInfo.Advise != null)
            //                {
            //                    if (commandInfo.Advise.Count > 0)
            //                    {
            //                        commandInfo.Participle = AllDocIndex.GetSensWordsSplitBySlash(commandInfo.Content);
            //                        //Count = AllDocIndex.SensIndexString(commandInfo);
            //                    }
            //                }

            //                //返回敏感词
            //                //返回敏感词
            //                string returnString = AllDocIndex.GetSensSegementByInput(commandInfo.Content);
            //                Response res = new Response();
            //                res.Q = "WI";
            //                List<string> R = new List<string>();
            //                R.Add(commandInfo.InformationID);
            //                R.Add(returnString);
            //                res.R = R;
            //                string sendStr = JsonHelper.SerializeObject(res);
            //                session.Send(sendStr);

            //                AllDocIndex.CommitWriter();
            //            }
            //            else
            //            {
            //                list.Add(commandInfo);
            //                ReceiveList[commandInfo.InformationID] = list;
            //            }
            //        }

            //    }
            //    else
            //    {

            //        string log = string.Format("文件：{0}；", commandInfo.InformationID);
            //        Logger.WriteInfo(log);
            //        commandInfo.Participle = AllDocIndex.GetKeyWordsSplitBySlash(commandInfo.Content);
            //        //int Count = AllDocIndex.AllIndexString(commandInfo);

            //        if (commandInfo.Advise != null)
            //        {
            //            if (commandInfo.Advise.Count > 0)
            //            {
            //                commandInfo.Participle = AllDocIndex.GetSensWordsSplitBySlash(commandInfo.Content);
            //                //Count = AllDocIndex.SensIndexString(commandInfo);
            //            }
            //        }

            //        //返回敏感词
            //        string returnString = AllDocIndex.GetSensSegementByInput(commandInfo.Content);
            //        Response res = new Response();
            //        res.Q = "WI";
            //        List<string> R = new List<string>();

            //        R.Add(commandInfo.InformationID);
            //        R.Add(returnString);
            //        res.R = R;
            //        string sendStr = JsonHelper.SerializeObject(res);

            //        session.Send(sendStr);
            //        AllDocIndex.CommitWriter();
            //    }

            //    //string SR=  AllDocIndex.GetKeyWordsSplitBySpace(commandInfo.Content);
            //    //SendJsonMessage(session, commandInfo);
            //}
            //catch (Exception EX)
            //{

            //}
            //finally
            //{
            //    //AllDocIndex.ReOpenSearch();
            //}
            #endregion
        }
        #endregion
    }
    
}
