namespace SearchService.Server.SSystemSet
{
    partial class SSystemSetUI
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.tbcSet = new System.Windows.Forms.TabControl();
            this.tbpWebSocket = new System.Windows.Forms.TabPage();
            this.tbxIP = new System.Windows.Forms.TextBox();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.tbpDic = new System.Windows.Forms.TabPage();
            this.tbxDicPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbpLog = new System.Windows.Forms.TabPage();
            this.tbxLog = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.fbdLog = new System.Windows.Forms.FolderBrowserDialog();
            this.tbcSet.SuspendLayout();
            this.tbpWebSocket.SuspendLayout();
            this.tbpDic.SuspendLayout();
            this.tbpLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "服务IP地址：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "服务端口：";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(285, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Enabled = false;
            this.btnConfirm.Location = new System.Drawing.Point(198, 201);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(61, 23);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Visible = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // tbcSet
            // 
            this.tbcSet.Controls.Add(this.tbpWebSocket);
            this.tbcSet.Controls.Add(this.tbpDic);
            this.tbcSet.Controls.Add(this.tbpLog);
            this.tbcSet.Location = new System.Drawing.Point(23, 12);
            this.tbcSet.Name = "tbcSet";
            this.tbcSet.SelectedIndex = 0;
            this.tbcSet.Size = new System.Drawing.Size(327, 178);
            this.tbcSet.TabIndex = 8;
            // 
            // tbpWebSocket
            // 
            this.tbpWebSocket.Controls.Add(this.tbxIP);
            this.tbpWebSocket.Controls.Add(this.tbxPort);
            this.tbpWebSocket.Controls.Add(this.label2);
            this.tbpWebSocket.Controls.Add(this.label3);
            this.tbpWebSocket.Location = new System.Drawing.Point(4, 22);
            this.tbpWebSocket.Name = "tbpWebSocket";
            this.tbpWebSocket.Padding = new System.Windows.Forms.Padding(3);
            this.tbpWebSocket.Size = new System.Drawing.Size(319, 152);
            this.tbpWebSocket.TabIndex = 0;
            this.tbpWebSocket.Text = "WebSocket服务配置";
            this.tbpWebSocket.UseVisualStyleBackColor = true;
            // 
            // tbxIP
            // 
            this.tbxIP.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxIP.Location = new System.Drawing.Point(111, 44);
            this.tbxIP.Name = "tbxIP";
            this.tbxIP.Size = new System.Drawing.Size(166, 21);
            this.tbxIP.TabIndex = 5;
            this.tbxIP.Text = "192.168.0.50";
            // 
            // tbxPort
            // 
            this.tbxPort.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxPort.Location = new System.Drawing.Point(111, 84);
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(166, 21);
            this.tbxPort.TabIndex = 4;
            this.tbxPort.Text = "8080";
            // 
            // tbpDic
            // 
            this.tbpDic.Controls.Add(this.tbxDicPath);
            this.tbpDic.Controls.Add(this.label5);
            this.tbpDic.Location = new System.Drawing.Point(4, 22);
            this.tbpDic.Name = "tbpDic";
            this.tbpDic.Size = new System.Drawing.Size(319, 152);
            this.tbpDic.TabIndex = 2;
            this.tbpDic.Text = "词典配置";
            this.tbpDic.UseVisualStyleBackColor = true;
            // 
            // tbxDicPath
            // 
            this.tbxDicPath.Location = new System.Drawing.Point(84, 65);
            this.tbxDicPath.Name = "tbxDicPath";
            this.tbxDicPath.Size = new System.Drawing.Size(213, 21);
            this.tbxDicPath.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "词典路径：";
            // 
            // tbpLog
            // 
            this.tbpLog.Controls.Add(this.tbxLog);
            this.tbpLog.Controls.Add(this.label4);
            this.tbpLog.Controls.Add(this.btnSave);
            this.tbpLog.Location = new System.Drawing.Point(4, 22);
            this.tbpLog.Name = "tbpLog";
            this.tbpLog.Padding = new System.Windows.Forms.Padding(3);
            this.tbpLog.Size = new System.Drawing.Size(319, 152);
            this.tbpLog.TabIndex = 1;
            this.tbpLog.Text = "日志模块配置";
            this.tbpLog.UseVisualStyleBackColor = true;
            // 
            // tbxLog
            // 
            this.tbxLog.Location = new System.Drawing.Point(97, 64);
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.Size = new System.Drawing.Size(171, 21);
            this.tbxLog.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "日志保存路径：";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(274, 64);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(28, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "……";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SSystemSetUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 231);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbcSet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SSystemSetUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.SSystemSetUI_Load);
            this.tbcSet.ResumeLayout(false);
            this.tbpWebSocket.ResumeLayout(false);
            this.tbpWebSocket.PerformLayout();
            this.tbpDic.ResumeLayout(false);
            this.tbpDic.PerformLayout();
            this.tbpLog.ResumeLayout(false);
            this.tbpLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.TabControl tbcSet;
        private System.Windows.Forms.TabPage tbpWebSocket;
        private System.Windows.Forms.TabPage tbpLog;
        private System.Windows.Forms.TextBox tbxLog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.FolderBrowserDialog fbdLog;
        private System.Windows.Forms.TabPage tbpDic;
        private System.Windows.Forms.TextBox tbxDicPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxIP;
        private System.Windows.Forms.TextBox tbxPort;
    }
}