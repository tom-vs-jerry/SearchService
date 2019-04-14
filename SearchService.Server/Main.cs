using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Windows.Forms;
using SearchService.Common.Indexs;
namespace SearchService.Server
{
    public partial class Main : Form
    {


        //===============================  构造函数  ===============================
        #region 构造函数
        /// <summary>构造函数
        /// 构造函数
        /// </summary>
        public Main()
        {
            InitializeComponent();
        }


        public delegate void ChangeHander(int i);
        event ChangeHander OnChange;



        #endregion

        //===============================  变量区  ===============================
        #region 锁定窗体实例
        /// <summary>
        /// 锁定窗体实例
        /// </summary>
        private LockedUI m_SLockedUI = null;
        #endregion

        #region 设置窗体
        /// <summary>
        /// 设置窗体
        /// </summary>
        private SSystemSet.SystemSetUI m_SSystemSetUI = null;
        //文件信息
        List<FileInfo> listDoc = new List<FileInfo>();
        #endregion


        /// <summary>
        /// 当专线服务器异常时更换专线的等待时间 单位：秒
        /// </summary>
        private int changServerLink = 120;


        private int isMarketOpen = 0;
        /// <summary>
        /// 目前连接的参数0--41专线，1--42专线
        /// </summary>
        private int currentLink = 0;
        private int dicNum = 0;
        Common.WebSocket.WebSocket webSocket = null;

        /// <summary>
        /// 已经运行天数
        /// </summary>
        private int hadRundays = 0;
        private int RestartDays = 30;


        //=============================== 事件方法 ===============================
        DataTable tt = null;
        #region 窗体加载事件
        /// <summary>窗体加载事件
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SMainUI_Load(object sender, EventArgs e)
        {
            //log4net.Config.DOMConfigurator.Configure();
            try
            {
                //string temp = SearchService.Common.CommonFun.NumberToChinese("8994703211345");
                //Request Re = new Request();
                //Re.Q = "1500";
                //List<string> list=new List<string>(){"2000"};
                //Re.P = list;

                //string str = SearchService.Common.JsonHelper.SerializeObject(Re);


                //ws = new WebSocket();
                //ws.WebSocketMessageEvent += ws_WebSocketMessageEvent;

                //ws.Start();

                //string input = "select t.files_infor_type_id,t.files_infor_id,t.files_infor_title,z.filepath from TB_FILES_INFOR t, tb_files_receive r, DOC_FILE_2 z where t.files_infor_submit_time < to_date('2015-12-01', 'yyyy-mm-dd')   and r.receive_unit_id = 8 and t.files_infor_id = r.information_id  and z.upflag = 1   and  z.IIPsPath = t.files_infor_id ";
                //Pagination pn = new Pagination();
                //DataSet dt = Pagination.GetDataList_IIPS(input);
                //tt = dt.Tables[0];
                //if (OnChange != null)
                //{
                //    OnChange += new ChangeHander(OnFrontDisconnected);
                //}

                //registEvent();

                this.tsslServerType.Text = "服务器类型：搜索引擎服务器";

                //加载窗体的时候就启动服务
                //this.MenuStartService_Click(null, null);

                this.tsslServerState.Text = "服务端状态：开启";
                this.tsslConnNum.Text = "Udp广播状态: 开启";
                //this.tsslBreakMarketOpen.Text = "行情分解的状态：未开启";

                //监控服务器状态
                //this.timer1.Start();

                //扫描连接情况,长时间没有响应,则重新主动发起连接 中金所认为是攻击..注释掉
                //timerHeartBeat.Start();

                //初始化重启应用程序的时间间隔
                //RestartDays = int.Parse(ConfigFile.GetKeyValue("RestartDays"));
            }
            catch (Exception ex)
            {

                //log.LogHandle.LogError("SMainUI", "OnFrontConnectedEvent", ex);
            }

        }

        private void ws_WebSocketMessageEvent(object sender, EventArgs e)
        {
            beginInvokPrint(sender.ToString(), DateTime.Now);
        }
        #endregion

        #region 锁定事件
        /// <summary>
        /// 锁定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBtnLocked_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (m_SLockedUI == null)
            {
                m_SLockedUI = new LockedUI();
                m_SLockedUI.Owner = this;
            }
            m_SLockedUI.Show();
        }
        #endregion

        #region 启动服务
        /// <summary>启动服务
        /// 启动服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuStartService_Click(object sender, EventArgs e)
        {

            //if (Convert.ToInt32(ConfigFile.GetKeyValue("SendQhData")) >= 1)
            //{
            //开始发送期货数据
            WebsocketThreadRun();
            //SegementThreadRun();
            AllDocIndexThreadRun();
            //SensWriteThreadRun();
            //AllSearchThreadRun();
            //SensSearchThreadRun();
            //QhThreadRun();
            //}
            this.tsslBreakMarketOpen.Text = "服务状态：开启";
        }

        private void Messages_Receive(object sender, EventArgs e)
        {
            string mes = sender.ToString();
            //if (mes == "——全文搜索服务加载已完成。——" || mes == "——敏感词搜索服务加载已完成。——")
            //{
            //    dicNum = dicNum + 1;
            //}

            DateTime dt = DateTime.Now;
            //消息显示
            beginInvokPrint(mes, dt);

            //if (dicNum == 1)
            //{
            //    //SensWriteThreadRun();
            //    ////AllSearchThreadRun();
            //    //SensSearchThreadRun();
            //}else if (dicNum == 2)
            //{
            //    SegementThreadRun();
            //    dicNum = 0;
            //}
        }

        private void SegementMessages_Receive(object sender, EventArgs e)
        {
            string mes = sender.ToString();
            DateTime dt = DateTime.Now;
            //消息显示
            beginInvokPrint(mes, dt);
        }
        #endregion

        #region 重启socket服务
        /// <summary>
        /// 重启socket服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuReStartSocket_Click(object sender, EventArgs e)
        {
            //if (Convert.ToInt32(ConfigFile.GetKeyValue("SendQhData")) >= 1)
            //{
            //api = null;
            //开始发送期货数据
            WebsocketThreadRun();
            //QhThreadRun();
            //}
            this.tsslBreakMarketOpen.Text = "WebSocket服务状态：开启";

        }

        #endregion

        #region 启动分词服务
        private void MenuStartSegmenter_Click(object sender, EventArgs e)
        {
            //SegementManage.Start();
            //if (Convert.ToInt32(ConfigFile.GetKeyValue("SendQhData")) >= 1)
            //{
            //    //api = null;
            //    if (currentLink == 0)
            //    {
            //        currentLink = 1;
            //    }
            //    else
            //    {
            //        currentLink = 0;
            //    }

            //    //注册事件
            //    //SPackApi.CFfexFtdcMduserSpiNetEvent.FreeInstance();

            //    registEvent();
            //    //处理线程
            //    //_QhThread.Abort();
            //    //_QhThread = null;

            //    //_RunPro = null;

            //    //_ZjsApiExport = null;

            //    //_ZjsApiExport = new ZjsApiExport();

            //    ////开始发送期货数据
            //    //QhThreadRun();
            //}
            this.tsslBreakMarketOpen.Text = "行情分解的状态：开启";
        }
        #endregion

        #region 停止行情分解
        /// <summary>
        /// 停止行情分解
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuStopRealTime_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 显示客户端连接数
        /// <summary>
        /// 显示客户端连接数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuDisClient_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 退出 事件
        /// <summary>
        /// 退出 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBtnClose_Click(object sender, EventArgs e)
        {
            AllDocIndex.OptimizeWriter();
            Application.Exit();
        }
        #endregion

        #region 设置事件
        /// <summary>
        /// 设置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBtnConfig_Click(object sender, EventArgs e)
        {
            if (this.m_SSystemSetUI == null)
            {
                this.m_SSystemSetUI = new SSystemSet.SystemSetUI();
                m_SSystemSetUI.ShowDialog();
            }
            else
            {
                m_SSystemSetUI.ShowDialog();
            }
        }

        #endregion

        #region  窗体正在退出响应事件
        /// <summary>
        /// 窗体正在退出响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SMainUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出系统？", "系统信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                AllDocIndex.CloseWriter();
                //Index.AllWrite.Commit();
                //Index.SensWrite.Commit();
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region  关于响应按钮事件MenuAbout_Click
        /// <summary>
        /// 关于响应按钮事件MenuAbout_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAbout_Click(object sender, EventArgs e)
        {
            AboutUI _SAboutUI = new AboutUI();
            _SAboutUI.ShowInTaskbar = false;
            _SAboutUI.ShowDialog();
        }
        #endregion

        private int Num = 0;

        #region 注册的期货DDE的事件

        #region 主线程
        private System.Threading.Thread WebsocketThread = null;

        private System.Threading.Thread AllDocIndexThread = null;

        private void WebsocketThreadRun()
        {
            // _QhThread = null;

            WebsocketThread = new System.Threading.Thread(new System.Threading.ThreadStart(RunWebsocket));
            WebsocketThread.IsBackground = true;

            try
            {
                WebsocketThread.Start();
            }
            catch (Exception)
            {
                try
                {
                    WebsocketThread.Interrupt();
                }
                catch (Exception)
                {
                    WebsocketThread = new System.Threading.Thread(new System.Threading.ThreadStart(RunWebsocket));
                    WebsocketThread.IsBackground = true;
                    WebsocketThread.Start();
                }
            }
        }







        private void AllDocIndexThreadRun()
        {
            // _QhThread = null;

            AllDocIndexThread = new System.Threading.Thread(new System.Threading.ThreadStart(RunAllDocIndex));
            AllDocIndexThread.IsBackground = true;

            try
            {
                AllDocIndexThread.Start();
            }
            catch (Exception)
            {
                try
                {
                    AllDocIndexThread.Interrupt();
                }
                catch (Exception)
                {
                    AllDocIndexThread = new System.Threading.Thread(new System.Threading.ThreadStart(RunAllDocIndex));
                    AllDocIndexThread.IsBackground = true;
                    AllDocIndexThread.Start();
                }
            }
        }


        #region 重连修改代码

        private void DisposeAPIResource(object sender, EventArgs e)
        {
            //_ZjsApiExport.Release();
        }



        private void msgComplete(IAsyncResult a)
        {

            PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);
            //注册事件
            //SPackApi.CFfexFtdcMduserSpiNetEvent.FreeInstance();

            //   registEvent();
            //   //处理线程
            //   _QhThread.Abort();

            //   _QhThread = null;

            //   _RunPro = null;

            //   _ZjsApiExport = null;

            //   _ZjsApiExport = new ZjsApiExport();
            //   //run起来 

            //   string tempLineName = String.Empty;
            //   if (currentLink == 0)
            //   {
            //       tempLineName = "41";
            //   }
            //   else
            //   {
            //       tempLineName = "42";
            //   }
            //   this.BeginInvoke(
            //    _PrintTimeSpanDelegate, new object[] { "事件时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "，期货服务器已经启动，准备重新连接中金所服务器" + tempLineName, null }
            //);
            //   QhThreadRun();
        }

        /// <summary>
        /// 后台API工作线程
        /// </summary>
        private void QhJobThreadRun()
        {
            //api = null;
            EventHandler eh = new EventHandler(DisposeAPIResource);
            AsyncCallback ac = new AsyncCallback(msgComplete);
            IAsyncResult Irs = eh.BeginInvoke(null, null, ac, null);
        }


        #endregion


        //private SPackApi.RunPro _RunPro = null;


        private void RunWebsocket()
        {
            //
            PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);
            //FirstInitQhData();
            this.BeginInvoke(
                _PrintTimeSpanDelegate, new object[] { "……WebSocket服务启动中……", null }
            );

            try
            {

                webSocket = new Common.WebSocket.WebSocket();
                webSocket.WebSocketMessageEvent += new Common.WebSocket.WebSocket.WebSocketMessageEventHandler(Messages_Receive);

                webSocket.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //api.Release();
            }

            //}

            #region 发送伪造数据
            //发送伪造数据
            //if (ConfigFile.GetKeyValue("SendQhData") == "2")
            //{
            //    //一直发送伪造的数据
            //    int Num = 100000;
            //    for (int i = 0; i < Num; i++)
            //    {
            //        FirstInitQhData(i);
            //        Thread.Sleep(1000);
            //    }
            //}
            #endregion


        }



        private void RunAllDocIndex()
        {
            //
            PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);
            //FirstInitQhData();
            //this.BeginInvoke(
            //    _PrintTimeSpanDelegate, new object[] { "……全文搜索写入服务启动中……", null }
            //);

            try
            {

                AllDocIndex.AllDocMessageEvent += new AllDocIndex.AllDocMessageEventHandler(Messages_Receive);
                AllDocIndex.Start();
                ////Index.Search.INDEX_DIR = "Test";
                //Index.AllWrite.Start();
            }
            catch (Exception e)
            {
                //log.LogHandle.LogError("全文索引写入服务异常。", "SMainUI.RunAllWrite", e);
            }
            finally
            {
                //api.Release();
            }
        }


        #endregion

        #region 连接事件
        //private SPackApi.ZjsApiExport _ZjsApiExport = new SPackApi.ZjsApiExport();

        /// <summary>
        /// 重新连接的次数
        /// </summary>
        int connEventCount = -1;
        /// <summary>
        /// 连接事件
        /// </summary>
        private void OnFrontConnectedEvent()
        {
            try
            {
                PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);
                //登陆
                int i = 1;// _ZjsApiExport.ReqUserLogin();
                int sleeptime = 2000;
                //modifyied by janyo at 081226=========
                connEventCount++;
                if (connEventCount != 0)
                {
                    this.BeginInvoke(_PrintTimeSpanDelegate, new object[] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff" )+string.Format(" 期货服务器断连 {0} 次",connEventCount),
                                null }
                                        );
                    //记录日志
                    //SearchService.Common.Log.LogHandle.LogError(string.Format("fserver break {0} 次", connEventCount));
                }
                for (int m = 0; m < 3; m++)//请求三次登陆不上，提示失败
                {
                    // if (sleeptime != 2000)
                    System.Threading.Thread.Sleep(sleeptime);



                    //i = _ZjsApiExport.ReqUserLogin();
                    //i = ReqUserLogin();
                    // sleeptime += 100;

                    if (i == 0)
                    {
                        //成功了.计数器重新设置
                        connEventCount = -1;
                        changeFlage = true;
                        break;
                    }

                }

                //=================================
                if (i == 0)
                {

                    //登陆成功之后再次进行一次 初始化
                    //FirstInitQhData();
                    string tempLineName = String.Empty;
                    if (currentLink == 0)
                    {
                        tempLineName = "41";
                    }
                    else
                    {
                        tempLineName = "42";
                    }
                    IsConnected = true;
                    this.BeginInvoke(_PrintTimeSpanDelegate, new object[] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff" )+ " 连接期货服务器成功,用专线"+tempLineName,
                                null }
                                        );
                    //记录日志
                    //SearchService.Common.Log.LogHandle.LogError("connect succeed");
                }
                else
                {
                    string str = "connect  {0} fail，change on line now ";
                    if (currentLink == 0)
                    {
                        currentLink = 1;
                        str = string.Format(str, "41");
                    }
                    else
                    {
                        currentLink = 0;
                        str = string.Format(str, "42");
                    }

                    this.BeginInvoke(_PrintTimeSpanDelegate, new object[] { str, null });
                    //记录日志
                    //SearchService.Common.Log.LogHandle.LogError(str);
                }
            }
            catch (Exception ex)
            {
                //SearchService.Common.Log.LogHandle.LogError("SMainUI", "OnFrontConnectedEvent", ex);
            }
        }
        #endregion






        /// <summary>
        /// 切换专线时的计时器
        /// </summary>
        System.DateTime dt = new DateTime();
        /// <summary>
        /// 是否已经切换专线的标志
        /// </summary>
        bool changeFlage = true;


        /// <summary>
        /// 是否已经连接上服务器
        /// </summary>
        bool IsConnected = false;


        #region 当断开连接的事件
        /// <summary>
        /// 当断开连接的事件
        /// </summary>
        /// <param name="n"></param>
        private void OnFrontDisconnected(int n)
        {
            ///        n的释义
            ///        0x1001 网络读失败
            ///        0x1002 网络写失败
            ///        0x2001 接收心跳超时
            ///        0x2002 发送心跳失败
            ///        0x2003 收到错误报文

            try
            {
                if (changeFlage)
                    dt = System.DateTime.Now;

                Num = 0;
                IsConnected = false;
                //SearchService.Common.Log.LogHandle.LogError("break conntect:OnFrontDisconnected" + n.ToString("X"));
                PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);

                if (this.IsShareMarketOpen(DateTime.Now))
                {
                    this.BeginInvoke(_PrintTimeSpanDelegate, new object[] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff" )+  " 期货市场交易时间中断 "+n.ToString("X") ,
                                null });
                    //当异常中断时连接到另一台服务器
                    //一定要先换一次
                    if (currentLink == 1)
                    {
                        currentLink = 0;
                    }
                    else
                    {
                        currentLink = 1;
                    }


                    // isMarketOpen = 1;
                    TimeSpan ts = System.DateTime.Now - dt;
                    if (ts.Seconds > changServerLink)
                    {
                        changeFlage = true;

                        if (currentLink == 1)
                        {
                            currentLink = 0;
                        }
                        else
                        {
                            currentLink = 1;
                        }
                    }
                    else
                    {
                        changeFlage = false;
                    }

                    System.Threading.Thread.Sleep(1000);
                    this.BeginInvoke(_PrintTimeSpanDelegate, new object[] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff" )+"原因:"+n.ToString("X")+  " 期货服务器已重连，正在重新连接中金所服务器,使用专线"+currentLink.ToString()+"号" ,
                                null });
                    //_RunPro.RegisterFront(currentLink);
                    QhJobThreadRun();


                }
                else
                {
                    this.BeginInvoke(_PrintTimeSpanDelegate, new object[] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff" )+  " 期货市场已经收市 "+n.ToString("X") ,
                                null });
                    //SearchService.Common.Log.LogHandle.LogError("the market is close");

                    isMarketOpen = 1;
                    //api.Release();//modified by janyo at 081226
                    try
                    {
                        //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(api);
                        //if (api != null)
                        //api = null;
                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke(_PrintTimeSpanDelegate, new object[] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff" )+  " 释放中金所API资源错误",
                                null });
                        try
                        {
                            //api = null;
                        }
                        catch
                        {
                        }
                        //  _RunPro = null;
                        // throw;
                    }
                }

                #region 登陆连接时,一定时间内,自动切换连接的线路



                //      int sleeptime = 2000;
                //      int flag = 0;
                ////  RESART:
                //      int count = (int)((changServerLink * 1000) / sleeptime); //计算重连次数
                //      for (int m = 0; m < count; m++)//请求N次登陆不上，更换专线
                //      {
                //          //if (sleeptime != 400)


                //          Thread.Sleep(sleeptime);


                //          //flag = _ZjsApiExport.ReqUserLogin();
                //          ////sleeptime += 100;//?如果累加.会越等得越久
                //          //if (flag == 0)
                //          //{
                //          //    break;
                //          //}



                //      }

                //      //如果还没有连接上 切换线,再连接
                //      if (flag == 1)
                //      {
                //          if (currentLink == 1)
                //          {
                //              currentLink = 0;
                //          }
                //          else
                //          {
                //              currentLink = 1;
                //          }
                //          //goto RESART;
                //      }



                ////注册事件
                //SPackApi.CFfexFtdcMduserSpiNetEvent.FreeInstance();
                //registEvent();
                ////处理线程
                //_QhThread.Abort();
                //_QhThread = null;

                //_RunPro = null;

                //_ZjsApiExport = null;

                //_ZjsApiExport = new ZjsApiExport();
                ////run起来 
                //QhThreadRun();
                #endregion

            }
            catch (Exception ex)
            {
                //SearchService.Common.Log.LogHandle.LogError("ServerUI", "OnFrontDisconnected", ex);
            }
        }
        #endregion

        #region 当长时间未收到报文时，该方法被调用。
        /// <summary>
        /// 当长时间未收到报文时，该方法被调用。
        /// </summary>
        /// <param name="n"></param>
        public void OnHeartBeatWarning(int n)
        {
            //不需要处理
            PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);
            this.BeginInvoke(_PrintTimeSpanDelegate, new object[] { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " 长时间未收到数据.........", null });
        }
        #endregion

        #region 当客户端发出登录请求之后，该方法会被调用，通知客户端登录是否成功







        #endregion

        #region 当客户端发出退出请求之后，该方法会被调用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRspUserLogout"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        //private void OnRspUserLogout(ref RefRspUserLogoutField pRspUserLogout, ref RefRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        //private void OnRspUserLogout(ThostFtdcUserLogoutField pUserLogout, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        //{
        //    PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);
        //    this.BeginInvoke(_PrintTimeSpanDelegate, new object[] { "正在发出登出请求........." +
        //        "ErrorID" + pRspInfo.ErrorID.ToString() +
        //        "Error" + pRspInfo.ErrorMsg + "UserId" + pUserLogout.UserID
        //        + "BrokerID" + pUserLogout.BrokerID ,
        //                        null }
        //                            );
        //    SearchService.Common.Log.LogHandle.LogError("send request of logout.........,ErrorID" + pRspInfo.ErrorID.ToString() +
        //        "Error" + pRspInfo.ErrorMsg + "UserId" + pUserLogout.UserID
        //        + "BrokerID" + pUserLogout.BrokerID);

        //    #region 不需要处理
        //    //if (pRspInfo.ErrorID == 0)
        //    //{
        //    //    if (this.IsShareMarketOpen(DateTime.Now))
        //    //    {
        //    //        //重新登录 
        //    //        _ZjsApiExport.ReqUserLogin();

        //    //        ////_ZjsApiExport.Release();
        //    //        //////注册事件
        //    //        //////SPackApi.CFfexFtdcMduserSpiNetEvent.FreeInstance();
        //    //        //////registEvent();

        //    //        ////处理线程
        //    //        //_QhThread.Abort();
        //    //        //_QhThread = null;
        //    //        //_RunPro = null;
        //    //        //_ZjsApiExport = null;
        //    //        //_ZjsApiExport = new ZjsApiExport();
        //    //        ////run起来 
        //    //        //QhThreadRun();
        //    //    }
        //    //}
        //    #endregion
        //}
        #endregion

        #region 行情通知，行情服务器会主动通知客户端
        /// <summary>
        /// 行情通知，行情服务器会主动通知客户端
        /// </summary>
        /// <param name="pDepthMarketData"></param>
        //private void OnRtnDepthMarketData(ThostFtdcDepthMarketDataField pDepthMarketData)
        //{
        //    try
        //    {
        //        DateTime _start = DateTime.Now;
        //        if (ChangeFlag == "GZMF")
        //        {
        //            //PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);
        //            #region 股指期货
        //            //执行异步方法进行socket传输
        //            UdpFutData _FutData = new UdpFutData();
        //            _FutData.Stockno = pDepthMarketData.InstrumentID.Trim();
        //            _FutData.Yclose = pDepthMarketData.PreClosePrice;
        //            _FutData.PreSettlementPrice = pDepthMarketData.PreSettlementPrice;
        //            //开盘
        //            if (pDepthMarketData.OpenPrice != -double.MinValue)
        //            {
        //                _FutData.Open = pDepthMarketData.OpenPrice;
        //            }

        //            //_FutData.Time
        //            if (pDepthMarketData.BidPrice1 != -double.MinValue)
        //            {

        //                _FutData.Buyprice1 = pDepthMarketData.BidPrice1;
        //            }


        //            _FutData.Buyvol1 = pDepthMarketData.BidVolume1;


        //            if (pDepthMarketData.AskPrice1 != -double.MinValue)
        //            {

        //                _FutData.Sellprice1 = pDepthMarketData.AskPrice1;
        //            }

        //            _FutData.Sellvol1 = pDepthMarketData.AskVolume1;

        //            //最新价
        //            if (pDepthMarketData.LastPrice != -double.MinValue)
        //            {

        //                _FutData.Lasttrade = pDepthMarketData.LastPrice;
        //            }



        //            _FutData.PreOpenInterest = pDepthMarketData.PreOpenInterest;


        //            if (pDepthMarketData.OpenInterest != -double.MinValue)
        //            {
        //                _FutData.OpenInterest = pDepthMarketData.OpenInterest;
        //            }
        //            //最高价
        //            if (pDepthMarketData.HighestPrice != -double.MinValue)
        //            {

        //                _FutData.HighestPrice = pDepthMarketData.HighestPrice;
        //            }

        //            //当日最低
        //            if (pDepthMarketData.LowestPrice != -double.MinValue)
        //            {

        //                _FutData.LowestPrice = pDepthMarketData.LowestPrice;

        //            }

        //            //收盘价
        //            if (pDepthMarketData.ClosePrice != -double.MinValue)
        //            {
        //                _FutData.ClosePrice = pDepthMarketData.ClosePrice;
        //            }
        //            //结算价
        //            if (pDepthMarketData.SettlementPrice != -double.MinValue)
        //            {
        //                _FutData.SettlementPrice = pDepthMarketData.SettlementPrice;
        //            }

        //            //均价
        //            double dbAvg = pDepthMarketData.AveragePrice;
        //            if (dbAvg != -double.MinValue)
        //            {
        //                _FutData.AvgPrice = dbAvg;
        //            }

        //            //跌停价
        //            _FutData.LowerLimitPrice = pDepthMarketData.LowerLimitPrice;

        //            //涨停价
        //            _FutData.UpperLimitPrice = pDepthMarketData.UpperLimitPrice;

        //            //总金额
        //            if (!double.IsInfinity(pDepthMarketData.Turnover))
        //            {
        //                _FutData.Amount = pDepthMarketData.Turnover;
        //            }
        //            //总量 
        //            _FutData.Volume = pDepthMarketData.Volume;

        //            _FutData.Date = pDepthMarketData.TradingDay;
        //            _FutData.Time = pDepthMarketData.UpdateTime + "." + pDepthMarketData.UpdateMillisec.ToString("D3");

        //            //_FutData.PriceGap = pDepthMarketData.CurrDelta;

        //            BroadCastUdpData(_FutData);
        //            #endregion
        //        }
        //        else if (ChangeFlag == "DLMF")
        //        {
        //            #region 大连商品期货
        //            MerFutData DLFutData = new MerFutData();

        //            DLFutData.ContractID = pDepthMarketData.InstrumentID.Trim();
        //            DLFutData.PreClosePrice = pDepthMarketData.PreClosePrice;
        //            DLFutData.PreClearPrice = pDepthMarketData.PreSettlementPrice;

        //            //开盘
        //            if (pDepthMarketData.OpenPrice != -double.MinValue)
        //            {
        //                DLFutData.OpenPrice = pDepthMarketData.OpenPrice;
        //            }

        //            //_FutData.Time
        //            if (pDepthMarketData.BidPrice1 != -double.MinValue)
        //            {

        //                DLFutData.Buyprice1 = pDepthMarketData.BidPrice1;
        //            }


        //            DLFutData.Buyvol1 = pDepthMarketData.BidVolume1;


        //            if (pDepthMarketData.AskPrice1 != -double.MinValue)
        //            {

        //                DLFutData.Sellprice1 = pDepthMarketData.AskPrice1;
        //            }

        //            DLFutData.Sellvol1 = pDepthMarketData.AskVolume1;

        //            //最新价
        //            if (pDepthMarketData.LastPrice != -double.MinValue)
        //            {

        //                DLFutData.Lasttrade = pDepthMarketData.LastPrice;
        //            }



        //            DLFutData.PreOpenInterest = pDepthMarketData.PreOpenInterest;


        //            if (pDepthMarketData.OpenInterest != -double.MinValue)
        //            {
        //                DLFutData.OpenInterest = pDepthMarketData.OpenInterest;
        //            }
        //            //最高价
        //            if (pDepthMarketData.HighestPrice != -double.MinValue)
        //            {

        //                DLFutData.HighPrice = pDepthMarketData.HighestPrice;
        //            }

        //            //当日最低
        //            if (pDepthMarketData.LowestPrice != -double.MinValue)
        //            {

        //                DLFutData.LowPrice = pDepthMarketData.LowestPrice;

        //            }

        //            //收盘价
        //            if (pDepthMarketData.ClosePrice != -double.MinValue)
        //            {
        //                DLFutData.ClosePrice = pDepthMarketData.ClosePrice;
        //            }
        //            //结算价
        //            if (pDepthMarketData.SettlementPrice != -double.MinValue)
        //            {
        //                DLFutData.ClearPrice = pDepthMarketData.SettlementPrice;
        //            }

        //            //均价
        //            double dbAvg = pDepthMarketData.AveragePrice;
        //            if (dbAvg != -double.MinValue)
        //            {
        //                DLFutData.AveragePrice = dbAvg;
        //            }

        //            //跌停价
        //            DLFutData.LowerLimit = pDepthMarketData.LowerLimitPrice;

        //            //涨停价
        //            DLFutData.UpperLimit = pDepthMarketData.UpperLimitPrice;

        //            //总金额
        //            if (!double.IsInfinity(pDepthMarketData.Turnover))
        //            {
        //                DLFutData.Amount = pDepthMarketData.Turnover;
        //            }
        //            //总量 
        //            DLFutData.Volume = pDepthMarketData.Volume;

        //            DLFutData.Date = pDepthMarketData.TradingDay;
        //            DLFutData.Time = pDepthMarketData.UpdateTime + "." + pDepthMarketData.UpdateMillisec.ToString("D3");


        //            //DLFutData.uUpdateTime = pDepthMarketData.UpdateTime;

        //            //涨跌=当前价-昨收
        //            //DLFutData.Chg = DLFutData.Lasttrade - DLFutData.PreClearPrice;
        //            //涨跌幅
        //            //DLFutData.ChgPct = DLFutData.Chg / DLFutData.PreClearPrice;


        //            BroadCastDLUdpData(DLFutData);
        //            #endregion
        //        }
        //        else if (ChangeFlag == "ZZMF")
        //        {
        //            #region 郑州商品期货
        //            MerFutData ZZFutData = new MerFutData();


        //            ZZFutData.ContractID = pDepthMarketData.InstrumentID.Trim();
        //            ZZFutData.PreClosePrice = pDepthMarketData.PreClosePrice;
        //            ZZFutData.PreClearPrice = pDepthMarketData.PreSettlementPrice;

        //            //开盘
        //            if (pDepthMarketData.OpenPrice != -double.MinValue)
        //            {
        //                ZZFutData.OpenPrice = pDepthMarketData.OpenPrice;
        //            }

        //            //_FutData.Time
        //            if (pDepthMarketData.BidPrice1 != -double.MinValue)
        //            {

        //                ZZFutData.Buyprice1 = pDepthMarketData.BidPrice1;
        //            }


        //            ZZFutData.Buyvol1 = pDepthMarketData.BidVolume1;


        //            if (pDepthMarketData.AskPrice1 != -double.MinValue)
        //            {

        //                ZZFutData.Sellprice1 = pDepthMarketData.AskPrice1;
        //            }

        //            ZZFutData.Sellvol1 = pDepthMarketData.AskVolume1;

        //            //最新价
        //            if (pDepthMarketData.LastPrice != -double.MinValue)
        //            {

        //                ZZFutData.Lasttrade = pDepthMarketData.LastPrice;
        //            }



        //            ZZFutData.PreOpenInterest = pDepthMarketData.PreOpenInterest;


        //            if (pDepthMarketData.OpenInterest != -double.MinValue)
        //            {
        //                ZZFutData.OpenInterest = pDepthMarketData.OpenInterest;
        //            }
        //            //最高价
        //            if (pDepthMarketData.HighestPrice != -double.MinValue)
        //            {

        //                ZZFutData.HighPrice = pDepthMarketData.HighestPrice;
        //            }

        //            //当日最低
        //            if (pDepthMarketData.LowestPrice != -double.MinValue)
        //            {

        //                ZZFutData.LowPrice = pDepthMarketData.LowestPrice;

        //            }

        //            //收盘价
        //            if (pDepthMarketData.ClosePrice != -double.MinValue)
        //            {
        //                ZZFutData.ClosePrice = pDepthMarketData.ClosePrice;
        //            }
        //            //结算价
        //            if (pDepthMarketData.SettlementPrice != -double.MinValue)
        //            {
        //                ZZFutData.ClearPrice = pDepthMarketData.SettlementPrice;
        //            }

        //            //均价
        //            double dbAvg = pDepthMarketData.AveragePrice;
        //            if (dbAvg != -double.MinValue)
        //            {
        //                ZZFutData.AveragePrice = dbAvg;
        //            }

        //            //跌停价
        //            ZZFutData.LowerLimit = pDepthMarketData.LowerLimitPrice;

        //            //涨停价
        //            ZZFutData.UpperLimit = pDepthMarketData.UpperLimitPrice;

        //            //总金额
        //            if (!double.IsInfinity(pDepthMarketData.Turnover))
        //            {
        //                ZZFutData.Amount = pDepthMarketData.Turnover;
        //            }
        //            //总量 
        //            ZZFutData.Volume = pDepthMarketData.Volume;

        //            ZZFutData.Date = pDepthMarketData.TradingDay;
        //            ZZFutData.Time = pDepthMarketData.UpdateTime + "." + pDepthMarketData.UpdateMillisec.ToString("D3");

        //            BroadCastZZUdpData(ZZFutData);
        //            #endregion
        //        }
        //        else if (ChangeFlag == "SHMF")
        //        {
        //            #region 上海商品期货
        //            MerFutData SHFutData = new MerFutData();

        //            SHFutData.ContractID = pDepthMarketData.InstrumentID.Trim();
        //            SHFutData.PreClosePrice = pDepthMarketData.PreClosePrice;
        //            SHFutData.PreClearPrice = pDepthMarketData.PreSettlementPrice;

        //            //开盘
        //            if (pDepthMarketData.OpenPrice != -double.MinValue)
        //            {
        //                SHFutData.OpenPrice = pDepthMarketData.OpenPrice;
        //            }

        //            //_FutData.Time
        //            if (pDepthMarketData.BidPrice1 != -double.MinValue)
        //            {

        //                SHFutData.Buyprice1 = pDepthMarketData.BidPrice1;
        //            }


        //            SHFutData.Buyvol1 = pDepthMarketData.BidVolume1;


        //            if (pDepthMarketData.AskPrice1 != -double.MinValue)
        //            {

        //                SHFutData.Sellprice1 = pDepthMarketData.AskPrice1;
        //            }

        //            SHFutData.Sellvol1 = pDepthMarketData.AskVolume1;

        //            //最新价
        //            if (pDepthMarketData.LastPrice != -double.MinValue)
        //            {

        //                SHFutData.Lasttrade = pDepthMarketData.LastPrice;
        //            }



        //            SHFutData.PreOpenInterest = pDepthMarketData.PreOpenInterest;


        //            if (pDepthMarketData.OpenInterest != -double.MinValue)
        //            {
        //                SHFutData.OpenInterest = pDepthMarketData.OpenInterest;
        //            }
        //            //最高价
        //            if (pDepthMarketData.HighestPrice != -double.MinValue)
        //            {

        //                SHFutData.HighPrice = pDepthMarketData.HighestPrice;
        //            }

        //            //当日最低
        //            if (pDepthMarketData.LowestPrice != -double.MinValue)
        //            {

        //                SHFutData.LowPrice = pDepthMarketData.LowestPrice;

        //            }

        //            //收盘价
        //            if (pDepthMarketData.ClosePrice != -double.MinValue)
        //            {
        //                SHFutData.ClosePrice = pDepthMarketData.ClosePrice;
        //            }
        //            //结算价
        //            if (pDepthMarketData.SettlementPrice != -double.MinValue)
        //            {
        //                SHFutData.ClearPrice = pDepthMarketData.SettlementPrice;
        //            }

        //            //均价
        //            double dbAvg = pDepthMarketData.AveragePrice;
        //            if (dbAvg != -double.MinValue)
        //            {
        //                SHFutData.AveragePrice = dbAvg;
        //            }

        //            //跌停价
        //            SHFutData.LowerLimit = pDepthMarketData.LowerLimitPrice;

        //            //涨停价
        //            SHFutData.UpperLimit = pDepthMarketData.UpperLimitPrice;

        //            //总金额
        //            if (!double.IsInfinity(pDepthMarketData.Turnover))
        //            {
        //                SHFutData.Amount = pDepthMarketData.Turnover;
        //            }
        //            //总量 
        //            SHFutData.Volume = pDepthMarketData.Volume;

        //            SHFutData.Date = pDepthMarketData.TradingDay;
        //            SHFutData.Time = pDepthMarketData.UpdateTime + "." + pDepthMarketData.UpdateMillisec.ToString("D3");
        //            //SHFutData.PTrans=pDepthMarketData.
        //            //涨跌=当前价-昨收
        //            //SHFutData.Chg = DLFutData.Lasttrade - DLFutData.ClearPrice;
        //            //    //涨跌幅
        //            //SHFutData.ChgPct = DLFutData.Chg / DLFutData.PreClosePrice;


        //            BroadCastSHUdpData(SHFutData);
        //            #endregion
        //        }

        //    }
        //    catch (Exception _ex)
        //    {
        //        SearchService.Common.Log.LogHandle.LogError("SMainUI", "OnRtnDepthMarketData", _ex);
        //    }
        //}

        public DateTime GetQhDateTime(string _Date, string _Time)
        {
            int _Year = Convert.ToInt32(_Date.Substring(0, 4));
            int _Month = Convert.ToInt32(_Date.Substring(4, 2));
            int _Day = Convert.ToInt32(_Date.Substring(6, 2));

            int _HH;
            int _MM;
            int _SS;
            if (_Time.Length < 8)
            {
                _HH = Convert.ToInt32(_Time.Substring(0, 1));
                _MM = Convert.ToInt32(_Time.Substring(2, 2));
                _SS = Convert.ToInt32(_Time.Substring(4, 2));
            }
            else
            {
                _HH = Convert.ToInt32(_Time.Substring(0, 2));
                _MM = Convert.ToInt32(_Time.Substring(3, 2));
                _SS = Convert.ToInt32(_Time.Substring(6, 2));
            }
            DateTime _NowDt = new DateTime(_Year, _Month, _Day, _HH, _MM, _SS);
            return _NowDt;
        }
        #endregion

        #endregion

        //=============================== 私有方法 ===============================
        #region 打印刷新情况
        public void beginInvokPrint(string _str, DateTime _dt)
        {

            if (this.rtbMessage.InvokeRequired)
            {
                PrintTimeSpanDelegate _PrintTimeSpanDelegate = new PrintTimeSpanDelegate(PrintTimeSpan);
                this.rtbMessage.BeginInvoke(_PrintTimeSpanDelegate, new object[] { _str, _dt });
            }
            else
            {
                PrintTimeSpan(_str, _dt);
            }
        }



        public delegate void PrintTimeSpanDelegate(string _str, DateTime _dt);
        /// <summary>
        /// 打印刷新情况
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_dt"></param>
        public void PrintTimeSpan(string _str, DateTime _dt)
        {
            if (_dt != null)
            {
                rtbMessage.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "  " + _str + "\r\n");
            }
            else
            {
                rtbMessage.AppendText(_dt.ToString("yyyy/MM/dd HH:mm:ss.fff") + "  " + _str + "\r\n");

            }
        }


        #endregion

        #region 当Listbox中的记录超过100条时,进行清理
        /// <summary>
        /// 当Listbox中的记录超过100条时,进行清理
        /// </summary>
        private void ConformListNum()
        {
            if (this.listBox1.Items.Count > 100)
            {
                this.listBox1.Items.Clear();
            }
        }
        #endregion

        #region 开启界面时对期货数据进行一个初始化
        private void FirstInitQhData()
        {
            //对应四个代码

        }

        //伪造数据
        private void FirstInitQhData(int num)
        {



        }
        #endregion

        private byte[] _byte = null;
        //private IMarketTransferProtocol _IMarketTransferProtocol = TransferProtocolFacotry.GetTransferProtocolObject();

        #region 广播数据



        #endregion

        #region 判断是否是开盘时间
        /// <summary>
        /// 判断是否是开盘时间
        /// </summary>
        /// <returns></returns>
        private bool IsShareMarketOpen(DateTime _dt)
        {
            //if (_dt >= Convert.ToDateTime(this.m_MarketingAmStartTime) &&
            //    _dt <= Convert.ToDateTime(this.m_MarketingAmEndTim))
            //{
            //    return true;
            //}
            //else
            //{
            //    if (_dt >= Convert.ToDateTime(this.m_MarketingPmStartTime) &&
            //    _dt <= Convert.ToDateTime(this.m_MarketingPmEndTime))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            return false;
            //    }
            //}


        }
        #endregion

        #region 注册事件
        private void registEvent()
        {
            #region 注册事件
            //注册期货DDE事件


            //建立起通信连接时,当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
            // SPackApi.CFfexFtdcMduserSpiNetEvent.Instance().OnFrontConnectedEventHandle -= new OnFrontConnectedCallback(OnFrontConnectedEvent);

            //SPackApi.CFfexFtdcMduserSpiNetEvent.Instance().OnFrontConnectedEventHandle += new OnFrontConnectedCallback(OnFrontConnectedEvent);


            ////当长时间未收到报文时，该方法被调用。心跳超时警告。当长时间未收到报文时，该方法被调用。
            //SPackApi.CFfexFtdcMduserSpiNetEvent.Instance().OnHeartBeatWarningEventHandle += new OnHeartBeatWarningCallback(OnHeartBeatWarning);

            ////通信连接断开
            //SPackApi.CFfexFtdcMduserSpiNetEvent.Instance().OnFrontDisconnectedEventHandle += new OnFrontDisconnectedCallback(OnFrontDisconnected);

            ////针对用户请求的出错通知
            //SPackApi.CFfexFtdcMduserSpiNetEvent.Instance().OnRspErrorEventHandle += new OnRspErrorCallback(OnRspError);

            //// 当客户端发出登录请求之后，该方法会被调用，通知客户端登录是否成功
            //SPackApi.CFfexFtdcMduserSpiNetEvent.Instance().OnRspUserLoginEventHandle += new OnRspUserLoginCallback(OnRspUserLogin);

            ////用户退出应答
            //SPackApi.CFfexFtdcMduserSpiNetEvent.Instance().OnRspUserLogoutEventHandle += new OnRspUserLogoutCallback(OnRspUserLogout);

            ////行情通知，行情服务器会主动通知客户端
            //SPackApi.CFfexFtdcMduserSpiNetEvent.Instance().OnRtnDepthMarketDataEventHandle += new OnRtnDepthMarketDataCallback(OnRtnDepthMarketData);

            #endregion
        }
        #endregion

        //private bool isRelease = false; 
        private int nowHour = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                #region 设置应用程序在运行指定的天数后重启
                if (DateTime.Now.Hour == 23 && nowHour != 23)
                {
                    hadRundays++;
                    nowHour = 23;
                    if (hadRundays >= RestartDays)
                    {
                        Application.Restart();
                    }
                }
                else if (DateTime.Now.Hour != 23 && nowHour == 23)
                {
                    nowHour = 1;
                }
                #endregion

                if (isMarketOpen == 1)
                {
                    if (this.IsShareMarketOpen(DateTime.Now))
                    {
                        // GC.Collect();//modified by janyo at 090227

                        isMarketOpen = 0;
                        //处理线程
                        WebsocketThread.Abort();
                        AllDocIndexThread.Abort();
                        // _QhThread.Abort();
                        //SearchService.Common.Log.LogHandle.LogError("WebsocketThread.Abort();");
                        ////_QhThread = null;
                        //SearchService.Common.Log.LogHandle.LogError("_QhThread = null;");
                        //SearchService.Common.Log.LogHandle.LogError("SegementThread.Abort();");
                        ////_QhThread = null;
                        //SearchService.Common.Log.LogHandle.LogError("SegementThread = null;");
                        //SearchService.Common.Log.LogHandle.LogError("SearchThread.Abort();");
                        ////_QhThread = null;
                        //SearchService.Common.Log.LogHandle.LogError("SearchThread = null;");

                        //run起来 

                        //SearchThreadRun();

                        //SearchService.Common.Log.LogHandle.LogError("QhThreadRun();");
                        //SearchService.Common.Log.LogHandle.LogError("QhThreadRun();");
                        //SearchService.Common.Log.LogHandle.LogError("QhThreadRun();");
                    }
                }
            }
            catch (Exception ex)
            {
                isMarketOpen = 1;

                Common.Log.Logger.WriteError("", ex);
            }


            //if (isRelease)
            //{
            //    isRelease = false;
            //    //处理线程
            //    _QhThread.Abort();
            //    SearchService.Common.Log.LogHandle.LogError("_QhThread.Abort();");
            //    _QhThread = null;
            //    SearchService.Common.Log.LogHandle.LogError("_QhThread = null;");

            //    _RunPro = null;
            //    SearchService.Common.Log.LogHandle.LogError("_RunPro = null;");
            //    _ZjsApiExport = null;
            //    SearchService.Common.Log.LogHandle.LogError("_ZjsApiExport = null;");
            //    _ZjsApiExport = new ZjsApiExport();
            //    SearchService.Common.Log.LogHandle.LogError("_ZjsApiExport = new ZjsApiExport();");

            //    //run起来 
            //    QhThreadRun();
            //    SearchService.Common.Log.LogHandle.LogError("QhThreadRun();");
            //}
        }


        //用于测试异常断开.同时会自动重新连接.换线
        private void 异常断开测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SegementThreadRun();
            //SegementManage.ReloadDict();
            //连接上了.
            //if (changeFlage)
            //{
            //    changeFlage = false;


            //if (_ZjsApiExport == null)
            //{
            //    _ZjsApiExport = new ZjsApiExport();
            //    _ZjsApiExport.ReqUserLogout();//用户主动退出
            //}
            //System.Threading.Thread.Sleep(2000);
            //if (OnChange != null)
            //{
            //    this.OnChange(999);
            //}

            //  OnFrontDisconnected(99999);
            //}
        }

        private void MenuReloadDIC_Click(object sender, EventArgs e)
        {
            //SegementManage.ReloadDict();
        }

        BatchInsert frmBatchInsert = new BatchInsert();
        private void MenuBatch_Click(object sender, EventArgs e)
        {

            fbdFolderBrowser.ShowDialog();
            string path = fbdFolderBrowser.SelectedPath;
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (frmBatchInsert.IsDisposed)
                {
                    frmBatchInsert = new BatchInsert();
                }
                frmBatchInsert.Show();
                frmBatchInsert.Progress = 0;
                frmBatchInsert.Project = "遍历文件夹进度";
                List<FileInfo> dc = FindFoldersAndFiles(path);
                frmBatchInsert.Progress = 1;

                frmBatchInsert.Project = "建立全文索引文件进度";
                string IndexPath = ConfigurationManager.AppSettings["AllWordsIndexPath"];
                string dicXMLFullName = ConfigurationManager.AppSettings["AllWordsDicConfigPath"];
                //AddIndex(dc, IndexPath, dicXMLFullName);//创建索引文件
                AddNewAllIndex(dc, IndexPath, dicXMLFullName);//创建索引文件（在原有的基础上添加字段并按年份分割索引文件夹）
                                                              //frmBatchInsert.Project = "建立敏感词索引文件进度";
                                                              //IndexPath = ConfigurationManager.AppSettings["SensWordsIndexPath"];
                                                              //dicXMLFullName = ConfigurationManager.AppSettings["SensWordsDicConfigPath"];
                                                              //AddIndex(dc, IndexPath, dicXMLFullName);

                frmBatchInsert.Close();
            }
            else
            {
                MessageBox.Show("所选文件夹为空！");
            }
        }




        private void MenuAlarmIndex_Click(object sender, EventArgs e)
        {
            if (frmBatchInsert.IsDisposed)
            {
                frmBatchInsert = new BatchInsert();
            }
            frmBatchInsert.Show();
            frmBatchInsert.Progress = 0;
            frmBatchInsert.Project = "数据库中获取处警记录信息！";
            int pageCount;
            int totalRecord;
            //接处警警情编号，警综系统编号，警情状态编号，核实警情性质，原始警情性质
            //--警情级别，警情内容，警综内容，报警时间
            //--报警电话，报警人，死亡人数，受伤人数，损失金额
            //--逮捕嫌疑人数，救援人数，接警来源 0-市局 1-宝安 2-龙岗 3-警综，损失总金额
            //--分局编号，派出所编号，社区编号，重复警情编号，关联警情编号
            //string input = "select t.alarmid,t.jzsystemid,t.status,t.alarmsubtypeid,t.alarmoldtype,t.alarmlevel,t.content,t.jzdocontent,t.alarmtime,t.callerphone,t.caller,t.deathnum,t.injurednum,t.losscash,t.arrestsuspectnum,t.rescuepepolenum,t.centertype,t.losscashtotal,t.subbureausid,t.stationid,t.areaid,t.dupalarmid,t.relatealarmid from A_ALARM_RECORDS t where t.centertype!='3'";
            string input = "select t.alarmid,t.jzsystemid,t.status,t.alarmsubtypeid,t.alarmoldtype,t.alarmlevel," +
                            "t.content,t.jzdocontent,t.alarmtime,t.callerphone,t.caller,t.deathnum,t.injurednum," +
                            "t.losscash,t.arrestsuspectnum,t.rescuepepolenum,t.centertype,t.losscashtotal," +
                            "t.subbureausid,t.stationid,t.areaid,t.dupalarmid,t.relatealarmid,h.address,fk.feedback,r.incsource " +
                            "from A_ALARM_RECORDS t left join a_alarm_addr h on t.alarmid=h.alarmid " +
                            "left join a_alarm_fk fk on t.alarmid=fk.incno " +
                            "left join a_alarm_resources r on t.alarmid=r.incno";

            //Pagination pn = new Pagination();
            //DataSet dt = Pagination.GetDataList_IIPS(input);

            //frmBatchInsert.Progress = 1;
            //DataTable dtResult = dt.Tables[0];
            //totalRecord = dtResult.Rows.Count;
            ////List<Model.AlarmInfor> InforList = new List<Model.AlarmInfor>();
            //Dictionary<string, Model.AlarmInfor> InforList = new Dictionary<string, Model.AlarmInfor>();
            //frmBatchInsert.Progress = 0;
            //int j = 1;
            //for (int i = 0; i < totalRecord; i++)
            //{
            //    string alarmid = dtResult.Rows[i]["alarmid"].ToString();

            //    if (!string.IsNullOrWhiteSpace(alarmid))
            //    {
            //        Model.AlarmInfor alarmInfor = new Model.AlarmInfor();

            //        alarmInfor.AlarmID = alarmid; //dtResult.Rows[i]["alarmid"].ToString();
            //        if (!InforList.ContainsKey(alarmInfor.AlarmID))
            //        {
            //            alarmInfor.ID = j;
            //            alarmInfor.Status = dtResult.Rows[i]["status"].ToString();
            //            alarmInfor.AlarmSubTypeID = StringToInt(dtResult.Rows[i]["alarmsubtypeid"].ToString());
            //            alarmInfor.AlarmoldType = StringToInt(dtResult.Rows[i]["alarmoldtype"].ToString());
            //            alarmInfor.AlarmLevel = dtResult.Rows[i]["alarmlevel"].ToString();
            //            alarmInfor.Content = dtResult.Rows[i]["content"].ToString();
            //            alarmInfor.JZDOContent = dtResult.Rows[i]["jzdocontent"].ToString();
            //            alarmInfor.AlarmTime = Convert.ToDateTime(dtResult.Rows[i]["alarmtime"].ToString());
            //            alarmInfor.CallerPhone = dtResult.Rows[i]["callerphone"].ToString();
            //            alarmInfor.Caller = dtResult.Rows[i]["caller"].ToString();
            //            //string temp = string.IsNullOrWhiteSpace(dtResult.Rows[i]["deathnum"].ToString()) ? "0" : dtResult.Rows[i]["deathnum"].ToString();
            //            alarmInfor.DeathNum = StringToInt(dtResult.Rows[i]["deathnum"].ToString());
            //            alarmInfor.InjuredNum = StringToInt(dtResult.Rows[i]["injurednum"].ToString());
            //            alarmInfor.LossCash = StringToInt(dtResult.Rows[i]["losscash"].ToString());
            //            alarmInfor.ArrestSuspectNum = StringToInt(dtResult.Rows[i]["arrestsuspectnum"].ToString());
            //            alarmInfor.CenterType = StringToInt(dtResult.Rows[i]["centertype"].ToString());
            //            alarmInfor.LossCashTotal = StringToInt(dtResult.Rows[i]["losscashtotal"].ToString());
            //            alarmInfor.SubbureausID = StringToInt(dtResult.Rows[i]["subbureausid"].ToString());
            //            alarmInfor.StationID = StringToInt(dtResult.Rows[i]["stationid"].ToString());
            //            alarmInfor.AreaID = StringToInt(dtResult.Rows[i]["areaid"].ToString());
            //            alarmInfor.DupalarmID = dtResult.Rows[i]["dupalarmid"].ToString();
            //            alarmInfor.RelateAlarmID = dtResult.Rows[i]["relatealarmid"].ToString();
            //            alarmInfor.RescuePepoleNum = StringToInt(dtResult.Rows[i]["rescuepepolenum"].ToString());
            //            alarmInfor.OccurADD = dtResult.Rows[i]["address"].ToString();

            //            string feedback = dtResult.Rows[i]["feedback"].ToString();
            //            alarmInfor.FeedBack.Add(feedback);
            //            alarmInfor.AlarmResTypeID = StringToInt(dtResult.Rows[i]["incsource"].ToString());
            //            InforList.Add(alarmInfor.AlarmID, alarmInfor);
            //            j = j + 1;
            //        }
            //        else
            //        {
            //            string feedback = dtResult.Rows[i]["feedback"].ToString().Trim();
            //            //Model.AlarmInfor infor=InforList[feedback];
            //            if (!InforList[alarmInfor.AlarmID].FeedBack.Contains(feedback))
            //            {
            //                InforList[alarmInfor.AlarmID].FeedBack.Add(feedback);
            //            }

            //        }
            //    }
            //alarmInfor.Feedback
            //string inputselect = "select h.address from a_alarm_addr h where h.alarmid='" + alarmInfor.AlarmID + "'";
            ////Pagination pn = new Pagination();
            //DataSet dts = Pagination.GetDataList_IIPS(inputselect);
            //if (dts.Tables.Count > 0)
            //{
            //    DataTable dtb = dts.Tables[0];
            //    if (dtb.Rows.Count > 0)
            //    {
            //        for (int j = 0; j < dtb.Rows.Count; j++)
            //        {
            //            alarmInfor.Content = alarmInfor.Content + ";" + dtb.Rows[j]["address"].ToString();
            //        }
            //    }
            //}
            //inputselect = "select fk.feedback from a_alarm_fk fk where fk.incno='" + alarmInfor.AlarmID + "'";
            //DataSet dtsfk = Pagination.GetDataList_IIPS(inputselect);
            //if (dtsfk.Tables.Count > 0)
            //{
            //    DataTable dtbfk = dtsfk.Tables[0];
            //    if (dtbfk.Rows.Count > 0)
            //    {
            //        for (int k = 0; k < dtbfk.Rows.Count; k++)
            //        {
            //            alarmInfor.Content = alarmInfor.Content + ";" + dtbfk.Rows[k]["feedback"].ToString();
            //        }
            //    }
            //}
            ////select rt.id from a_alarm_resources r,a_alarm_restype rt where r.incno='alarmid' and r.incsource=rt.id

            //inputselect = "select rt.id from a_alarm_resources r,a_alarm_restype rt where r.incno='" + alarmInfor.AlarmID + "' and r.incsource=rt.id";
            //DataSet dtsar = Pagination.GetDataList_IIPS(inputselect);
            //if (dtsar.Tables.Count > 0)
            //{
            //    DataTable dtbar = dtsar.Tables[0];

            //    if (dtbar.Rows.Count > 0)
            //    {
            //        alarmInfor.AlarmResTypeID = StringToInt(dtbar.Rows[0]["id"].ToString());
            //    }
            //}
            //InforList.Add(alarmInfor);

            //frmBatchInsert.Progress = i * 100 / totalRecord;




            //}

            //frmBatchInsert.Project = "建立处警信息索引文件进度";
            //string IndexPath = ConfigurationManager.AppSettings["AlarmIndexPath"];
            //string dicXMLFullName = ConfigurationManager.AppSettings["AlarmWordsDicConfigPath"];
            ////AddAlarmIndex(InforList, IndexPath, dicXMLFullName);

            //frmBatchInsert.Close();
            //frmBatchInsert.Project = "数据库中获取处警记录附加信息信息！";
        }


        private int StringToInt(string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return -1;
                }
                else
                {
                    if (Convert.ToInt32(input.Trim()) == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return Convert.ToInt32(input.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MenuBatchKeySearch_Click(object sender, EventArgs e)
        {

            //PaginationModel paginationModel = new PaginationModel();
            //paginationModel.TableName = " TB_FILES_INFOR t, tb_files_receive r, tb_files_infor_handle h";
            //paginationModel.GetFiledsName = " t.files_infor_type_id,t.files_infor_id,(case t.files_infor_type_id when 1 then '公安信息快报' else '其他' end) file_type, t.files_infor_title, h.handler_advice, h.director_advice, h.duty_leader_advice, h.handler_advice, (select s.send_unit_id from tb_files_send s where t.files_infor_id = s.information_id) send_unit_id,        (select z.filepath from DOC_FILE_2 z where z.upflag = 1 and  z.IIPsPath = t.files_infor_id )";
            ////paginationModel.OrderType = IIPS.Common.CommonClass.SysEnumClass.OrderBy.Desc;
            //paginationModel.OrderFiledName = "t.files_infor_id";
            //paginationModel.PageIndex = 1;
            //paginationModel.PageSize = int.MaxValue;
            //paginationModel.WhereString = "t.files_infor_submit_time < to_date('2015-12-01', 'yyyy-mm-dd') and r.receive_unit_id = 8 and t.files_infor_id = r.information_id and t.files_infor_id = h.information_id";


            if (frmBatchInsert.IsDisposed)
            {
                frmBatchInsert = new BatchInsert();
            }
            frmBatchInsert.Show();
            frmBatchInsert.Progress = 0;
            frmBatchInsert.Project = "数据库中获取信息！";
            int pageCount;
            int totalRecord;
            //original SQL(select t.files_infor_submit_time, t.files_infor_type_id, t.files_infor_id, t.files_infor_title, h.handler_advice, h.director_advice, h.duty_leader_advice, h.handle_result, (select s.send_unit_id from tb_files_send s where t.files_infor_id = s.information_id) send_unit_id, (select z.filepath from DOC_FILE_2 z where z.upflag = 1 and z.IIPsPath = t.files_infor_id) from TB_FILES_INFOR t, tb_files_receive r, tb_files_infor_handle h where t.files_infor_submit_time < to_date('2015-12-01', 'yyyy-mm-dd') and r.receive_unit_id = 8 and t.files_infor_id = r.information_id and t.files_infor_id = h.information_id)
            string input = "select t.files_infor_submit_time,t.files_infor_type_id,t.files_infor_id,t.files_infor_title,h.handler_advice, h.director_advice, h.duty_leader_advice,h.handle_result,(select s.send_unit_id from tb_files_send s where t.files_infor_id = s.information_id) send_unit_id,(select z.attachment_file_path from TB_FILES_INFOR_ATTACHMENTS z where z.information_id = t.files_infor_id and z.attachment_file_type=1) filepath from TB_FILES_INFOR t, tb_files_receive r, tb_files_infor_handle h where t.files_infor_submit_time < to_date('2015-12-01', 'yyyy-mm-dd') and r.receive_unit_id = 8 and t.files_infor_id = r.information_id and t.files_infor_id = h.information_id";
            //Pagination pn = new Pagination();
            //DataSet dt = Pagination.GetDataList_IIPS(input);
            //DataTable ds = dt.Tables[0];

            //#region 拷贝敏感词
            //if (ds.Rows.Count > 0)
            //{
            //    string filepath = "";
            //    try
            //    {
            //        frmBatchInsert.Project = "文档中获取信息进度";
            //        frmBatchInsert.Progress = 0;
            //        for (int i = 0; i < ds.Rows.Count; i++)
            //        {
            //            Model.Infors fi = new Model.Infors();
            //            //files_infor_type_id, t.files_infor_id,
            //            fi.Url = ds.Rows[i]["files_infor_type_id"].ToString() + "," + ds.Rows[i]["files_infor_id"].ToString();
            //            fi.Title = ds.Rows[i]["files_infor_title"].ToString().Replace(".doc", "");
            //            fi.DocPath = ds.Rows[i][9].ToString();
            //            log.Logger.WriteInfo(fi.Title + "," + fi.DocPath);
            //            //if (File.Exists(fi.DocPath))
            //            //{

            //            //    filepath=ds.Rows[i][9].ToString();
            //            //    int lastIndex=filepath.LastIndexOf("/");
            //            //    string ToPath = filepath.Substring(0, lastIndex);

            //            //    if (!File.Exists(@"c:\文件\" + ToPath))
            //            //    {
            //            //        Directory.CreateDirectory(@"c:\文件\" + ToPath);
            //            //    }
            //            //    File.Copy(fi.DocPath, @"c:\文件\" + ds.Rows[i][9].ToString(),true);
            //            //}
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //#endregion

            frmBatchInsert.Progress = 1;

            List<Model.Infors> dc = new List<Model.Infors>();
            //if (ds.Rows.Count > 0)
            //{
            //    try
            //    {
            //        frmBatchInsert.Project = "文档中获取信息进度";
            //        frmBatchInsert.Progress = 0;
            //        for (int i = 0; i < ds.Rows.Count; i++)
            //        {
            //            Model.Infors fi = new Model.Infors();
            //            //files_infor_type_id, t.files_infor_id,
            //            fi.Url = ds.Rows[i]["files_infor_type_id"].ToString() + "," + ds.Rows[i]["files_infor_id"].ToString();
            //            fi.Title = ds.Rows[i]["files_infor_title"].ToString().Replace(".doc", "");
            //            fi.DocPath = @"C:\文件\文件\" + ds.Rows[i][9].ToString();

            //            List<string> advise = new List<string>();
            //            string strtifpath = ds.Rows[i]["handler_advice"].ToString().Replace("\n", "").Replace("\r", "").Replace(" ", "");
            //            //advise.Add(strtifpath);
            //            strtifpath = strtifpath + " \n " + ds.Rows[i]["director_advice"].ToString().Replace("\n", "").Replace("\r", "").Replace(" ", "");
            //            //advise.Add(strtifpath);
            //            strtifpath = strtifpath + " \n " + ds.Rows[i]["duty_leader_advice"].ToString().Replace("\n", "").Replace("\r", "").Replace(" ", "");
            //            advise.Add(strtifpath);
            //            fi.Advise = advise;

            //            DateTime faxtime = Convert.ToDateTime(ds.Rows[i]["files_infor_submit_time"].ToString());
            //            //strtifpath = strtifpath.Replace(faxtime.ToString("yyyyMMdd"), "");
            //            fi.TifPath = "";
            //            fi.Time = faxtime;

            //            if (File.Exists(fi.DocPath))
            //            {
            //                string strcontent = CommonClass.GetWordBody(fi.DocPath);
            //                fi.Content = strcontent;
            //                FileInfo file = new FileInfo(fi.DocPath);
            //                fi.Length = Convert.ToInt32(file.Length);
            //                dc.Add(fi);
            //            }

            //            frmBatchInsert.Progress = i * 100 / ds.Rows.Count;


            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}

            frmBatchInsert.Progress = 1;

            //frmBatchInsert.Project = "建立全文索引文件进度";
            //string IndexPath = ConfigurationManager.AppSettings["AllWordsIndexPath"];
            //string dicXMLFullName = ConfigurationManager.AppSettings["AllWordsDicConfigPath"];
            //AddIndex(dc, IndexPath, dicXMLFullName);

            frmBatchInsert.Project = "建立敏感词索引文件进度";
            string IndexPath = ConfigurationManager.AppSettings["SensWordsIndexPath"];
            string dicXMLFullName = ConfigurationManager.AppSettings["SensWordsDicConfigPath"];
            AddSensIndex(dc, IndexPath, dicXMLFullName);

            frmBatchInsert.Close();
        }
        /// <summary>
        /// 递归目标文件夹中的所有文件和文件夹（path文件夹，fileName文件名）
        /// </summary>
        /// <param name="path"></param>
        public List<FileInfo> FindFoldersAndFiles(string path)
        {

            //遍历目标文件夹的所有文件
            foreach (string fileName in Directory.GetFiles(path))
            {
                try
                {
                    frmBatchInsert.Progress = 0;
                    FileInfo fi = new FileInfo(fileName);
                    listDoc.Add(fi);
                    frmBatchInsert.Progress = 100;
                }
                catch (Exception ex)
                {

                    //log.LogHelper.LogError(ex.Message, "SearchService.Common.CommonClass.FindFoldersAndFiles", ex);
                }

            }
            //遍历目标文件夹的所有文件夹
            foreach (string directory in Directory.GetDirectories(path))
            {
                FindFoldersAndFiles(directory);
            }
            return listDoc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuDictManage_Click(object sender, EventArgs e)
        {
            //Dic.DicManage dicm = new Dic.DicManage();
            //dicm.ShowDialog();
        }

        private void AddIndex(List<FileInfo> dc, string IndexPath, string dicPath)
        {
            int i = 0;
            frmBatchInsert.Progress = i * 100 / dc.Count;
            //Index.Write write = new Index.Write(IndexPath, dicPath);
            // write.INDEX_DIR = IndexPath;// @"Index\AllWord";
            //write.Start();

            foreach (FileInfo obj in dc)
            {
                Model.Infors infor = new Model.Infors();
                //infor.Url = "";
                //infor.DocPath = obj.FullName;
                //infor.TifPath=
                infor.Title = obj.Name;
                infor.Time = obj.LastWriteTime;
                //infor.Length = Convert.ToInt32(obj.Length);

                //    string strcontent = CommonClass.GetWordBody(infor.DocPath);

                //    if (!string.IsNullOrWhiteSpace(strcontent))
                //    {
                //        infor.Content = strcontent;
                //        write.IndexString(infor);
                //    }
                //    i++;
                //    frmBatchInsert.Progress = i * 100 / dc.Count;

            }
            //write.Close();
        }
        private void AddAllIndex(List<FileInfo> dc, string IndexPath, string dicPath)
        {
            int i = 0;
            if (dc.Count != 0)
            {
                frmBatchInsert.Progress = i * 100 / dc.Count;
                //Index.Write write = new Index.Write(IndexPath, dicPath);
                //// write.INDEX_DIR = IndexPath;// @"Index\AllWord";
                //write.Start();

                foreach (FileInfo obj in dc)
                {
                    Model.Infors infor = new Model.Infors();
                    //infor.Url = "";
                    //infor.DocPath = obj.FullName;
                    //infor.TifPath=
                    infor.Title = obj.Name;
                    infor.Time = obj.LastWriteTime;
                    //infor.Length = Convert.ToInt32(obj.Length);
                    //这四个参数要怎么加值啊？
                    infor.UserID = "2595";//文件的创建者id
                                          //infor.UnitID = "19";//文件创建者单位id
                    infor.InformationID = "";//文件在数据库中的id
                    infor.TypeID = "";//文件在数据库中的种类id
                                      //string strcontent = CommonClass.GetWordBody(infor.DocPath);

                    //if (!string.IsNullOrWhiteSpace(strcontent))
                    //{
                    //    infor.Content = strcontent;
                    //    write.IndexAllString(infor);
                    //}
                    i++;
                    frmBatchInsert.Progress = i * 100 / dc.Count;

                }
                //write.Close();
            }
        }
        ///
        private void AddNewAllIndex(List<FileInfo> dc, string IndexPath, string dicPath)
        {
            List<FileInfo> dc1 = new List<FileInfo>();
            List<FileInfo> dc2 = new List<FileInfo>();
            List<FileInfo> dc3 = new List<FileInfo>();
            string sPath = IndexPath + @"\" + System.DateTime.Now.Year.ToString();//新的一个文件夹
            string mPath = IndexPath + @"\" + System.DateTime.Now.AddYears(-1).Year.ToString();
            string ePath = IndexPath + @"\OldYear";
            if (!Directory.Exists(ePath))
            {
                Directory.CreateDirectory(ePath);
            }
            if (!Directory.Exists(mPath))
            {
                Directory.CreateDirectory(mPath);
            }
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }
            foreach (FileInfo obj in dc)
            {
                Model.Infors infors = new Model.Infors();
                if (obj.LastWriteTime.Year.ToString() == System.DateTime.Now.Year.ToString())
                {
                    dc1.Add(obj);
                }
                else if (obj.LastWriteTime.Year.ToString() == System.DateTime.Now.AddYears(-1).ToString())
                {
                    dc2.Add(obj);
                }
                else
                {
                    dc3.Add(obj);
                }
            }
            AddAllIndex(dc1, sPath, dicPath);
            AddAllIndex(dc2, mPath, dicPath);
            AddAllIndex(dc3, ePath, dicPath);
        }
        //向已有的索引文件中增加新的索引
        private void AddNewIndex(List<FileInfo> dc, string IndexPath, string dicPath)
        {
            int i = 0;
            frmBatchInsert.Progress = i * 100 / dc.Count;
            //Index.Write write = new Index.Write(IndexPath, dicPath);
            // write.INDEX_DIR = IndexPath;// @"Index\AllWord";
            //write.Add();

            foreach (FileInfo obj in dc)
            {
                Model.Infors infor = new Model.Infors();
                //infor.Url = "";
                //infor.DocPath = obj.FullName;
                //infor.TifPath=
                infor.Title = obj.Name;
                infor.Time = obj.LastWriteTime;
                //infor.Length = Convert.ToInt32(obj.Length);

                //string strcontent = CommonClass.GetWordBody(infor.DocPath);

                //if (!string.IsNullOrWhiteSpace(strcontent))
                //{
                //    infor.Content = strcontent;
                //    write.IndexString(infor);
                //}
                i++;
                frmBatchInsert.Progress = i * 100 / dc.Count;

            }
            //write.Close();
        }

        private void AddSensIndex(List<Model.Infors> dc, string IndexPath, string dicPath)
        {
            int i = 0;
            frmBatchInsert.Progress = i * 100 / dc.Count;
            //Index.Write write = new Index.Write(IndexPath, dicPath);
            // write.INDEX_DIR = IndexPath;// @"Index\AllWord";
            //write.Start();

            foreach (Model.Infors obj in dc)
            {
                if (!string.IsNullOrWhiteSpace(obj.Content))
                {

                    //write.SensIndexString(obj);
                }
                i++;
                frmBatchInsert.Progress = i * 100 / dc.Count;

            }
            //write.Close();
        }

        //private void AddAlarmIndex(Dictionary<string, Model.AlarmInfor> allInfor, string IndexPath, string dicPath)
        //{
        //    int i = 0;
        //    frmBatchInsert.Progress = i * 100 / allInfor.Count;
        //    Index.Write write = new Index.Write(IndexPath, dicPath);
        //    // write.INDEX_DIR = IndexPath;// @"Index\AllWord";
        //    write.Start();

        //    foreach (KeyValuePair<string, Model.AlarmInfor> kv in allInfor)
        //    {
        //        //Model.AlarmInfor alarmInfor = kv.Value;
        //        //kv.Value.ID = i;
        //        write.AlarmIndexString(kv.Value);
        //        i++;
        //        frmBatchInsert.Progress = i * 100 / allInfor.Count;
        //    }

        //    write.Close();
        //}
        private void AddIndex(List<Model.Infors> dc, string IndexPath, string dicPath)
        {
            int i = 0;
            frmBatchInsert.Progress = 0;
            frmBatchInsert.Project = "建立索引文件进度";
            //Index.Write write = new Index.Write(IndexPath, dicPath);
            ////write.INDEX_DIR = IndexPath;// @"Index\AllWord";
            //write.Start();

            foreach (Model.Infors obj in dc)
            {

                if (!string.IsNullOrWhiteSpace(obj.Content))
                {

                    //write.IndexString(obj);
                }
                i++;
                frmBatchInsert.Progress = i * 100 / dc.Count;

            }
            //write.Close();
        }

        private void MenuBatchSens_Click(object sender, EventArgs e)
        {
            //SegementThreadRun();
            //SegementManage.Start();
            if (frmBatchInsert.IsDisposed)
            {
                frmBatchInsert = new BatchInsert();
            }
            frmBatchInsert.Show();
            frmBatchInsert.Progress = 0;
            frmBatchInsert.Project = "数据库中获取信息";
            //int pageCount;
            //int totalRecord;
            string input = "";
            //string input = "select t.files_infor_type_id,t.files_infor_id,t.files_infor_title,z.filepath from TB_FILES_INFOR t, tb_files_receive r, DOC_FILE_2 z where t.files_infor_submit_time < to_date('2015-12-01', 'yyyy-mm-dd')   and r.receive_unit_id = 8 and t.files_infor_id = r.information_id  and z.upflag = 1   and  z.IIPsPath = t.files_infor_id ";
            ////Pagination pn = new Pagination();
            //DataSet dt = Pagination.GetDataList_IIPS(input);
            DataTable ds = tt;
            frmBatchInsert.Progress = 1;
            List<string> SensList = new List<string>();
            List<string[]> NoSensList = new List<string[]>();
            List<Model.Infors> dc = new List<Model.Infors>();
            if (ds.Rows.Count > 0)
            {
                try
                {
                    frmBatchInsert.Project = "文档中获取信息进度";
                    frmBatchInsert.Progress = 0;
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        Model.Infors fi = new Model.Infors();
                        //files_infor_type_id, t.files_infor_id,
                        //fi.Url = ds.Rows[i]["files_infor_id"].ToString();
                        //if (fi.Url == "1025")
                        //{

                        //}

                        //fi.Title = ds.Rows[i]["files_infor_title"].ToString().Replace(".doc", "");
                        //fi.DocPath = @"D:\文件\文件\" + ds.Rows[i][3].ToString();

                        //List<string> advise = new List<string>();
                        //string strtifpath = ds.Rows[i]["handler_advice"].ToString().Replace("\n", "").Replace("\r", "").Replace(" ", "");
                        ////advise.Add(strtifpath);
                        //strtifpath = strtifpath + " | " + ds.Rows[i]["director_advice"].ToString().Replace("\n", "").Replace("\r", "").Replace(" ", "");
                        ////advise.Add(strtifpath);
                        //strtifpath = strtifpath + " | " + ds.Rows[i]["duty_leader_advice"].ToString().Replace("\n", "").Replace("\r", "").Replace(" ", "");
                        //advise.Add(strtifpath);
                        //fi.Advise = advise;

                        //DateTime faxtime = Convert.ToDateTime(ds.Rows[i]["files_infor_submit_time"].ToString());
                        //strtifpath = strtifpath.Replace(faxtime.ToString("yyyyMMdd"), "");
                        //fi.TifPath = "";
                        //fi.Time = faxtime;

                        //if (File.Exists(fi.DocPath))
                        //{
                        //string strcontent = CommonClass.GetWordBody(fi.DocPath);
                        //fi.Content = strcontent;
                        //Index.SensWrite.
                        //List<string> words = SegementManage.GetSensSegementByInput(strcontent);
                        //string sens = SegementManage.GetSensSegementByInput(words);
                        input = "insert into TB_FILES_INFOR_KEYWORD(INFORMATION_ID,Infor_Keywords)values({0},'{1}')";
                        //if (!string.IsNullOrWhiteSpace(sens))
                        //{
                        //    string temp = string.Format(input, fi.Url, sens);
                        //    //log.Logger.WriteInfo(temp);
                        //}
                        //else
                        //{
                        //    string[] array = new string[2];
                        //    array[0] = fi.Url;
                        //    array[1] = fi.DocPath;
                        //    //log.Logger.WriteInfo(fi.Url);
                        //}
                        //input = string.Format(input, Convert.ToInt32( fi.Url), sens);
                        ////Pagination pn = new Pagination();
                        //DataSet dst = Pagination.GetDataList_IIPS(input);
                        //FileInfo file = new FileInfo(fi.DocPath);
                        //fi.Length = Convert.ToInt32(file.Length);
                        //dc.Add(fi);
                        //}

                        frmBatchInsert.Progress = i * 100 / ds.Rows.Count;


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            frmBatchInsert.Progress = 1;

            //frmBatchInsert.Project = "建立全文索引文件进度";
            //string IndexPath = ConfigurationManager.AppSettings["AllWordsIndexPath"];
            //string dicXMLFullName = ConfigurationManager.AppSettings["AllWordsDicConfigPath"];
            //AddIndex(dc, IndexPath, dicXMLFullName);

            //frmBatchInsert.Project = "建立敏感词索引文件进度";
            //string IndexPath = ConfigurationManager.AppSettings["SensWordsIndexPath"];
            //string dicXMLFullName = ConfigurationManager.AppSettings["SensWordsDicConfigPath"];
            //AddSensIndex(dc, IndexPath, dicXMLFullName);

            frmBatchInsert.Close();
        }
        //新增全文索引
        private void MenuAddNewSearch_Click(object sender, EventArgs e)
        {
            fbdFolderBrowser.ShowDialog();
            string path = fbdFolderBrowser.SelectedPath;
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (frmBatchInsert.IsDisposed)
                {
                    frmBatchInsert = new BatchInsert();
                }
                frmBatchInsert.Show();
                frmBatchInsert.Progress = 0;
                frmBatchInsert.Project = "遍历文件夹进度";
                List<FileInfo> dc = FindFoldersAndFiles(path);
                frmBatchInsert.Progress = 1;

                frmBatchInsert.Project = "添加全文索引文件进度";
                string IndexPath = ConfigurationManager.AppSettings["AllWordsIndexPath"];
                string dicXMLFullName = ConfigurationManager.AppSettings["AllWordsDicConfigPath"];
                AddNewIndex(dc, IndexPath, dicXMLFullName);

                //frmBatchInsert.Project = "建立敏感词索引文件进度";
                //IndexPath = ConfigurationManager.AppSettings["SensWordsIndexPath"];
                //dicXMLFullName = ConfigurationManager.AppSettings["SensWordsDicConfigPath"];
                //AddIndex(dc, IndexPath, dicXMLFullName);

                frmBatchInsert.Close();
            }
            else
            {
                MessageBox.Show("所选文件夹为空！");
            }
        }
    }

}
