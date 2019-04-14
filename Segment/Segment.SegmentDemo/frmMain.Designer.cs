namespace Segment.SegmentDemo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label14 = new System.Windows.Forms.Label();
            this.numericUpDownRedundancy = new System.Windows.Forms.NumericUpDown();
            this.checkBoxMultiSelect = new System.Windows.Forms.CheckBox();
            this.buttonSaveConfig = new System.Windows.Forms.Button();
            this.checkBoxFreqFirst = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayPosition = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterStopWords = new System.Windows.Forms.CheckBox();
            this.checkBoxMatchName = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelSrcLength = new System.Windows.Forms.Label();
            this.labelRegRate = new System.Windows.Forms.Label();
            this.labelSegTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSegment = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSegwords = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxForceSingleWord = new System.Windows.Forms.CheckBox();
            this.checkBoxTraditionalChs = new System.Windows.Forms.CheckBox();
            this.checkBoxST = new System.Windows.Forms.CheckBox();
            this.checkBoxUnknownWord = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterEnglish = new System.Windows.Forms.CheckBox();
            this.numericUpDownFilterEnglishLength = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.labelFilterNumericLength = new System.Windows.Forms.Label();
            this.numericUpDownFilterNumericLength = new System.Windows.Forms.NumericUpDown();
            this.checkBoxFilterNumeric = new System.Windows.Forms.CheckBox();
            this.checkBoxIgnoreCapital = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTimeOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxEnglishSegment = new System.Windows.Forms.CheckBox();
            this.checkBoxSynonymOutput = new System.Windows.Forms.CheckBox();
            this.checkBoxWildcard = new System.Windows.Forms.CheckBox();
            this.checkBoxWildcardSegment = new System.Windows.Forms.CheckBox();
            this.checkBoxCustomRule = new System.Windows.Forms.CheckBox();
            this.checkBoxEnglishMultiSelect = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.fbdFileFold = new System.Windows.Forms.FolderBrowserDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxSource = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRedundancy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterEnglishLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterNumericLength)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(294, 598);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 87;
            this.label14.Text = "冗余度";
            // 
            // numericUpDownRedundancy
            // 
            this.numericUpDownRedundancy.Location = new System.Drawing.Point(387, 596);
            this.numericUpDownRedundancy.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownRedundancy.Name = "numericUpDownRedundancy";
            this.numericUpDownRedundancy.Size = new System.Drawing.Size(52, 21);
            this.numericUpDownRedundancy.TabIndex = 86;
            this.numericUpDownRedundancy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBoxMultiSelect
            // 
            this.checkBoxMultiSelect.AutoSize = true;
            this.checkBoxMultiSelect.Location = new System.Drawing.Point(191, 598);
            this.checkBoxMultiSelect.Name = "checkBoxMultiSelect";
            this.checkBoxMultiSelect.Size = new System.Drawing.Size(72, 16);
            this.checkBoxMultiSelect.TabIndex = 85;
            this.checkBoxMultiSelect.Text = "多元分词";
            this.checkBoxMultiSelect.UseVisualStyleBackColor = true;
            // 
            // buttonSaveConfig
            // 
            this.buttonSaveConfig.Location = new System.Drawing.Point(91, 574);
            this.buttonSaveConfig.Name = "buttonSaveConfig";
            this.buttonSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveConfig.TabIndex = 84;
            this.buttonSaveConfig.Text = "保存配置";
            this.buttonSaveConfig.UseVisualStyleBackColor = true;
            this.buttonSaveConfig.Click += new System.EventHandler(this.buttonSaveConfig_Click);
            // 
            // checkBoxFreqFirst
            // 
            this.checkBoxFreqFirst.AutoSize = true;
            this.checkBoxFreqFirst.Checked = true;
            this.checkBoxFreqFirst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFreqFirst.Location = new System.Drawing.Point(494, 577);
            this.checkBoxFreqFirst.Name = "checkBoxFreqFirst";
            this.checkBoxFreqFirst.Size = new System.Drawing.Size(96, 16);
            this.checkBoxFreqFirst.TabIndex = 78;
            this.checkBoxFreqFirst.Text = "优先判断词频";
            this.checkBoxFreqFirst.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisplayPosition
            // 
            this.checkBoxDisplayPosition.AutoSize = true;
            this.checkBoxDisplayPosition.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBoxDisplayPosition.Location = new System.Drawing.Point(387, 577);
            this.checkBoxDisplayPosition.Name = "checkBoxDisplayPosition";
            this.checkBoxDisplayPosition.Size = new System.Drawing.Size(96, 16);
            this.checkBoxDisplayPosition.TabIndex = 76;
            this.checkBoxDisplayPosition.Text = "显示单词位置";
            this.checkBoxDisplayPosition.UseVisualStyleBackColor = true;
            // 
            // checkBoxFilterStopWords
            // 
            this.checkBoxFilterStopWords.AutoSize = true;
            this.checkBoxFilterStopWords.Location = new System.Drawing.Point(298, 577);
            this.checkBoxFilterStopWords.Name = "checkBoxFilterStopWords";
            this.checkBoxFilterStopWords.Size = new System.Drawing.Size(84, 16);
            this.checkBoxFilterStopWords.TabIndex = 74;
            this.checkBoxFilterStopWords.Text = "过滤停用词";
            this.checkBoxFilterStopWords.UseVisualStyleBackColor = true;
            // 
            // checkBoxMatchName
            // 
            this.checkBoxMatchName.AutoSize = true;
            this.checkBoxMatchName.Checked = true;
            this.checkBoxMatchName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMatchName.Location = new System.Drawing.Point(191, 577);
            this.checkBoxMatchName.Name = "checkBoxMatchName";
            this.checkBoxMatchName.Size = new System.Drawing.Size(96, 16);
            this.checkBoxMatchName.TabIndex = 73;
            this.checkBoxMatchName.Text = "识别中文人名";
            this.checkBoxMatchName.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(189, 549);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 69;
            this.label3.Text = "chars";
            // 
            // labelSrcLength
            // 
            this.labelSrcLength.AutoSize = true;
            this.labelSrcLength.Location = new System.Drawing.Point(112, 549);
            this.labelSrcLength.Name = "labelSrcLength";
            this.labelSrcLength.Size = new System.Drawing.Size(11, 12);
            this.labelSrcLength.TabIndex = 68;
            this.labelSrcLength.Text = "0";
            // 
            // labelRegRate
            // 
            this.labelRegRate.AutoSize = true;
            this.labelRegRate.Location = new System.Drawing.Point(502, 549);
            this.labelRegRate.Name = "labelRegRate";
            this.labelRegRate.Size = new System.Drawing.Size(11, 12);
            this.labelRegRate.TabIndex = 67;
            this.labelRegRate.Text = "0";
            // 
            // labelSegTime
            // 
            this.labelSegTime.AutoSize = true;
            this.labelSegTime.Location = new System.Drawing.Point(296, 549);
            this.labelSegTime.Name = "labelSegTime";
            this.labelSegTime.Size = new System.Drawing.Size(11, 12);
            this.labelSegTime.TabIndex = 66;
            this.labelSegTime.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(616, 549);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 65;
            this.label8.Text = "chars/s";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(416, 549);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 64;
            this.label7.Text = "s";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(433, 549);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 63;
            this.label6.Text = "分词速度";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(228, 549);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 62;
            this.label5.Text = "分词时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 549);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 61;
            this.label4.Text = "源字符串长度";
            // 
            // buttonSegment
            // 
            this.buttonSegment.Location = new System.Drawing.Point(10, 574);
            this.buttonSegment.Name = "buttonSegment";
            this.buttonSegment.Size = new System.Drawing.Size(75, 23);
            this.buttonSegment.TabIndex = 60;
            this.buttonSegment.Text = "分词";
            this.buttonSegment.UseVisualStyleBackColor = true;
            this.buttonSegment.Click += new System.EventHandler(this.buttonSegment_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 384);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 59;
            this.label2.Text = "分词结果";
            // 
            // textBoxSegwords
            // 
            this.textBoxSegwords.Location = new System.Drawing.Point(1, 406);
            this.textBoxSegwords.Multiline = true;
            this.textBoxSegwords.Name = "textBoxSegwords";
            this.textBoxSegwords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSegwords.Size = new System.Drawing.Size(1171, 132);
            this.textBoxSegwords.TabIndex = 58;
            this.textBoxSegwords.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 57;
            this.label1.Text = "源文";
            // 
            // checkBoxForceSingleWord
            // 
            this.checkBoxForceSingleWord.AutoSize = true;
            this.checkBoxForceSingleWord.Location = new System.Drawing.Point(494, 597);
            this.checkBoxForceSingleWord.Name = "checkBoxForceSingleWord";
            this.checkBoxForceSingleWord.Size = new System.Drawing.Size(96, 16);
            this.checkBoxForceSingleWord.TabIndex = 89;
            this.checkBoxForceSingleWord.Text = "强制一元分词";
            this.checkBoxForceSingleWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxTraditionalChs
            // 
            this.checkBoxTraditionalChs.AutoSize = true;
            this.checkBoxTraditionalChs.Checked = true;
            this.checkBoxTraditionalChs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTraditionalChs.Location = new System.Drawing.Point(599, 575);
            this.checkBoxTraditionalChs.Name = "checkBoxTraditionalChs";
            this.checkBoxTraditionalChs.Size = new System.Drawing.Size(84, 16);
            this.checkBoxTraditionalChs.TabIndex = 90;
            this.checkBoxTraditionalChs.Text = "繁体字分词";
            this.checkBoxTraditionalChs.UseVisualStyleBackColor = true;
            // 
            // checkBoxST
            // 
            this.checkBoxST.AutoSize = true;
            this.checkBoxST.Checked = true;
            this.checkBoxST.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxST.Location = new System.Drawing.Point(702, 574);
            this.checkBoxST.Name = "checkBoxST";
            this.checkBoxST.Size = new System.Drawing.Size(132, 16);
            this.checkBoxST.TabIndex = 91;
            this.checkBoxST.Text = "同时输出简体和繁体";
            this.checkBoxST.UseVisualStyleBackColor = true;
            // 
            // checkBoxUnknownWord
            // 
            this.checkBoxUnknownWord.AutoSize = true;
            this.checkBoxUnknownWord.Location = new System.Drawing.Point(599, 596);
            this.checkBoxUnknownWord.Name = "checkBoxUnknownWord";
            this.checkBoxUnknownWord.Size = new System.Drawing.Size(96, 16);
            this.checkBoxUnknownWord.TabIndex = 92;
            this.checkBoxUnknownWord.Text = "未登录词识别";
            this.checkBoxUnknownWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxFilterEnglish
            // 
            this.checkBoxFilterEnglish.AutoSize = true;
            this.checkBoxFilterEnglish.Location = new System.Drawing.Point(191, 622);
            this.checkBoxFilterEnglish.Name = "checkBoxFilterEnglish";
            this.checkBoxFilterEnglish.Size = new System.Drawing.Size(72, 16);
            this.checkBoxFilterEnglish.TabIndex = 93;
            this.checkBoxFilterEnglish.Text = "过滤英文";
            this.checkBoxFilterEnglish.UseVisualStyleBackColor = true;
            // 
            // numericUpDownFilterEnglishLength
            // 
            this.numericUpDownFilterEnglishLength.Location = new System.Drawing.Point(387, 619);
            this.numericUpDownFilterEnglishLength.Name = "numericUpDownFilterEnglishLength";
            this.numericUpDownFilterEnglishLength.Size = new System.Drawing.Size(52, 21);
            this.numericUpDownFilterEnglishLength.TabIndex = 94;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(294, 623);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 95;
            this.label11.Text = "过滤英文长度";
            // 
            // labelFilterNumericLength
            // 
            this.labelFilterNumericLength.AutoSize = true;
            this.labelFilterNumericLength.Location = new System.Drawing.Point(599, 621);
            this.labelFilterNumericLength.Name = "labelFilterNumericLength";
            this.labelFilterNumericLength.Size = new System.Drawing.Size(77, 12);
            this.labelFilterNumericLength.TabIndex = 98;
            this.labelFilterNumericLength.Text = "过滤数字长度";
            // 
            // numericUpDownFilterNumericLength
            // 
            this.numericUpDownFilterNumericLength.Location = new System.Drawing.Point(692, 618);
            this.numericUpDownFilterNumericLength.Name = "numericUpDownFilterNumericLength";
            this.numericUpDownFilterNumericLength.Size = new System.Drawing.Size(52, 21);
            this.numericUpDownFilterNumericLength.TabIndex = 97;
            // 
            // checkBoxFilterNumeric
            // 
            this.checkBoxFilterNumeric.AutoSize = true;
            this.checkBoxFilterNumeric.Location = new System.Drawing.Point(494, 621);
            this.checkBoxFilterNumeric.Name = "checkBoxFilterNumeric";
            this.checkBoxFilterNumeric.Size = new System.Drawing.Size(72, 16);
            this.checkBoxFilterNumeric.TabIndex = 96;
            this.checkBoxFilterNumeric.Text = "过滤数字";
            this.checkBoxFilterNumeric.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreCapital
            // 
            this.checkBoxIgnoreCapital.AutoSize = true;
            this.checkBoxIgnoreCapital.Location = new System.Drawing.Point(702, 595);
            this.checkBoxIgnoreCapital.Name = "checkBoxIgnoreCapital";
            this.checkBoxIgnoreCapital.Size = new System.Drawing.Size(108, 16);
            this.checkBoxIgnoreCapital.TabIndex = 99;
            this.checkBoxIgnoreCapital.Text = "忽略英文大小写";
            this.checkBoxIgnoreCapital.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowTimeOnly
            // 
            this.checkBoxShowTimeOnly.AutoSize = true;
            this.checkBoxShowTimeOnly.Location = new System.Drawing.Point(10, 620);
            this.checkBoxShowTimeOnly.Name = "checkBoxShowTimeOnly";
            this.checkBoxShowTimeOnly.Size = new System.Drawing.Size(108, 16);
            this.checkBoxShowTimeOnly.TabIndex = 100;
            this.checkBoxShowTimeOnly.Text = "仅显示分词时间";
            this.checkBoxShowTimeOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnglishSegment
            // 
            this.checkBoxEnglishSegment.AutoSize = true;
            this.checkBoxEnglishSegment.Location = new System.Drawing.Point(297, 646);
            this.checkBoxEnglishSegment.Name = "checkBoxEnglishSegment";
            this.checkBoxEnglishSegment.Size = new System.Drawing.Size(72, 16);
            this.checkBoxEnglishSegment.TabIndex = 101;
            this.checkBoxEnglishSegment.Text = "英文分词";
            this.checkBoxEnglishSegment.UseVisualStyleBackColor = true;
            // 
            // checkBoxSynonymOutput
            // 
            this.checkBoxSynonymOutput.AutoSize = true;
            this.checkBoxSynonymOutput.Location = new System.Drawing.Point(191, 668);
            this.checkBoxSynonymOutput.Name = "checkBoxSynonymOutput";
            this.checkBoxSynonymOutput.Size = new System.Drawing.Size(84, 16);
            this.checkBoxSynonymOutput.TabIndex = 102;
            this.checkBoxSynonymOutput.Text = "输出同义词";
            this.checkBoxSynonymOutput.UseVisualStyleBackColor = true;
            // 
            // checkBoxWildcard
            // 
            this.checkBoxWildcard.AutoSize = true;
            this.checkBoxWildcard.Location = new System.Drawing.Point(283, 668);
            this.checkBoxWildcard.Name = "checkBoxWildcard";
            this.checkBoxWildcard.Size = new System.Drawing.Size(84, 16);
            this.checkBoxWildcard.TabIndex = 103;
            this.checkBoxWildcard.Text = "通配符匹配";
            this.checkBoxWildcard.UseVisualStyleBackColor = true;
            // 
            // checkBoxWildcardSegment
            // 
            this.checkBoxWildcardSegment.AutoSize = true;
            this.checkBoxWildcardSegment.Location = new System.Drawing.Point(388, 668);
            this.checkBoxWildcardSegment.Name = "checkBoxWildcardSegment";
            this.checkBoxWildcardSegment.Size = new System.Drawing.Size(180, 16);
            this.checkBoxWildcardSegment.TabIndex = 104;
            this.checkBoxWildcardSegment.Text = "对通配符匹配出来的词再分词";
            this.checkBoxWildcardSegment.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomRule
            // 
            this.checkBoxCustomRule.AutoSize = true;
            this.checkBoxCustomRule.Location = new System.Drawing.Point(586, 667);
            this.checkBoxCustomRule.Name = "checkBoxCustomRule";
            this.checkBoxCustomRule.Size = new System.Drawing.Size(84, 16);
            this.checkBoxCustomRule.TabIndex = 105;
            this.checkBoxCustomRule.Text = "自定义规则";
            this.checkBoxCustomRule.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnglishMultiSelect
            // 
            this.checkBoxEnglishMultiSelect.AutoSize = true;
            this.checkBoxEnglishMultiSelect.Location = new System.Drawing.Point(191, 646);
            this.checkBoxEnglishMultiSelect.Name = "checkBoxEnglishMultiSelect";
            this.checkBoxEnglishMultiSelect.Size = new System.Drawing.Size(96, 16);
            this.checkBoxEnglishMultiSelect.TabIndex = 106;
            this.checkBoxEnglishMultiSelect.Text = "英文多元分词";
            this.checkBoxEnglishMultiSelect.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(73, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 107;
            this.button1.Text = "批量处理";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(118, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 108;
            this.button2.Text = "Next";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(212, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(109, 23);
            this.button3.TabIndex = 109;
            this.button3.Text = "保存当前文档分词";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(341, 16);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 110;
            this.button4.Text = "保存所有分词";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(25, 16);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 111;
            this.button5.Text = "加载文档";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(76, 353);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(548, 50);
            this.groupBox1.TabIndex = 112;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "手工处理";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(440, 16);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(86, 23);
            this.button6.TabIndex = 112;
            this.button6.Text = "清理手工记录";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(757, 353);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 50);
            this.groupBox2.TabIndex = 113;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "批量处理";
            // 
            // textBoxSource
            // 
            this.textBoxSource.Location = new System.Drawing.Point(1, 25);
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.Size = new System.Drawing.Size(1171, 325);
            this.textBoxSource.TabIndex = 114;
            this.textBoxSource.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(43, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 12);
            this.label9.TabIndex = 115;
            this.label9.Text = "0";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(176, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 116;
            this.label10.Text = "标题：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(216, 7);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 12);
            this.label12.TabIndex = 117;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1020, 368);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 118;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 699);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxSource);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxEnglishMultiSelect);
            this.Controls.Add(this.checkBoxCustomRule);
            this.Controls.Add(this.checkBoxWildcardSegment);
            this.Controls.Add(this.checkBoxWildcard);
            this.Controls.Add(this.checkBoxSynonymOutput);
            this.Controls.Add(this.checkBoxEnglishSegment);
            this.Controls.Add(this.checkBoxShowTimeOnly);
            this.Controls.Add(this.checkBoxIgnoreCapital);
            this.Controls.Add(this.labelFilterNumericLength);
            this.Controls.Add(this.numericUpDownFilterNumericLength);
            this.Controls.Add(this.checkBoxFilterNumeric);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numericUpDownFilterEnglishLength);
            this.Controls.Add(this.checkBoxFilterEnglish);
            this.Controls.Add(this.checkBoxUnknownWord);
            this.Controls.Add(this.checkBoxST);
            this.Controls.Add(this.checkBoxTraditionalChs);
            this.Controls.Add(this.checkBoxForceSingleWord);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.numericUpDownRedundancy);
            this.Controls.Add(this.checkBoxMultiSelect);
            this.Controls.Add(this.buttonSaveConfig);
            this.Controls.Add(this.checkBoxFreqFirst);
            this.Controls.Add(this.checkBoxDisplayPosition);
            this.Controls.Add(this.checkBoxFilterStopWords);
            this.Controls.Add(this.checkBoxMatchName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelSrcLength);
            this.Controls.Add(this.labelRegRate);
            this.Controls.Add(this.labelSegTime);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonSegment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSegwords);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "模板文档处理";
            this.Load += new System.EventHandler(this.FormDemo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRedundancy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterEnglishLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterNumericLength)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numericUpDownRedundancy;
        private System.Windows.Forms.CheckBox checkBoxMultiSelect;
        private System.Windows.Forms.Button buttonSaveConfig;
        private System.Windows.Forms.CheckBox checkBoxFreqFirst;
        private System.Windows.Forms.CheckBox checkBoxDisplayPosition;
        private System.Windows.Forms.CheckBox checkBoxFilterStopWords;
        private System.Windows.Forms.CheckBox checkBoxMatchName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelSrcLength;
        private System.Windows.Forms.Label labelRegRate;
        private System.Windows.Forms.Label labelSegTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSegment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSegwords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxForceSingleWord;
        private System.Windows.Forms.CheckBox checkBoxTraditionalChs;
        private System.Windows.Forms.CheckBox checkBoxST;
        private System.Windows.Forms.CheckBox checkBoxUnknownWord;
        private System.Windows.Forms.CheckBox checkBoxFilterEnglish;
        private System.Windows.Forms.NumericUpDown numericUpDownFilterEnglishLength;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelFilterNumericLength;
        private System.Windows.Forms.NumericUpDown numericUpDownFilterNumericLength;
        private System.Windows.Forms.CheckBox checkBoxFilterNumeric;
        private System.Windows.Forms.CheckBox checkBoxIgnoreCapital;
        private System.Windows.Forms.CheckBox checkBoxShowTimeOnly;
        private System.Windows.Forms.CheckBox checkBoxEnglishSegment;
        private System.Windows.Forms.CheckBox checkBoxSynonymOutput;
        private System.Windows.Forms.CheckBox checkBoxWildcard;
        private System.Windows.Forms.CheckBox checkBoxWildcardSegment;
        private System.Windows.Forms.CheckBox checkBoxCustomRule;
        private System.Windows.Forms.CheckBox checkBoxEnglishMultiSelect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog fbdFileFold;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox textBoxSource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}

