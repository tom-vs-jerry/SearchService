namespace SearchService.Server
{
    partial class SMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SMain));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuService = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStartService = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReStartWebSocket = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReStartSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReStartKeySearch = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReStartSegmenter = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReloadDIC = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDisClientConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSegement = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDictManage = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSegementAlgo = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSearchAlgo = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAddInfors = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAddNewSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBatchAllSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBatchKeySearch = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAlarmIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTool = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBatchSens = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelpDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.toolBtnLocked = new System.Windows.Forms.ToolStripButton();
            this.toolBtnConfig = new System.Windows.Forms.ToolStripButton();
            this.toolBtnClose = new System.Windows.Forms.ToolStripButton();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.tsslServerType = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslConnNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslServerState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslBreakMarketOpen = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.fbdFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.menuMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuService,
            this.MenuSegement,
            this.MenuIndex,
            this.MenuTool,
            this.MenuWindows,
            this.MenuHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(591, 25);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(58, 21);
            this.MenuFile.Text = "文件(&F)";
            this.MenuFile.Visible = false;
            // 
            // MenuService
            // 
            this.MenuService.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStartService,
            this.MenuReStartWebSocket,
            this.MenuReStartSearch,
            this.MenuReStartKeySearch,
            this.MenuReStartSegmenter,
            this.MenuReloadDIC,
            this.MenuDisClientConnection});
            this.MenuService.Name = "MenuService";
            this.MenuService.Size = new System.Drawing.Size(59, 21);
            this.MenuService.Text = "启动(S)";
            // 
            // MenuStartService
            // 
            this.MenuStartService.Name = "MenuStartService";
            this.MenuStartService.Size = new System.Drawing.Size(190, 22);
            this.MenuStartService.Text = "启动所有服务";
            this.MenuStartService.Click += new System.EventHandler(this.MenuStartService_Click);
            // 
            // MenuReStartWebSocket
            // 
            this.MenuReStartWebSocket.Name = "MenuReStartWebSocket";
            this.MenuReStartWebSocket.Size = new System.Drawing.Size(190, 22);
            this.MenuReStartWebSocket.Text = "重启WebSocket服务";
            this.MenuReStartWebSocket.Click += new System.EventHandler(this.MenuReStartSocket_Click);
            // 
            // MenuReStartSearch
            // 
            this.MenuReStartSearch.Name = "MenuReStartSearch";
            this.MenuReStartSearch.Size = new System.Drawing.Size(190, 22);
            this.MenuReStartSearch.Text = "重启全文搜索服务";
            // 
            // MenuReStartKeySearch
            // 
            this.MenuReStartKeySearch.Name = "MenuReStartKeySearch";
            this.MenuReStartKeySearch.Size = new System.Drawing.Size(190, 22);
            this.MenuReStartKeySearch.Text = "重启敏感词搜索服务";
            // 
            // MenuReStartSegmenter
            // 
            this.MenuReStartSegmenter.Name = "MenuReStartSegmenter";
            this.MenuReStartSegmenter.Size = new System.Drawing.Size(190, 22);
            this.MenuReStartSegmenter.Text = "重启词典/分词服务";
            this.MenuReStartSegmenter.Click += new System.EventHandler(this.异常断开测试ToolStripMenuItem_Click);
            // 
            // MenuReloadDIC
            // 
            this.MenuReloadDIC.Name = "MenuReloadDIC";
            this.MenuReloadDIC.Size = new System.Drawing.Size(190, 22);
            this.MenuReloadDIC.Text = "重新加载词典";
            this.MenuReloadDIC.Click += new System.EventHandler(this.MenuReloadDIC_Click);
            // 
            // MenuDisClientConnection
            // 
            this.MenuDisClientConnection.Name = "MenuDisClientConnection";
            this.MenuDisClientConnection.Size = new System.Drawing.Size(190, 22);
            this.MenuDisClientConnection.Text = "显示客户端已连接数";
            this.MenuDisClientConnection.Click += new System.EventHandler(this.MenuDisClient_Click);
            // 
            // MenuSegement
            // 
            this.MenuSegement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuDictManage,
            this.MenuSegementAlgo,
            this.MenuSearchAlgo});
            this.MenuSegement.Name = "MenuSegement";
            this.MenuSegement.Size = new System.Drawing.Size(68, 21);
            this.MenuSegement.Text = "分词管理";
            // 
            // MenuDictManage
            // 
            this.MenuDictManage.Name = "MenuDictManage";
            this.MenuDictManage.Size = new System.Drawing.Size(148, 22);
            this.MenuDictManage.Text = "词典管理";
            this.MenuDictManage.Click += new System.EventHandler(this.MenuDictManage_Click);
            // 
            // MenuSegementAlgo
            // 
            this.MenuSegementAlgo.Enabled = false;
            this.MenuSegementAlgo.Name = "MenuSegementAlgo";
            this.MenuSegementAlgo.Size = new System.Drawing.Size(148, 22);
            this.MenuSegementAlgo.Text = "分词算法管理";
            // 
            // MenuSearchAlgo
            // 
            this.MenuSearchAlgo.Enabled = false;
            this.MenuSearchAlgo.Name = "MenuSearchAlgo";
            this.MenuSearchAlgo.Size = new System.Drawing.Size(148, 22);
            this.MenuSearchAlgo.Text = "搜索算法管理";
            // 
            // MenuIndex
            // 
            this.MenuIndex.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuReBuild,
            this.MenuAddInfors,
            this.MenuAddNewSearch,
            this.MenuBatchAllSearch,
            this.MenuBatchKeySearch,
            this.MenuAlarmIndex});
            this.MenuIndex.Name = "MenuIndex";
            this.MenuIndex.Size = new System.Drawing.Size(68, 21);
            this.MenuIndex.Text = "索引管理";
            // 
            // MenuReBuild
            // 
            this.MenuReBuild.Name = "MenuReBuild";
            this.MenuReBuild.Size = new System.Drawing.Size(184, 22);
            this.MenuReBuild.Text = "重建索引";
            // 
            // MenuAddInfors
            // 
            this.MenuAddInfors.Name = "MenuAddInfors";
            this.MenuAddInfors.Size = new System.Drawing.Size(184, 22);
            this.MenuAddInfors.Text = "添加信息";
            // 
            // MenuAddNewSearch
            // 
            this.MenuAddNewSearch.Name = "MenuAddNewSearch";
            this.MenuAddNewSearch.Size = new System.Drawing.Size(184, 22);
            this.MenuAddNewSearch.Text = "添加全文索引";
            this.MenuAddNewSearch.Click += new System.EventHandler(this.MenuAddNewSearch_Click);
            // 
            // MenuBatchAllSearch
            // 
            this.MenuBatchAllSearch.Name = "MenuBatchAllSearch";
            this.MenuBatchAllSearch.Size = new System.Drawing.Size(184, 22);
            this.MenuBatchAllSearch.Text = "创建全文索引";
            this.MenuBatchAllSearch.Click += new System.EventHandler(this.MenuBatch_Click);
            // 
            // MenuBatchKeySearch
            // 
            this.MenuBatchKeySearch.Name = "MenuBatchKeySearch";
            this.MenuBatchKeySearch.Size = new System.Drawing.Size(184, 22);
            this.MenuBatchKeySearch.Text = "创建敏感词索引";
            this.MenuBatchKeySearch.Click += new System.EventHandler(this.MenuBatchKeySearch_Click);
            // 
            // MenuAlarmIndex
            // 
            this.MenuAlarmIndex.Name = "MenuAlarmIndex";
            this.MenuAlarmIndex.Size = new System.Drawing.Size(184, 22);
            this.MenuAlarmIndex.Text = "创建接处警系统索引";
            this.MenuAlarmIndex.Click += new System.EventHandler(this.MenuAlarmIndex_Click);
            // 
            // MenuTool
            // 
            this.MenuTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuConfig,
            this.MenuBatchSens});
            this.MenuTool.Name = "MenuTool";
            this.MenuTool.Size = new System.Drawing.Size(59, 21);
            this.MenuTool.Text = "工具(&T)";
            // 
            // MenuConfig
            // 
            this.MenuConfig.Name = "MenuConfig";
            this.MenuConfig.Size = new System.Drawing.Size(160, 22);
            this.MenuConfig.Text = "设置(&S)";
            this.MenuConfig.Click += new System.EventHandler(this.toolBtnConfig_Click);
            // 
            // MenuBatchSens
            // 
            this.MenuBatchSens.Name = "MenuBatchSens";
            this.MenuBatchSens.Size = new System.Drawing.Size(160, 22);
            this.MenuBatchSens.Text = "批量处理敏感词";
            this.MenuBatchSens.Click += new System.EventHandler(this.MenuBatchSens_Click);
            // 
            // MenuWindows
            // 
            this.MenuWindows.Name = "MenuWindows";
            this.MenuWindows.Size = new System.Drawing.Size(64, 21);
            this.MenuWindows.Text = "窗口(&W)";
            this.MenuWindows.Visible = false;
            // 
            // MenuHelp
            // 
            this.MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHelpDoc,
            this.MenuAbout});
            this.MenuHelp.Name = "MenuHelp";
            this.MenuHelp.Size = new System.Drawing.Size(61, 21);
            this.MenuHelp.Text = "帮助(&H)";
            // 
            // MenuHelpDoc
            // 
            this.MenuHelpDoc.Name = "MenuHelpDoc";
            this.MenuHelpDoc.Size = new System.Drawing.Size(164, 22);
            this.MenuHelpDoc.Text = "帮助文档(&H)";
            // 
            // MenuAbout
            // 
            this.MenuAbout.Name = "MenuAbout";
            this.MenuAbout.Size = new System.Drawing.Size(164, 22);
            this.MenuAbout.Text = "关于搜索服务(&A)";
            this.MenuAbout.Click += new System.EventHandler(this.MenuAbout_Click);
            // 
            // toolMain
            // 
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnLocked,
            this.toolBtnConfig,
            this.toolBtnClose});
            this.toolMain.Location = new System.Drawing.Point(0, 25);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(591, 25);
            this.toolMain.TabIndex = 1;
            this.toolMain.Text = "toolStrip1";
            // 
            // toolBtnLocked
            // 
            this.toolBtnLocked.Image = global::SearchService.Server.Properties.Resources.toolbtnlocked_image;
            this.toolBtnLocked.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnLocked.Name = "toolBtnLocked";
            this.toolBtnLocked.Size = new System.Drawing.Size(52, 22);
            this.toolBtnLocked.Text = "锁定";
            this.toolBtnLocked.Click += new System.EventHandler(this.toolBtnLocked_Click);
            // 
            // toolBtnConfig
            // 
            this.toolBtnConfig.Image = global::SearchService.Server.Properties.Resources.toolbtnconfig_image;
            this.toolBtnConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnConfig.Name = "toolBtnConfig";
            this.toolBtnConfig.Size = new System.Drawing.Size(52, 22);
            this.toolBtnConfig.Text = "设置";
            this.toolBtnConfig.Click += new System.EventHandler(this.toolBtnConfig_Click);
            // 
            // toolBtnClose
            // 
            this.toolBtnClose.Image = global::SearchService.Server.Properties.Resources.toolbtnclose_image;
            this.toolBtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnClose.Name = "toolBtnClose";
            this.toolBtnClose.Size = new System.Drawing.Size(52, 22);
            this.toolBtnClose.Text = "退出";
            this.toolBtnClose.Click += new System.EventHandler(this.toolBtnClose_Click);
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslServerType,
            this.tsslConnNum,
            this.tsslServerState,
            this.tsslBreakMarketOpen});
            this.statusMain.Location = new System.Drawing.Point(0, 404);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(591, 22);
            this.statusMain.TabIndex = 2;
            this.statusMain.Text = "statusStrip1";
            // 
            // tsslServerType
            // 
            this.tsslServerType.Name = "tsslServerType";
            this.tsslServerType.Size = new System.Drawing.Size(0, 17);
            // 
            // tsslConnNum
            // 
            this.tsslConnNum.Name = "tsslConnNum";
            this.tsslConnNum.Size = new System.Drawing.Size(566, 17);
            this.tsslConnNum.Spring = true;
            // 
            // tsslServerState
            // 
            this.tsslServerState.Name = "tsslServerState";
            this.tsslServerState.Size = new System.Drawing.Size(0, 17);
            // 
            // tsslBreakMarketOpen
            // 
            this.tsslBreakMarketOpen.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.tsslBreakMarketOpen.Name = "tsslBreakMarketOpen";
            this.tsslBreakMarketOpen.Size = new System.Drawing.Size(0, 17);
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 63);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(591, 12);
            this.listBox1.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rtbMessage
            // 
            this.rtbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMessage.Location = new System.Drawing.Point(0, 50);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(591, 354);
            this.rtbMessage.TabIndex = 4;
            this.rtbMessage.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 426);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.toolMain);
            this.Controls.Add(this.menuMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信息处理平台 V1.0 - 搜索服务";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SMainUI_FormClosing);
            this.Load += new System.EventHandler(this.SMainUI_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuWindows;
        private System.Windows.Forms.ToolStripMenuItem MenuHelp;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripButton toolBtnLocked;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripMenuItem MenuTool;
        private System.Windows.Forms.ToolStripMenuItem MenuConfig;
        private System.Windows.Forms.ToolStripMenuItem MenuHelpDoc;
        private System.Windows.Forms.ToolStripMenuItem MenuAbout;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripMenuItem MenuService;
        private System.Windows.Forms.ToolStripMenuItem MenuStartService;
        private System.Windows.Forms.ToolStripMenuItem MenuReStartWebSocket;
        private System.Windows.Forms.ToolStripMenuItem MenuDisClientConnection;
        private System.Windows.Forms.ToolStripStatusLabel tsslServerType;
        private System.Windows.Forms.ToolStripButton toolBtnConfig;
        private System.Windows.Forms.ToolStripButton toolBtnClose;
        private System.Windows.Forms.ToolStripStatusLabel tsslConnNum;
        private System.Windows.Forms.ToolStripStatusLabel tsslServerState;
        private System.Windows.Forms.ToolStripStatusLabel tsslBreakMarketOpen;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem MenuReStartSegmenter;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.ToolStripMenuItem MenuReloadDIC;
        private System.Windows.Forms.ToolStripMenuItem MenuSegement;
        private System.Windows.Forms.ToolStripMenuItem MenuSegementAlgo;
        private System.Windows.Forms.ToolStripMenuItem MenuDictManage;
        private System.Windows.Forms.ToolStripMenuItem MenuSearchAlgo;
        private System.Windows.Forms.ToolStripMenuItem MenuReStartSearch;
        private System.Windows.Forms.ToolStripMenuItem MenuIndex;
        private System.Windows.Forms.ToolStripMenuItem MenuReBuild;
        private System.Windows.Forms.ToolStripMenuItem MenuAddInfors;
        //private System.Windows.Forms.ToolStripMenuItem MenuAddNewSearch;
        private System.Windows.Forms.ToolStripMenuItem MenuBatchAllSearch;
        private System.Windows.Forms.FolderBrowserDialog fbdFolderBrowser;
        private System.Windows.Forms.ToolStripMenuItem MenuReStartKeySearch;
        private System.Windows.Forms.ToolStripMenuItem MenuBatchKeySearch;
        private System.Windows.Forms.ToolStripMenuItem MenuBatchSens;
        private System.Windows.Forms.ToolStripMenuItem MenuAlarmIndex;
        private System.Windows.Forms.ToolStripMenuItem MenuAddNewSearch;
    }
}