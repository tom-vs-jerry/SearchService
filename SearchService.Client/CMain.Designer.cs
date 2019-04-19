namespace SearchService.Client
{
    partial class CMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartConnetServer = new System.Windows.Forms.Button();
            this.cbbIP = new System.Windows.Forms.ComboBox();
            this.cbbPort = new System.Windows.Forms.ComboBox();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.btnECHO = new System.Windows.Forms.Button();
            this.btnADD = new System.Windows.Forms.Button();
            this.btnMULT = new System.Windows.Forms.Button();
            this.btnMES = new System.Windows.Forms.Button();
            this.btnPING = new System.Windows.Forms.Button();
            this.btnQUIT = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button13 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnSendDoc = new System.Windows.Forms.Button();
            this.btnMULTX = new System.Windows.Forms.Button();
            this.btnLOGIN = new System.Windows.Forms.Button();
            this.btnADDX = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.cbbIP1 = new System.Windows.Forms.ComboBox();
            this.cbbPort1 = new System.Windows.Forms.ComboBox();
            this.fbdFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdFiles = new System.Windows.Forms.OpenFileDialog();
            this.button14 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartConnetServer
            // 
            this.btnStartConnetServer.Location = new System.Drawing.Point(166, 18);
            this.btnStartConnetServer.Name = "btnStartConnetServer";
            this.btnStartConnetServer.Size = new System.Drawing.Size(75, 23);
            this.btnStartConnetServer.TabIndex = 0;
            this.btnStartConnetServer.Text = "连接服务";
            this.btnStartConnetServer.UseVisualStyleBackColor = true;
            this.btnStartConnetServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // cbbIP
            // 
            this.cbbIP.FormattingEnabled = true;
            this.cbbIP.Items.AddRange(new object[] {
            "localhost",
            "127.0.0.1",
            "192.168.0.50",
            "192.168.0.51"});
            this.cbbIP.Location = new System.Drawing.Point(6, 19);
            this.cbbIP.Name = "cbbIP";
            this.cbbIP.Size = new System.Drawing.Size(95, 20);
            this.cbbIP.TabIndex = 1;
            // 
            // cbbPort
            // 
            this.cbbPort.FormattingEnabled = true;
            this.cbbPort.Items.AddRange(new object[] {
            "81",
            "82",
            "2012",
            "2020"});
            this.cbbPort.Location = new System.Drawing.Point(107, 19);
            this.cbbPort.Name = "cbbPort";
            this.cbbPort.Size = new System.Drawing.Size(54, 20);
            this.cbbPort.TabIndex = 2;
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(6, 198);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(915, 180);
            this.rtbMessage.TabIndex = 8;
            this.rtbMessage.Text = "";
            // 
            // btnECHO
            // 
            this.btnECHO.Location = new System.Drawing.Point(247, 19);
            this.btnECHO.Name = "btnECHO";
            this.btnECHO.Size = new System.Drawing.Size(49, 23);
            this.btnECHO.TabIndex = 9;
            this.btnECHO.Text = "ECHO";
            this.btnECHO.UseVisualStyleBackColor = true;
            this.btnECHO.Click += new System.EventHandler(this.btnECHO_Click);
            // 
            // btnADD
            // 
            this.btnADD.Location = new System.Drawing.Point(379, 18);
            this.btnADD.Name = "btnADD";
            this.btnADD.Size = new System.Drawing.Size(49, 23);
            this.btnADD.TabIndex = 10;
            this.btnADD.Text = "ADD";
            this.btnADD.UseVisualStyleBackColor = true;
            this.btnADD.Click += new System.EventHandler(this.btnADD_Click);
            // 
            // btnMULT
            // 
            this.btnMULT.Location = new System.Drawing.Point(489, 18);
            this.btnMULT.Name = "btnMULT";
            this.btnMULT.Size = new System.Drawing.Size(49, 23);
            this.btnMULT.TabIndex = 11;
            this.btnMULT.Text = "MULT";
            this.btnMULT.UseVisualStyleBackColor = true;
            this.btnMULT.Click += new System.EventHandler(this.btnMULT_Click);
            // 
            // btnMES
            // 
            this.btnMES.Location = new System.Drawing.Point(302, 18);
            this.btnMES.Name = "btnMES";
            this.btnMES.Size = new System.Drawing.Size(71, 23);
            this.btnMES.TabIndex = 12;
            this.btnMES.Text = "ECHOJson";
            this.btnMES.UseVisualStyleBackColor = true;
            this.btnMES.Click += new System.EventHandler(this.btnMES_Click);
            // 
            // btnPING
            // 
            this.btnPING.Location = new System.Drawing.Point(709, 18);
            this.btnPING.Name = "btnPING";
            this.btnPING.Size = new System.Drawing.Size(49, 23);
            this.btnPING.TabIndex = 13;
            this.btnPING.Text = "ping";
            this.btnPING.UseVisualStyleBackColor = true;
            this.btnPING.Click += new System.EventHandler(this.btnPING_Click);
            // 
            // btnQUIT
            // 
            this.btnQUIT.Location = new System.Drawing.Point(654, 17);
            this.btnQUIT.Name = "btnQUIT";
            this.btnQUIT.Size = new System.Drawing.Size(49, 23);
            this.btnQUIT.TabIndex = 14;
            this.btnQUIT.Text = "QUIT";
            this.btnQUIT.UseVisualStyleBackColor = true;
            this.btnQUIT.Click += new System.EventHandler(this.btnQUIT_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button14);
            this.groupBox1.Controls.Add(this.button13);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.button11);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.btnSendDoc);
            this.groupBox1.Controls.Add(this.btnMULTX);
            this.groupBox1.Controls.Add(this.btnLOGIN);
            this.groupBox1.Controls.Add(this.btnADDX);
            this.groupBox1.Controls.Add(this.btnQUIT);
            this.groupBox1.Controls.Add(this.cbbPort);
            this.groupBox1.Controls.Add(this.btnECHO);
            this.groupBox1.Controls.Add(this.cbbIP);
            this.groupBox1.Controls.Add(this.btnStartConnetServer);
            this.groupBox1.Controls.Add(this.btnMES);
            this.groupBox1.Controls.Add(this.btnPING);
            this.groupBox1.Controls.Add(this.btnADD);
            this.groupBox1.Controls.Add(this.btnMULT);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(908, 113);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Command";
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(148, 51);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(128, 23);
            this.button13.TabIndex = 27;
            this.button13.Text = "发送单个文件信息";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(822, 49);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 24;
            this.button9.Text = "查询";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(764, 18);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 26;
            this.button12.Text = "WIO";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(446, 51);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 25;
            this.button11.Text = "传真";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(845, 19);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(49, 23);
            this.button10.TabIndex = 24;
            this.button10.Text = "QUIT";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(741, 49);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 23;
            this.button8.Text = "查询";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(660, 49);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 22;
            this.button7.Text = "查询";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(282, 53);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(77, 21);
            this.textBox1.TabIndex = 21;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(579, 49);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(365, 51);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 19;
            this.button6.Text = "联想";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btnSendDoc
            // 
            this.btnSendDoc.Location = new System.Drawing.Point(7, 49);
            this.btnSendDoc.Name = "btnSendDoc";
            this.btnSendDoc.Size = new System.Drawing.Size(128, 23);
            this.btnSendDoc.TabIndex = 18;
            this.btnSendDoc.Text = "发送所选文件夹PDF";
            this.btnSendDoc.UseVisualStyleBackColor = true;
            this.btnSendDoc.Click += new System.EventHandler(this.btnSendDoc_Click);
            // 
            // btnMULTX
            // 
            this.btnMULTX.Location = new System.Drawing.Point(544, 18);
            this.btnMULTX.Name = "btnMULTX";
            this.btnMULTX.Size = new System.Drawing.Size(49, 23);
            this.btnMULTX.TabIndex = 17;
            this.btnMULTX.Text = "MULTX";
            this.btnMULTX.UseVisualStyleBackColor = true;
            this.btnMULTX.Click += new System.EventHandler(this.btnMULTX_Click);
            // 
            // btnLOGIN
            // 
            this.btnLOGIN.Location = new System.Drawing.Point(599, 17);
            this.btnLOGIN.Name = "btnLOGIN";
            this.btnLOGIN.Size = new System.Drawing.Size(49, 23);
            this.btnLOGIN.TabIndex = 16;
            this.btnLOGIN.Text = "LOGIN";
            this.btnLOGIN.UseVisualStyleBackColor = true;
            this.btnLOGIN.Click += new System.EventHandler(this.btnLOGIN_Click);
            // 
            // btnADDX
            // 
            this.btnADDX.Location = new System.Drawing.Point(434, 17);
            this.btnADDX.Name = "btnADDX";
            this.btnADDX.Size = new System.Drawing.Size(49, 23);
            this.btnADDX.TabIndex = 15;
            this.btnADDX.Text = "ADDX";
            this.btnADDX.UseVisualStyleBackColor = true;
            this.btnADDX.Click += new System.EventHandler(this.btnADDX_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(338, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 23);
            this.button3.TabIndex = 19;
            this.button3.Text = "ECHO-Class";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(248, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "ECHO-string";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "连接JsonWebServer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.cbbIP1);
            this.groupBox2.Controls.Add(this.cbbPort1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(14, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(907, 58);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "JsonWebSocket";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(509, 17);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 19;
            this.button5.Text = "发送所选";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(428, 17);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 21;
            this.button4.Text = "MULTX";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // cbbIP1
            // 
            this.cbbIP1.FormattingEnabled = true;
            this.cbbIP1.Items.AddRange(new object[] {
            "localhost",
            "127.0.0.1",
            "192.168.0.50",
            "192.168.0.51"});
            this.cbbIP1.Location = new System.Drawing.Point(6, 20);
            this.cbbIP1.Name = "cbbIP1";
            this.cbbIP1.Size = new System.Drawing.Size(121, 20);
            this.cbbIP1.TabIndex = 18;
            // 
            // cbbPort1
            // 
            this.cbbPort1.FormattingEnabled = true;
            this.cbbPort1.Items.AddRange(new object[] {
            "81",
            "82",
            "2012",
            "2020"});
            this.cbbPort1.Location = new System.Drawing.Point(131, 20);
            this.cbbPort1.Name = "cbbPort1";
            this.cbbPort1.Size = new System.Drawing.Size(54, 20);
            this.cbbPort1.TabIndex = 20;
            // 
            // ofdFiles
            // 
            this.ofdFiles.Filter = "(.json,.pdf)|*.json;*.pdf";
            this.ofdFiles.Title = "请选择单个文件（json，pdf）";
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(7, 79);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(128, 23);
            this.button14.TabIndex = 28;
            this.button14.Text = "测试单一json文件";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // CMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 407);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtbMessage);
            this.Name = "CMain";
            this.Text = "ClientCommand";
            this.Load += new System.EventHandler(this.CMainUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartConnetServer;
        private System.Windows.Forms.ComboBox cbbIP;
        private System.Windows.Forms.ComboBox cbbPort;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Button btnECHO;
        private System.Windows.Forms.Button btnADD;
        private System.Windows.Forms.Button btnMULT;
        private System.Windows.Forms.Button btnMES;
        private System.Windows.Forms.Button btnPING;
        private System.Windows.Forms.Button btnQUIT;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnADDX;
        private System.Windows.Forms.Button btnLOGIN;
        private System.Windows.Forms.Button btnMULTX;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbbPort1;
        private System.Windows.Forms.ComboBox cbbIP1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnSendDoc;
        private System.Windows.Forms.FolderBrowserDialog fbdFolder;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.OpenFileDialog ofdFiles;
        private System.Windows.Forms.Button button14;
    }
}