using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

using WebSocket4Net;
using WebSocket.ClientEngine;
using SearchService.Model;
using SearchService.Common.Log;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace SearchService.Client
{
    public partial class CMain : Form
    {

        List<FileInfo> listDoc = new List<FileInfo>();
        private WebSocketVersion m_Version = WebSocketVersion.DraftHybi00;
        private WebSocket4Net.WebSocket webSocketClient;
        private JsonWebSocket webSocket;

        private int ServerMaxReceiveByteCount = int.Parse(ConfigurationManager.AppSettings["ServerMaxReceiveByteCount"]);
        public CMain()
        {
            InitializeComponent();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            //if (cbbIP.SelectedItem == null || cbbPort.SelectedItem == null)
            //{
            //    MessageBox.Show("IP或Port不能为空！");
            //    cbbIP.Focus();
            //    return;
            //}
            //string IP = cbbIP.SelectedItem.ToString();
            //string Port = cbbPort.SelectedItem.ToString();

            //string path = string.Format("ws://{0}:{1}/", IP, Port);
            string path = "wss://ws.blockchain.info/inv";

            webSocketClient = new WebSocket4Net.WebSocket(path, "basic", m_Version);
            webSocketClient.AllowUnstrustedCertificate = true;
            webSocketClient.Opened += new EventHandler(webSocketClient_Opened);
            webSocketClient.Closed += new EventHandler(webSocketClient_Closed);
            webSocketClient.DataReceived += new EventHandler<DataReceivedEventArgs>(webSocketClient_DataReceived);
            webSocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(webSocketClient_MessageReceived);
            webSocketClient.Error += new EventHandler<WebSocket.ClientEngine.ErrorEventArgs>(webSocketClient_Error);


            webSocketClient.Open();


            //EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(IP), Port);

            //socket = CreateClientSocket();
            //socket.Connect(serverAddress);
            //socketStream = GetSocketStream(socket);
            //reader = new StreamReader(socketStream, m_Encoding, true);
            //writer = new ConsoleWriter(socketStream, m_Encoding, 1024 * 8);
        }

        private void webSocketClient_Error(object sender, WebSocket.ClientEngine.ErrorEventArgs e)
        {
            beginInvokPrint("ER:" + e.Exception.Message, DateTime.Now);
        }

        List<string> ResultList = new List<string>();
        private void webSocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //ResultList.Add(e.Message);
            beginInvokPrint("MR:" + e.Message, DateTime.Now);
        }

        private void webSocketClient_DataReceived(object sender, DataReceivedEventArgs e)
        {
            beginInvokPrint("DR:" + e.Data, DateTime.Now);
        }

        private void webSocketClient_Closed(object sender, EventArgs e)
        {
            beginInvokPrint("CR:断开连接！", DateTime.Now);
        }

        private void webSocketClient_Opened(object sender, EventArgs e)
        {


            beginInvokPrint("OR:" + ((WebSocket4Net.WebSocket)sender).LocalEndPoint.ToString(), DateTime.Now);

            webSocketClient.Send("{'op':'unconfirmed_sub'}");
        }


        private void btnECHO_Click(object sender, EventArgs e)
        {
            #region ECHO

            string[] lines = new string[100];

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = Guid.NewGuid().ToString();
                webSocketClient.Send("ECHOX " + lines[i]);
                beginInvokPrint("ECHOX " + lines[i], DateTime.Now);
            }
            #endregion
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            #region ADD MULT
            Random rd = new Random();

            for (int i = 0; i < 5; i++)
            {
                int x = rd.Next(1, 1000), y = rd.Next(1, 1000);

                StringBuilder command = new StringBuilder();
                command.Append("ADD ");
                command.Append(x);
                command.Append(" ");
                command.Append(y);

                webSocketClient.Send(command.ToString());
                beginInvokPrint(command.ToString(), DateTime.Now);

            }
            #endregion
        }
        private void btnADDX_Click(object sender, EventArgs e)
        {
            #region ADDX
            Random rd = new Random();

            for (int i = 0; i < 5; i++)
            {
                int x = rd.Next(1, 1000), y = rd.Next(1, 1000);

                StringBuilder command = new StringBuilder();
                command.Append("ADDX {\"A\":\"");
                command.Append(x + "\",\"B\":\"");
                command.Append(y + "\"}");

                webSocketClient.Send(command.ToString());
                beginInvokPrint(command.ToString(), DateTime.Now);

            }
            #endregion
        }
        private void btnMULT_Click(object sender, EventArgs e)
        {
            #region MULT
            Random rd = new Random();
            for (int i = 0; i < 5; i++)
            {
                int x = rd.Next(1, 1000), y = rd.Next(1, 1000);

                StringBuilder command = new StringBuilder();
                command.Append("MULT ");
                command.Append(x);
                command.Append(" ");
                command.Append(y);
                beginInvokPrint(command.ToString(), DateTime.Now);
                webSocketClient.Send(command.ToString());
            }
            #endregion
        }

        private void btnMULTX_Click(object sender, EventArgs e)
        {
            #region MULTX
            Random rd = new Random();
            for (int i = 0; i < 5; i++)
            {
                int x = rd.Next(1, 1000), y = rd.Next(1, 1000);

                StringBuilder command = new StringBuilder();
                command.Append("MULTX {\"A\":\"");
                command.Append(x + "\",\"B\":\"");
                command.Append(y + "\"}");
                beginInvokPrint(command.ToString(), DateTime.Now);
                webSocketClient.Send(command.ToString());
            }
            #endregion
        }
        private void btnMES_Click(object sender, EventArgs e)
        {
            string command = "ECHOJSON " + "{\"A\":\"100\",\"B\":\"200\"}";
            beginInvokPrint(command, DateTime.Now);
            webSocketClient.Send(command);
        }
        private void btnPING_Click(object sender, EventArgs e)
        {
            string command = "PING";
            beginInvokPrint(command, DateTime.Now);
            webSocketClient.Send(command);
        }

        private void btnQUIT_Click(object sender, EventArgs e)
        {
            webSocketClient.Send("WIO Commit");

            //string command = "QUIT";
            //beginInvokPrint(command, DateTime.Now);
            //webSocketClient.Send(command+" bye");
        }
        private void btnLOGIN_Click(object sender, EventArgs e)
        {
            //var data = Encoding.UTF8.GetBytes("ECHO " + message);

            //webSocketClient.Send(data, 0, data.Length);

            string command = "LOGIN 12345 12345";
            byte[] Bytes = Encoding.UTF8.GetBytes(command);
            beginInvokPrint(command, DateTime.Now);
            webSocketClient.Send("LOGIN");
        }

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
            try
            {
                if (_dt != null)
                {
                    rtbMessage.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "  " + _str + "\r\n");
                }
                else
                {
                    rtbMessage.AppendText(_dt.ToString("yyyy/MM/dd HH:mm:ss.fff") + "  " + _str + "\r\n");

                }
                rtbMessage.ScrollToCaret();
            }
            catch (Exception e)
            {

            }
        }


        #endregion






        static Socket CreateClientSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        static Stream GetSocketStream(Socket socket)
        {
            return new NetworkStream(socket);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webSocket = new JsonWebSocket("ws://127.0.0.1:2012/");

            //webSocket.On<string>("ECHO", HandleEchoResponse);
            webSocket.Closed += new EventHandler(websocket_Closed);
            //webSocket.
            //websocket.Error +=new EventHandler<WebSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
            webSocket.Opened += new EventHandler(websocket_Opened);
            webSocket.Open();

        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            beginInvokPrint("OR:", DateTime.Now);
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            beginInvokPrint("CR:断开连接！", DateTime.Now);
        }

        private void HandleEchoResponse(string obj)
        {
            webSocket.Send("ECHO", Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            webSocket.Send("ECHO", Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Random m_Random = new Random();
            webSocket.Send("ECHOX", new ClientInfo
            {
                ID = m_Random.Next(1, 1000),
                Height = m_Random.Next(1, 1000),
                LocationX = m_Random.Next(1, 1000),
                LocationY = m_Random.Next(1, 1000)
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webSocket.Send("MULTX", new AddIn
            {
                A = 160,
                B = 170
            });
        }

        private void btnSendDoc_Click(object sender, EventArgs e)
        {
            if (fbdFolder.ShowDialog() == DialogResult.OK)
            {

                FindFoldersAndFiles(fbdFolder.SelectedPath);
                int i = 0;
                foreach (FileInfo node in listDoc)
                {
                    i++;
                    Infors infor = new Infors();
                    infor.InformationID = i.ToString();
                    infor.Title = node.Name;
                    infor.TypeID = "0";
                    infor.Time = node.CreationTime;

                    infor.UserID = "2001";
                    //infor.SUnitID = new List<string>() { "70" };
                    infor.DocNum = "SW" + i.ToString();

                    //infor.RUnitID = new List<string>() { (75 + i).ToString(), (175 + i).ToString(), (275 + i).ToString(), (186 + i).ToString(), (85 + i).ToString() };
                    //infor.Advise = new List<string>() { "呈", "牵头", "协助" };

                    if (File.Exists(node.FullName))
                    {
                        if (node.FullName.ToLower().EndsWith(".doc") || node.FullName.ToLower().EndsWith(".docx"))
                        {
                            infor.Content = pdf2itxt(node.FullName);
                            infor.PageNo = 1;
                            infor.PageCount = 1;

                            SplitContextSend(infor, "WI", node.FullName);
                        }
                    }

                }

                // webSocketClient.Send("WIO Commit");
            }
        }

        private void SplitContextSend(Infors sendContext, string command, string fullName)
        {
            string context = sendContext.Content;
            string splitFlag = " ";
            sendContext.Content = "";

            //除context外的壳的长度
            string JsonShell = SearchService.Common.JsonHelper.SerializeObject(sendContext);
            string strShell = command + splitFlag + JsonShell;
            double intShell = Encoding.UTF8.GetMaxByteCount(strShell.Length);

            //int contextLength = Encoding.UTF8.GetMaxByteCount(context.Length);
            //可发送的最大字符数 （x）*3
            double leftSpace = ServerMaxReceiveByteCount - intShell;//3072接受的最大byte值
            double temp = leftSpace / 3;
            int sendMaxLength = System.Convert.ToInt32(Math.Floor(temp));

            //判断内容长度是否大于可发送的最大字符数
            if (context.Length > sendMaxLength)
            {
                //计算发送次数和最后一次发送长度
                int starIndex = 0;
                int LastLength = context.Length % sendMaxLength;
                int SendCount = 0;
                if (LastLength > 0)
                {
                    SendCount = (context.Length / sendMaxLength) + 1;
                }
                else
                {
                    SendCount = (context.Length / sendMaxLength);
                }
                //double tempD =(double) context.Length / (double)strLen;
                //int PageCount = Convert.ToInt32(Math.Ceiling(tempD));
                sendContext.PageCount = SendCount;
                for (int j = 1; j <= SendCount; j++)
                {
                    string sendString = "";
                    if (j == SendCount)
                    {
                        sendString = context.Substring(starIndex, LastLength);
                    }
                    else
                    {
                        sendString = context.Substring(starIndex, sendMaxLength);
                    }

                    sendContext.Content = sendString;

                    starIndex = starIndex + sendMaxLength;

                    int contextlength = Encoding.UTF8.GetMaxByteCount(sendString.Length);
                    sendContext.PageNo = j;
                    string jsonSend = SearchService.Common.JsonHelper.SerializeObject(sendContext);
                    string sendStr = command + splitFlag + jsonSend;
                    int length = Encoding.UTF8.GetMaxByteCount(sendStr.Length);
                    webSocketClient.Send(sendStr);
                    //webSocketClient.Send(command,)
                    Logger.WriteInfo(fullName + ":" + sendContext.InformationID);
                }
            }
            else
            {
                sendContext.Content = context;
                //int contextlength = Encoding.UTF8.GetMaxByteCount(context.Length);
                string jsonSend = SearchService.Common.JsonHelper.SerializeObject(sendContext);
                string sendStr = command + splitFlag + jsonSend;
                int length = Encoding.UTF8.GetMaxByteCount(sendStr.Length);

                webSocketClient.Send(sendStr);
                Logger.WriteInfo(fullName + ":" + sendContext.InformationID);
            }

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

                    FileInfo fi = new FileInfo(fileName);
                    listDoc.Add(fi);

                }
                catch (Exception ex)
                {
                    string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                    Logger.WriteError(funName, ex);
                }

            }
            //遍历目标文件夹的所有文件夹
            foreach (string directory in Directory.GetDirectories(path))
            {
                FindFoldersAndFiles(directory);
            }
            return listDoc;
        }

        public string pdf2itxt(string SPath)
        {

            StringBuilder text = new StringBuilder();
            if (File.Exists(SPath))
            {
                PdfReader pdfReader = new PdfReader(SPath);
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(SPath));

                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);
                    //currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(
                    //    Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);


                }
                pdfDoc.Close();
                pdfReader.Close();
            }
            //string str = text.ToString();
            //if (!string.IsNullOrEmpty(str))
            //{
            //    char[] carr = str.ToCharArray();
            //    for (int i = 0; i < carr.Length; i++)
            //    {
            //        if ((int)carr[i] < 32 && (int)carr[i] >= 0)
            //        {
            //            carr[i] = (char)32;
            //        }
            //    }
            //    str = new string(carr);
            //    str = str.Replace("\'", " ").Replace("\"", " ").Replace("\\", " ").Replace("FORMTEXT", "").Replace("formtext", "");
            //}
            //StreamWriter swPdfChange = new StreamWriter(@"D:\WorkBackUp\搜索引擎\doc\201949-1.txt", false, Encoding.GetEncoding("gb2312"));
            //swPdfChange.Write(text.ToString());
            //swPdfChange.Close();
            return text.ToString();

        }

        public static string GetWordBody(string SPath)
        {
            string str = "";
            try
            {
                //Aspose.Words.Document doc = new Aspose.Words.Document(SPath);
                //NodeCollection nc = doc.GetChildNodes(NodeType.Body, true);
                //foreach (Node node in nc)
                //{
                //    str = str + node.GetText();
                //}
                //if (!string.IsNullOrEmpty(str))
                //{
                //    char[] carr = str.ToCharArray();
                //    for (int i = 0; i < carr.Length; i++)
                //    {
                //        if ((int)carr[i] < 32 && (int)carr[i] >= 0)
                //        {
                //            carr[i] = (char)32;
                //        }
                //    }
                //    str = new string(carr);
                //    str = str.Replace("\'", " ").Replace("\"", " ").Replace("\\", " ").Replace("FORMTEXT", "");
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteDebug(SPath);
                //Log.LogHelper.LogError(ex.Message, "SearchService.Common.CommonClass.GetWord  文件路径:" + SPath, ex);
            }
            return str;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (fbdFolder.ShowDialog() == DialogResult.OK)
            {

                FindFoldersAndFiles(fbdFolder.SelectedPath);
                int i = 0;
                foreach (FileInfo node in listDoc)
                {
                    i++;
                    Infors infor = new Infors();
                    infor.InformationID = i.ToString();
                    //infor.DocPath = node.FullName;
                    //infor.Url = node.FullName;
                    infor.Title = node.Name;
                    infor.UserID = "1";
                    //infor.UnitID = "1";
                    infor.TypeID = "0";
                    infor.Time = node.CreationTime;
                    //infor.Length = node.Length;
                    //infor.TifPath = "";
                    //infor.Advise = new List<string>() { "" };
                    infor.Participle = "";

                    if (File.Exists(node.FullName))
                    {
                        if (node.FullName.ToLower().EndsWith(".doc") || node.FullName.ToLower().EndsWith(".docx"))
                        {
                            infor.Content = pdf2itxt(infor.InformationID);
                            infor.PageNo = 1;
                            infor.PageCount = 1;

                            SplitContextJsonSend(infor, "WI");
                        }
                    }



                }
                webSocket.Send("WIO", "Commit");
                //webSocketClient.Send("WIO Commit");
            }
        }


        private void SplitContextJsonSend(Infors sendContext, string command)
        {
            string context = sendContext.Content;
            sendContext.Content = "";

            //除context外的壳的长度
            string JsonShell = SearchService.Common.JsonHelper.SerializeObject(sendContext);
            string strShell = command + JsonShell;
            double intShell = Encoding.UTF8.GetMaxByteCount(strShell.Length);

            //int contextLength = Encoding.UTF8.GetMaxByteCount(context.Length);
            //可发送的最大字符数 （x）*3
            double leftSpace = ServerMaxReceiveByteCount - intShell;//3072接受的最大byte值
            double temp = leftSpace / 3;
            int sendMaxLength = System.Convert.ToInt32(Math.Floor(temp));

            //判断内容长度是否大于可发送的最大字符数
            if (context.Length > sendMaxLength)
            {
                //计算发送次数和最后一次发送长度
                int starIndex = 0;
                int LastLength = context.Length % sendMaxLength;
                int SendCount = 0;
                if (LastLength > 0)
                {
                    SendCount = (context.Length / sendMaxLength) + 1;
                }
                else
                {
                    SendCount = (context.Length / sendMaxLength);
                }
                //double tempD =(double) context.Length / (double)strLen;
                //int PageCount = Convert.ToInt32(Math.Ceiling(tempD));
                sendContext.PageCount = SendCount;
                for (int j = 1; j <= SendCount; j++)
                {
                    string sendString = "";
                    if (j == SendCount)
                    {
                        sendString = context.Substring(starIndex, LastLength);
                    }
                    else
                    {
                        sendString = context.Substring(starIndex, sendMaxLength);
                    }

                    sendContext.Content = sendString;

                    starIndex = starIndex + sendMaxLength;

                    int contextlength = Encoding.UTF8.GetMaxByteCount(sendString.Length);
                    sendContext.PageNo = j;
                    string jsonSend = SearchService.Common.JsonHelper.SerializeObject(sendContext);
                    string sendStr = command + jsonSend;
                    int length = Encoding.UTF8.GetMaxByteCount(sendStr.Length);
                    webSocketClient.Send(sendStr);
                    //webSocket.Send(command, sendContext);
                    Logger.WriteInfo(sendContext.InformationID);
                }
            }
            else
            {
                sendContext.Content = context;
                //int contextlength = Encoding.UTF8.GetMaxByteCount(context.Length);
                string jsonSend = SearchService.Common.JsonHelper.SerializeObject(sendContext);
                string sendStr = command + jsonSend;
                int length = Encoding.UTF8.GetMaxByteCount(sendStr.Length);

                webSocket.Send(command, sendContext);
                Logger.WriteInfo(sendContext.InformationID);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string[] al = new string[] { "武汉", "福田", "龙岗", "龙城", "深圳", "医院", "武警", "部队", "东门", "都市", "长安", "成都", "北京", "上海", "广州", "大桥", };
            for (int i = 0; i < 100; i++)
            {
                foreach (string stem in al)
                {
                    AssParam asp = new AssParam();
                    asp.Input = stem;
                    asp.Count = 25;
                    string sendStr = SearchService.Common.JsonHelper.SerializeObject(asp);
                    webSocketClient.Send("SA " + sendStr);
                }
            }
            //AssParam asp = new AssParam();
            //asp.Input = "武汉";
            //asp.Count = 25;
            //string sendStr = SearchService.Common.JsonHelper.SerializeObject(asp);
            //webSocketClient.Send("SA "+sendStr);
        }

        private void CMainUI_Load(object sender, EventArgs e)
        {
            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            //object paramMissing = Type.Missing;
            //string docPath = @"C:\Users\Jacky\Desktop\doc2pdf\1178\117801.doc";
            //string savePath = @"C:\Users\Jacky\Desktop\test\m1y.pdf";
            //object docPathObject = docPath;

            //Convert(docPath, savePath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            //Aspose.Words.Document word = new Document(@"C:\Users\Jacky\Desktop\doc2pdf\1177\117701.doc");
            //word.Save(@"C:\Users\Jacky\Desktop\test\m1y.png", Aspose.Words.SaveFormat.Png);
            //word.Save(@"C:\Users\Jacky\Desktop\test\m1y.pdf", Aspose.Words.SaveFormat.Pdf);
            //watch.Stop();
            //string time = watch.ToString();
            //document.ExportAsFixedFormat(savePath,Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);



            //Infors infor = new Infors();
            //infor.InPutWord = textBox1.Text;
            //infor.InformationID = "1002";
            //infor.Content = "231123123123123";
            //infor.Time = DateTime.Now;
            //string str = SearchService.Common.JsonHelper.SerializeObject(infor);                

            //Infors IS = SearchService.Common.JsonHelper.DeserializeJsonToObject<Infors>(str);

        }


        //private bool Convert(string sourcePath, string targetPath, Microsoft.Office.Interop.Word.WdExportFormat exportFormat)
        //{
        //    bool result;
        //    object paramMissing = Type.Missing;
        //    //Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
        //    //Microsoft.Office.Interop.Word.Document wordDocument = null;
        //    //try
        //    //{
        //    //    object paramSourceDocPath = sourcePath;
        //    //    string paramExportFilePath = targetPath;

        //    //    Microsoft.Office.Interop.Word.WdExportFormat paramExportFormat = exportFormat;
        //    //    bool paramOpenAfterExport = false;
        //    //    Microsoft.Office.Interop.Word.WdExportOptimizeFor paramExportOptimizeFor =
        //    //            Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
        //    //    Microsoft.Office.Interop.Word.WdExportRange paramExportRange = Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument;
        //    //    int paramStartPage = 0;
        //    //    int paramEndPage = 0;
        //    //    Microsoft.Office.Interop.Word.WdExportItem paramExportItem = Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent;
        //    //    bool paramIncludeDocProps = true;
        //    //    bool paramKeepIRM = true;
        //    //    Microsoft.Office.Interop.Word.WdExportCreateBookmarks paramCreateBookmarks =
        //    //            Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
        //    //    bool paramDocStructureTags = true;
        //    //    bool paramBitmapMissingFonts = true;
        //    //    bool paramUseISO19005_1 = false;

        //    //    wordDocument = wordApplication.Documents.Open(
        //    //            ref paramSourceDocPath, ref paramMissing, ref paramMissing,
        //    //            ref paramMissing, ref paramMissing, ref paramMissing,
        //    //            ref paramMissing, ref paramMissing, ref paramMissing,
        //    //            ref paramMissing, ref paramMissing, ref paramMissing,
        //    //            ref paramMissing, ref paramMissing, ref paramMissing,
        //    //            ref paramMissing);

        //    //    if (wordDocument != null)
        //    //        wordDocument.ExportAsFixedFormat(paramExportFilePath,
        //    //                paramExportFormat, paramOpenAfterExport,
        //    //                paramExportOptimizeFor, paramExportRange, paramStartPage,
        //    //                paramEndPage, paramExportItem, paramIncludeDocProps,
        //    //                paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
        //    //                paramBitmapMissingFonts, paramUseISO19005_1,
        //    //                ref paramMissing);
        //    //    result = true;
        //    //}
        //    //finally
        //    //{
        //    //    if (wordDocument != null)
        //    //    {
        //    //        wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
        //    //        wordDocument = null;
        //    //    }
        //    //    if (wordApplication != null)
        //    //    {
        //    //        wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
        //    //        wordApplication = null;
        //    //    }
        //    //    GC.Collect();
        //    //    GC.WaitForPendingFinalizers();
        //    //    GC.Collect();
        //    //    GC.WaitForPendingFinalizers();
        //    //}
        //    return result;
        //}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            AllSearchParam SP = new AllSearchParam();
            SP.InPutWord = textBox1.Text;
            SP.StartTime = DateTime.Now.AddYears(-1);
            SP.EndTime = DateTime.Now;
            SP.Range = 0;
            //SP.RUnitID = new List<string>() { "76", "88" };
            //SP.DocNum = "A12313";
            SP.SUnitID = new List<string>() { "8", "45" };
            SP.NoneWord = "抢劫";

            //SP.InPutWord = textBox1.Text;
            //SP.Range = SearchRange.TitleAndContext;
            //SP.RUnitID = new List<string>() { "76", "88" };
            SP.PageNo = 1;
            SP.PageSize = 8;

            string Json = SearchService.Common.JsonHelper.SerializeObject(SP);
            //{"DocNum":"","InPutWord":"福","Range":0,"NoneWord":"","StartTime":"","EndTime":"","PageNo":1,"PageSize":8,"SUnitID":["8"]}
            webSocketClient.Send("RI " + Json);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AllSearchParam SP = new AllSearchParam();
            //SP.InPutWord = textBox1.Text;
            SP.StartTime = DateTime.Now.AddYears(-2);
            SP.EndTime = DateTime.Now;
            //SP.Range = SearchRange.TitleAndContext;
            //SP.RUnitID = new List<string>() { "76", "88" };
            SP.PageNo = 1;
            SP.PageSize = 2;

            string Json = SearchService.Common.JsonHelper.SerializeObject(SP);

            webSocketClient.Send("RI " + Json);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AllSearchParam SP = new AllSearchParam();

            SP.Range = 0;
            SP.InPutWord = "举报,维权,涉毒,盗窃";

            //SP.RUnitID = new List<string>() { "76", "88" };
            SP.RUnitID = new List<string>() { "8", "2266", "26552", "26523" };
            SP.PageNo = 1;
            SP.PageSize = 3;

            //{"InPutWord":"举报,维权,涉毒,盗窃","Range":0,"NoneWord":"","StartTime":"","EndTime":"","PageNo":1,"PageSize":3,"SUnitID":["8","2266","26552","26523"]}

            string Json = SearchService.Common.JsonHelper.SerializeObject(SP);

            webSocketClient.Send("RI " + Json);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AllSearchParam SP = new AllSearchParam();

            //SP.Range = SearchRange.TitleAndContext;
            //SP.RUnitID = new List<string>() { "76", "88" };
            SP.RUnitID = new List<string>();
            SP.InformationID = "6";
            SP.PageNo = 1;
            SP.PageSize = 2;

            string Json = SearchService.Common.JsonHelper.SerializeObject(SP);

            webSocketClient.Send("RI " + Json);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            webSocketClient.Send("RS Search");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            TiffInfor tif = new TiffInfor();
            tif.PageCount = 3;
            tif.ReceiverTel = "846214562";
            tif.SenderTel = "846200562";
            tif.Type = 1;

            string Json = "TI " + SearchService.Common.JsonHelper.SerializeObject(tif);

            webSocketClient.Send(Json);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            webSocketClient.Send("WIO Commit");
        }
    }
    class ConsoleWriter : StreamWriter
    {
        public ConsoleWriter(Stream stream, Encoding encoding, int bufferSize)
            : base(stream, encoding, bufferSize)
        {

        }

        private const string m_NewLine = "\r\n";

        public override void WriteLine()
        {
            Write(m_NewLine);
        }
    }
}

