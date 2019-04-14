using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using org.pdfbox.pdmodel;
using org.pdfbox.util;


using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
//using iText.Kernel.Pdf.Canvas.Parser.Data;


namespace Segment.SegmentDemo
{
    public partial class frmMain : Form
    {
        string DocPath = ConfigurationManager.AppSettings["DocPath"];
        string DocRange = ConfigurationManager.AppSettings["DocRange"];
        List<FileInfo> listDoc = new List<FileInfo>();
        List<string> StopWords = new List<string>(ConfigurationManager.AppSettings["StopWords"].Split(','));

        //文档索引
        int DocIndex = 0;
        //文档ID
        int DocID = 0;
        String _InitSource = "";
        Segment segment = new Segment();

        private Match.MatchOptions _Options;
        private Match.MatchParameter _Parameters;


        public frmMain()
        {
            
            InitializeComponent();
        }





        Dictionary<string, decimal> AffectDic = new Dictionary<string, decimal>();
        public void Read(string path)
        {
            string wpath = @"D:\KEY.txt";
            if (!File.Exists(wpath))
            {
                FileInfo myfile = new FileInfo(wpath);
                FileStream fs = myfile.Create();
                fs.Close();
            }
            StreamWriter sw = File.AppendText(wpath);
            //sw.WriteLine(errorMsg);

            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line != "")
                {
                    string[] curValue = line.Split(' ');

                    //Convert.ToDecimal(Decimal.Parse(curValue[1], System.Globalization.NumberStyles.Float));
                    AffectDic.Add(curValue[0], Convert.ToDecimal(decimal.Parse(curValue[1], System.Globalization.NumberStyles.Float)));
                    sw.WriteLine(curValue[0]);
                    // Console.WriteLine(line.ToString());
                }
            }
        }

        private void FormDemo_Load(object sender, EventArgs e)
        {

            //Read(@"C:\Users\Jacky\Downloads\BosonNLP_sentiment_score\BosonNLP_sentiment_score.txt");
            //string isExits = AffectDic["上访"].ToString();
            textBoxSource.Text = _InitSource;
            segment.Init();

            Match.MatchOptions options = Setting.SegmentSettings.Config.MatchOptions;
            checkBoxFreqFirst.Checked = options.FrequencyFirst;
            checkBoxFilterStopWords.Checked = options.FilterStopWords;
            checkBoxMatchName.Checked = options.ChineseNameIdentify;
            checkBoxMultiSelect.Checked = options.MultiDimensionality;
            checkBoxEnglishMultiSelect.Checked = options.EnglishMultiDimensionality;
            checkBoxForceSingleWord.Checked = options.ForceSingleWord;
            checkBoxTraditionalChs.Checked = options.TraditionalChineseEnabled;
            checkBoxST.Checked = options.OutputSimplifiedTraditional;
            checkBoxUnknownWord.Checked = options.UnknownWordIdentify;
            checkBoxFilterEnglish.Checked = options.FilterEnglish;
            checkBoxFilterNumeric.Checked = options.FilterNumeric;
            checkBoxIgnoreCapital.Checked = options.IgnoreCapital;
            checkBoxEnglishSegment.Checked = options.EnglishSegment;
            checkBoxSynonymOutput.Checked = options.SynonymOutput;
            checkBoxWildcard.Checked = options.WildcardOutput;
            checkBoxWildcardSegment.Checked = options.WildcardSegment;
            checkBoxCustomRule.Checked = options.CustomRule;

            if (checkBoxMultiSelect.Checked)
            {
                checkBoxDisplayPosition.Checked = true;
            }

            Match.MatchParameter parameters = Setting.SegmentSettings.Config.Parameters;

            numericUpDownRedundancy.Value = parameters.Redundancy;
            numericUpDownFilterEnglishLength.Value = parameters.FilterEnglishLength;
            numericUpDownFilterNumericLength.Value = parameters.FilterNumericLength;

            this.button1.Enabled = true;
            this.button2.Enabled = false;
            this.button3.Enabled = false;
            //str = Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);

        }

        private void DisplaySegment()
        {
            DisplaySegment(false);
        }

        private void DisplaySegment(bool showPosition)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //Segment segment = new Segment();
            ICollection<WordInfo> words = segment.DoSegment(textBoxSource.Text, _Options, _Parameters);

            watch.Stop();

            labelSrcLength.Text = textBoxSource.Text.Length.ToString();

            labelSegTime.Text = watch.Elapsed.ToString();
            if (watch.ElapsedMilliseconds == 0)
            {
                labelRegRate.Text = "无穷大";
            }
            else
            {
                labelRegRate.Text = ((double)(textBoxSource.Text.Length / watch.ElapsedMilliseconds) * 1000).ToString();
            }


            if (checkBoxShowTimeOnly.Checked)
            {
                return;
            }

            StringBuilder wordsString = new StringBuilder();
            foreach (WordInfo wordInfo in words)
            {
                if (wordInfo == null)
                {
                    continue;
                }

                if (showPosition)
                {

                    wordsString.AppendFormat("{0}({1},{2})/", wordInfo.Word, wordInfo.Position, wordInfo.Rank);
                    //if (_Options.MultiDimensionality)
                    //{
                    //}
                    //else
                    //{
                    //    wordsString.AppendFormat("{0}({1})/", wordInfo.Word, wordInfo.Position);
                    //}
                }
                else
                {
                    wordsString.AppendFormat("{0}/", wordInfo.Word);
                }
            }

            textBoxSegwords.Text = wordsString.ToString();


        }

        private void DisplaySegmentAndPostion()
        {
            DisplaySegment(true);
        }

        private void UpdateSettings()
        {
            _Options.FrequencyFirst = checkBoxFreqFirst.Checked;
            _Options.FilterStopWords = checkBoxFilterStopWords.Checked;
            _Options.ChineseNameIdentify = checkBoxMatchName.Checked;
            _Options.MultiDimensionality = checkBoxMultiSelect.Checked;
            _Options.EnglishMultiDimensionality = checkBoxEnglishMultiSelect.Checked;
            _Options.ForceSingleWord = checkBoxForceSingleWord.Checked;
            _Options.TraditionalChineseEnabled = checkBoxTraditionalChs.Checked;
            _Options.OutputSimplifiedTraditional = checkBoxST.Checked;
            _Options.UnknownWordIdentify = checkBoxUnknownWord.Checked;
            _Options.FilterEnglish = checkBoxFilterEnglish.Checked;
            _Options.FilterNumeric = checkBoxFilterNumeric.Checked;
            _Options.IgnoreCapital = checkBoxIgnoreCapital.Checked;
            _Options.EnglishSegment = checkBoxEnglishSegment.Checked;
            _Options.SynonymOutput = checkBoxSynonymOutput.Checked;
            _Options.WildcardOutput = checkBoxWildcard.Checked;
            _Options.WildcardSegment = checkBoxWildcardSegment.Checked;
            _Options.CustomRule = checkBoxCustomRule.Checked;

            _Parameters.Redundancy = (int)numericUpDownRedundancy.Value;
            _Parameters.FilterEnglishLength = (int)numericUpDownFilterEnglishLength.Value;
            _Parameters.FilterNumericLength = (int)numericUpDownFilterNumericLength.Value;

        }

        private void buttonSegment_Click(object sender, EventArgs e)
        {
            _Options = Setting.SegmentSettings.Config.MatchOptions.Clone();
            _Parameters = Setting.SegmentSettings.Config.Parameters.Clone();

            UpdateSettings();

            if (checkBoxDisplayPosition.Checked)
            {
                DisplaySegmentAndPostion();
            }
            else
            {
                DisplaySegment();
            }

            //this.button2.Enabled = true;
            this.button3.Enabled = true;
            this.button3.Focus();
        }

        private void buttonSaveConfig_Click(object sender, EventArgs e)
        {
            _Options = Setting.SegmentSettings.Config.MatchOptions;
            _Parameters = Setting.SegmentSettings.Config.Parameters;

            UpdateSettings();

            Setting.SegmentSettings.Save("HBcomm.xml");
        }
        private void GetMainDocument()
        {
            //fbdFileFold.ShowDialog();
            //string path = fbdFileFold.SelectedPath;

            string path = @"D:\WorkBackUp\搜索引擎\doc";
            if (!string.IsNullOrEmpty(path))
            {
                FindFoldersAndFiles(path);
                int i = 0;
                foreach (FileInfo obj in listDoc)
                {
                    if (obj.FullName.ToLower().EndsWith(".pdf"))
                    {                       
                        AllwordsID[i] = obj.FullName;
                        i++;
                    }
                }
            }
        }
        //加载文件所有编号
        Dictionary<int, string> AllwordsID = new Dictionary<int, string>();
        //加载文件字典
        Dictionary<int, string[]> Allwords = new Dictionary<int, string[]>();
        Dictionary<string, WordsTimes> AllWordsDic = new Dictionary<string, WordsTimes>();
        Dictionary<string, WordsTimes> CurWordsDic = new Dictionary<string, WordsTimes>();


        private void button1_Click(object sender, EventArgs e)
        {
            _Options = Setting.SegmentSettings.Config.MatchOptions;
            _Parameters = Setting.SegmentSettings.Config.Parameters;

            UpdateSettings();

            Segment segment = new Segment();


            //fbdFileFold.ShowDialog();
            //string path = fbdFileFold.SelectedPath;
            //if (!string.IsNullOrEmpty(path))
            //{
            //FindFoldersAndFiles(path);

            GetDocData();

            try
            {
                //文档编号
                //int intDocIndex = 0;
                //词编号
                int intWordsIndex = 0;
                for (int i = 0; i < Allwords.Count; i++)
                {
                    //intDocIndex = i + 1;
                    int documentID = i + 1;
                    string Path = Allwords[documentID][0];

                    if (Path.ToLower().EndsWith(".doc"))
                    {
                        //intDocIndex = intDocIndex + 1;


                        //当前文档所含词
                        CurWordsDic = new Dictionary<string, WordsTimes>();

                        string strcontent = pdf2itxt(Path);// pdf2itxt(Path);
                        //textBoxSource.Text = strcontent;

                        ICollection<WordInfo> words = segment.DoSegment(strcontent, _Options, _Parameters);

                        StringBuilder wordsString = new StringBuilder();
                        foreach (WordInfo wordInfo in words)
                        {
                            if (wordInfo == null)
                            {
                                continue;
                            }
                            if (!AllWordsDic.ContainsKey(wordInfo.Word))
                            {
                                WordsTimes wt = new WordsTimes();
                                intWordsIndex = intWordsIndex + 1;
                                wt.WordsID = intWordsIndex;
                                wt.Words = wordInfo.Word;
                                wt.Times = 1;
                                AllWordsDic.Add(wordInfo.Word, wt);

                            }
                            else
                            {
                                WordsTimes wt = AllWordsDic[wordInfo.Word];
                                wt.Times = wt.Times + 1;
                                //intWordsIndex = wt.WordsID;
                                AllWordsDic[wordInfo.Word] = wt;

                            }

                            if (!CurWordsDic.ContainsKey(wordInfo.Word))
                            {
                                WordsTimes wt = new WordsTimes();

                                wt.WordsID = AllWordsDic[wordInfo.Word].WordsID;
                                wt.Words = wordInfo.Word;
                                wt.Times = 1;
                                CurWordsDic.Add(wordInfo.Word, wt);

                            }
                            else
                            {
                                WordsTimes wt = CurWordsDic[wordInfo.Word];
                                wt.Times = wt.Times + 1;
                                CurWordsDic[wordInfo.Word] = wt;

                            }

                            //wordsString.AppendFormat("{0}/", wordInfo.Word);

                        }

                        //保存当前文档所含词
                        //foreach (string str in CurWordsDic.Keys)
                        //{
                        //    //oracleAccess.TBDOCUMENTTERM dt = new oracleAccess.TBDOCUMENTTERM();
                        //    //dt.TERMID = CurWordsDic[str].WordsID;
                        //    //dt.DOCUMENTID = documentID;
                        //    //dt.TFVALUE = CurWordsDic[str].Times;
                        //    //dt.Add();

                        //}
                        CurWordsDic.Clear();

                        //textBoxSegwords.Text = wordsString.ToString();
                    }


                }
                ////保存所有文档域词
                //foreach (string strtemp in AllWordsDic.Keys)
                //{
                //    oracleAccess.TBDOMAINTERM dt = new oracleAccess.TBDOMAINTERM();
                //    dt.TERMID = AllWordsDic[strtemp].WordsID;
                //    dt.TERM = AllWordsDic[strtemp].Words;
                //    dt.DOCNUM = AllWordsDic[strtemp].Times;
                //    dt.Add();

                //}
                AllWordsDic.Clear();
            }
            catch (Exception x)
            {
                //string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                //Logger.WriteError(funName, x);
            }
            //}

            //string aPath = System.Environment.CurrentDirectory + "FenCi.txt";
            //FileStream fs = new FileStream(aPath, FileMode.OpenOrCreate);


            //StreamWriter swWriter = new StreamWriter(fs);

            //foreach (KeyValuePair<int, string> kv in Allwords)
            //{
            //    swWriter.WriteLine(kv.Key + " " + kv.Value);

            //}
            //swWriter.Flush();
            //swWriter.Close();

        }

        public string pdf2txt(string SPath)
        {
            PDDocument doc = PDDocument.load(SPath);
            PDFTextStripper pdfStripper = new PDFTextStripper();
            string text = pdfStripper.getText(doc).Replace(" ", "");
            StreamWriter swPdfChange = new StreamWriter(@"D:\WorkBackUp\搜索引擎\doc\201949-2.txt", false, Encoding.GetEncoding("gb2312"));
            swPdfChange.Write(text);
            swPdfChange.Close();
            doc.close();
            return text;
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
            StreamWriter swPdfChange = new StreamWriter(@"D:\WorkBackUp\搜索引擎\doc\201949-1.txt", false, Encoding.GetEncoding("gb2312"));
            swPdfChange.Write(text.ToString());
            swPdfChange.Close();
            return text.ToString();

        }

        public string GetWordBody(string SPath)
        {
            string str = "";
            try
            {
                //Aspose.Words.Document doc = new Aspose.Words.Document(SPath);
                //Aspose.Words.NodeCollection nc = doc.GetChildNodes(Aspose.Words.NodeType.Body, true);
                //foreach (Aspose.Words.Node node in nc)
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
                //    str = str.Replace("\'", " ").Replace("\"", " ").Replace("\\", " ").Replace("FORMTEXT", "").Replace("formtext", "");
                //}

            }
            catch (Exception ex)
            {
                //;
            }
            return str;
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
                    //frmBatchInsert.Progress = 0;
                    FileInfo fi = new FileInfo(fileName);
                    listDoc.Add(fi);
                    //frmBatchInsert.Progress = 100;
                }
                catch (Exception ex)
                {

                    //log.LogHelper.LogError(ex.Message, "IIPS.SearchServer.Common.CommonClass.FindFoldersAndFiles", ex);
                }

            }
            //遍历目标文件夹的所有文件夹
            foreach (string directory in Directory.GetDirectories(path))
            {
                FindFoldersAndFiles(directory);
            }
            return listDoc;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DocIndex >= Allwords.Count)
            {
                DocIndex = 0;
            }
            DocID = DocIndex;
            string content = pdf2itxt(Allwords[DocID][0]).Replace(" ", "");
            this.textBoxSource.Text = content;
            this.label12.Text = Allwords[DocID][1];
            label9.Text = DocID.ToString();
            label9.ForeColor = Color.Red;
            DocIndex = DocIndex + 1;
            this.button2.Enabled = false;
            this.button3.Enabled = false;

            buttonSegment_Click(null, null);
        }


        //词的编号
        int Index = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            //手工处理标识
            int HANDLETYPE = 1;
            //CurWordsDic.Clear();
            if (string.IsNullOrWhiteSpace(this.textBoxSegwords.Text.Trim()))
            {
                MessageBox.Show("未分词！");
                this.textBoxSegwords.Focus();
                return;
            }

            string[] term = this.textBoxSegwords.Text.Split('/');
            //文档所含词数
            int wordNum = 0;
            foreach (string str in term)
            {

                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (!StopWords.Contains(str))
                    {
                        //词ID
                        int strID = 0;
                        //oracleAccess.TBDOMAINTERM mat = new oracleAccess.TBDOMAINTERM();
                        //if (!mat.TermExists(str, HANDLETYPE))
                        //{
                        //    //DOMAAINTERM记录数
                        //    int termID = mat.GetMAXID(HANDLETYPE);
                        //    oracleAccess.TBDOMAINTERM mt = new oracleAccess.TBDOMAINTERM();
                        //    mt.TERMID = termID + 1;
                        //    mt.TERM = str;
                        //    mt.DOCNUM = 1;
                        //    mt.HANDLETYPE = 1;
                        //    mt.Add();
                        //    strID = (int)mt.TERMID;

                        //}
                        //else
                        //{
                        //    oracleAccess.TBDOMAINTERM MMTT = new oracleAccess.TBDOMAINTERM();
                        //    MMTT.GetModel(str, HANDLETYPE);


                        //    oracleAccess.TBDOMAINTERM mtt = new oracleAccess.TBDOMAINTERM();
                        //    mtt.TERMID = MMTT.TERMID;
                        //    mtt.TERM = str;
                        //    mtt.DOCNUM = MMTT.DOCNUM + 1;
                        //    mtt.HANDLETYPE = 1;
                        //    mtt.Update();
                        //    strID = (int)mtt.TERMID;
                        //}

                        //oracleAccess.TBDOCUMENTTERM dot = new oracleAccess.TBDOCUMENTTERM();
                        //if (!dot.Exists(DocID, strID, HANDLETYPE))
                        //{
                        //    oracleAccess.TBDOCUMENTTERM dt = new oracleAccess.TBDOCUMENTTERM();
                        //    dt.DOCUMENTID = DocID;
                        //    dt.TERMID = strID;
                        //    dt.TFVALUE = 1;
                        //    dt.HANDLETYPE = 1;
                        //    dt.Add();
                        //    wordNum = wordNum + 1;
                        //}
                        //else
                        //{
                        //    oracleAccess.TBDOCUMENTTERM ddtt = new oracleAccess.TBDOCUMENTTERM();
                        //    ddtt.GetModel(DocID, strID, HANDLETYPE);

                        //    oracleAccess.TBDOCUMENTTERM dt = new oracleAccess.TBDOCUMENTTERM();
                        //    dt.DOCUMENTID = ddtt.DOCUMENTID;
                        //    dt.TERMID = ddtt.TERMID;
                        //    dt.TFVALUE = ddtt.TFVALUE + 1;
                        //    dt.HANDLETYPE = 1;
                        //    dt.Update();
                        //}
                        //segment.WordDictionary.InsertWord(str, 3, 0);
                        //Segment._WordDictionary.InsertWord(str, 3, 0);
                    }
                    else
                    {

                    }
                }

            }

            //oracleAccess.TBDOMAINDOCUMENT md = new oracleAccess.TBDOMAINDOCUMENT();
            //if (!md.UpdateTermNum(DocID, wordNum))
            //{
            //    MessageBox.Show("更新TBDOMAINDOCUMENT表的手工处理词数失败！");
            //}
            segment.SaveDic();
            //Segment.SaveDic();

            label9.ForeColor = Color.Green;
            this.button2.Enabled = true;
            this.button2.Focus();
            this.button3.Enabled = false;

            //保存当前文档所含词
            //foreach (string str in CurWordsDic.Keys)
            //{
            //    oracleAccess.TBDOCUMENTTERM dt = new oracleAccess.TBDOCUMENTTERM();
            //    dt.TERMINDEX = CurWordsDic[str].WordsID;
            //    dt.DOCUMENTID = DocIndex;
            //    dt.TFVALUE = CurWordsDic[str].Times;
            //    dt.Add();

            //}
            //CurWordsDic.Clear();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            #region 获取文件title
            //Dictionary<string, string> titleDic = new Dictionary<string, string>();
            //StreamReader sr = new StreamReader(@"C:\Users\Jacky\Desktop\Log.txt", Encoding.Default);
            //string line;
            //while ((line = sr.ReadLine()) != null)
            //{
            //    string[] arr = line.Split(',');
            //    if (!titleDic.ContainsKey(arr[1]))
            //    {
            //        titleDic.Add(arr[1], arr[0]);
            //    }
            //}
            //oracleAccess.TBDOMAINDOCUMENT md = new oracleAccess.TBDOMAINDOCUMENT();

            //DataSet ds = md.GetList("");
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    string paht = ds.Tables[0].Rows[i]["DOCUMENTPATH"].ToString();
            //    paht = paht.Remove(0, 3);
            //    paht=paht.Replace('\\','/');
            //    oracleAccess.TBDOMAINDOCUMENT mt = new oracleAccess.TBDOMAINDOCUMENT();
            //    mt.DOCUMENTID = int.Parse(ds.Tables[0].Rows[i]["DOCUMENTID"].ToString());
            //    mt.DOCUMENTTITLE = titleDic[paht];
            //    mt.UpdateTitle();
            //}
            #endregion

            //保存所有文档域词
            //foreach (string strtemp in AllWordsDic.Keys)
            //{
            //    oracleAccess.TBDOMAINTERM dt = new oracleAccess.TBDOMAINTERM();
            //    dt.TERMID = AllWordsDic[strtemp].WordsID;
            //    dt.TERM = AllWordsDic[strtemp].Words;
            //    dt.DOCNUM = CurWordsDic[strtemp].Times;
            //    dt.Add();

            //}
            AllWordsDic.Clear();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            GetMainDocument();
            GetDocData();
            // DocIndex = DocIndex + 1;
            DocID = DocIndex;

            string context = pdf2itxt(AllwordsID[DocID]).Replace(" ", "");//GetWordBody
            this.textBoxSource.Text = context;
            this.label12.Text = "?";// Allwords[DocID][1];
            label9.Text = DocID.ToString();
            label9.ForeColor = Color.Red;
            DocIndex = DocIndex + 1;

            this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.button3.Enabled = false;


        }



        private void GetDocData()
        {
            //oracleAccess.TBDOMAINDOCUMENT md = new oracleAccess.TBDOMAINDOCUMENT();
            //DataSet ds = md.GetList(DocRange);
            //DataTable dt = ds.Tables[0];
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    int key = int.Parse(dt.Rows[i]["DOCUMENTID"].ToString());
            //    string path = DocPath + dt.Rows[i]["DOCUMENTPATH"].ToString();
            //    string title = dt.Rows[i]["DOCUMENTTITLE"].ToString();
            //    string[] value = new string[] { path, title };
            //    Allwords.Add(key, value);
            //    AllwordsID.Add(key);
            //}
            //this.button5.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //int HANDLETYPE = 1;
            //oracleAccess.TBDOCUMENTTERM dt = new oracleAccess.TBDOCUMENTTERM();
            //dt.Delete(HANDLETYPE);

            //oracleAccess.TBDOMAINTERM TDT = new oracleAccess.TBDOMAINTERM();
            //TDT.Delete(HANDLETYPE);

            //oracleAccess.TBDOMAINDOCUMENT dd = new oracleAccess.TBDOMAINDOCUMENT();
            //dd.ClearHandleTermNum(HANDLETYPE);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //初始化分词工具
            _Options = Setting.SegmentSettings.Config.MatchOptions;
            _Parameters = Setting.SegmentSettings.Config.Parameters;

            UpdateSettings();
            Segment segment = new Segment();
            /////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<string, Words> DicAllWords = new Dictionary<string, Words>();    //所有分词及出现频率////////////////////////////////////////
            Dictionary<int, string> DicDocPath = new Dictionary<int, string>();         //所有文件路径///////////////////////////////////////////////
            Dictionary<int, string> DicAdv = new Dictionary<int, string>();             //所有文件拟办//////////////////////////////////////////////////
            Dictionary<int, string> DicTitle = new Dictionary<int, string>();           //所有文件标题/////////////////////////////////////////////////
            Dictionary<int, string> DicSeg = new Dictionary<int, string>();             //所有文件分词///////////////////////////////////////////////////
                                                                                        //Dictionary<stringint, > DicWords = new Dictionary<string,int>();           //所有分词/////////////////////////////////////////////////
                                                                                        //List<DocAdvSegInfor> list = new List<DocAdvSegInfor>();

            Dictionary<string, int> DicBefAftWord = new Dictionary<string, int>();
            /////初始化结果记录文件/////////////////////////////////////////////////////////
            if (!File.Exists(@"D:\文件\UUU\result\re.txt"))
            {
                FileInfo FI = new FileInfo(@"D:\文件\UUU\result\re.txt");
                FileStream FS = FI.Create();
                FS.Close();
            }
            StreamWriter sw = File.AppendText(@"D:\文件\UUU\result\re.txt");

            ///初始化分词记录文件//////////////////////////////////////////////////////////
            if (!File.Exists(@"D:\文件\UUU\result\se.txt"))
            {
                FileInfo FI = new FileInfo(@"D:\文件\UUU\result\se.txt");
                FileStream FS = FI.Create();
                FS.Close();
            }
            StreamWriter swse = File.AppendText(@"D:\文件\UUU\result\se.txt");

            ///初始化词前后记录文件//////////////////////////////////////////////////
            if (!File.Exists(@"D:\文件\UUU\result\we.txt"))
            {
                FileInfo FI = new FileInfo(@"D:\文件\UUU\result\we.txt");
                FileStream FS = FI.Create();
                FS.Close();
            }
            StreamWriter swwe = File.AppendText(@"D:\文件\UUU\result\we.txt");
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ///读取所有文件拟办信息///
            StreamReader sr = new StreamReader(@"D:\文件\UUU\log\log.txt", Encoding.UTF8);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] adviseList = line.ToString().Split('_');
                if (adviseList.Length == 2)
                {
                    int DocNum = int.Parse(adviseList[0].ToString());
                    string strAdv = adviseList[1].ToString();

                    if (!DicAdv.Keys.Contains(DocNum))
                    {
                        DicAdv.Add(DocNum, strAdv);
                    }
                    else
                    {
                        if (strAdv != DicAdv[DocNum])
                        {

                        }
                    }

                    //sw.WriteLine(DocNum.ToString() + "_" + strAdv);
                    //sw.Flush();
                }
                else
                {

                }
            }


            int itemIndex = 0;//分词编号

            ////////////////////读取所有文件信息////////////////////////////////////////////
            string[] arrAllFiles = Directory.GetFiles(@"D:\文件\UUU\");
            foreach (string str in arrAllFiles)
            {
                FileInfo fi = new FileInfo(str);
                if (fi.Extension == ".doc" || fi.Extension == ".docx")
                {
                    string[] titles = fi.Name.Split('_');
                    int NumDOC = int.Parse(titles[0]);
                    string NameDoc = "";

                    for (int i = 1; i < titles.Length; i++)
                    {
                        NameDoc = NameDoc + titles[i] + "_";
                    }

                    if (NameDoc.EndsWith("_"))
                    {
                        NameDoc = NameDoc.TrimEnd('_');
                    }

                    DicTitle.Add(NumDOC, NameDoc);  //保存所有文件标题
                    DicDocPath.Add(NumDOC, str);    //保存所有文件路径

                    string strcontent = pdf2itxt(str).Replace(" ", "");

                    if (string.IsNullOrWhiteSpace(strcontent))
                    {
                        File.Copy(str, @"D:\E\" + NameDoc);
                    }
                    //textBoxSource.Text = strcontent;

                    ICollection<WordInfo> words = segment.DoSegment(strcontent, _Options, _Parameters);
                    StringBuilder wordsString = new StringBuilder();
                    foreach (WordInfo wordInfo in words)
                    {
                        if (wordInfo == null)
                        {
                            continue;
                        }


                        wordsString.Append("/" + wordInfo.Word);

                        if (DicAllWords.Keys.Contains(wordInfo.Word))
                        {
                            Words w = DicAllWords[wordInfo.Word];
                            w.Count = w.Count + 1;
                            DicAllWords[wordInfo.Word] = w;     //更新分词出现频率结果
                        }
                        else
                        {
                            Words w = new Words();
                            w.Word = wordInfo.Word;
                            w.Count = 1;
                            w.Num = itemIndex;
                            DicAllWords.Add(wordInfo.Word, w);  //保存分词结果                            

                            itemIndex = itemIndex + 1;
                        }

                    }

                    DicSeg.Add(NumDOC, wordsString.ToString()); //保存所有文件的分词结果

                }
                else //if (fi.Extension == ".tif" || fi.Extension == ".tiff")
                {

                }

            }

            //DataTable segMutir = new DataTable("词的前后关系");






            ////对分词的出现频率排序
            Dictionary<string, Words> dic1_SortedByKey = DicAllWords.OrderBy(o => o.Value.Count).ThenByDescending(o => o.Value.Count).ToDictionary(p => p.Key, o => o.Value);

            foreach (KeyValuePair<string, Words> kvp in dic1_SortedByKey)
            {

                string word = kvp.Value.Word;
                string adv = kvp.Value.Count.ToString();

                //segMutir.Columns.Add(kvp.Value.Word);

                swse.WriteLine(string.Format("{0,-20}{1,-15}", word, adv)); //记录分词频率
                swse.Flush();
            }
            swse.Close();


            // int[,] mutir = new int[10000, 10000];
            int docCount = 0;

            foreach (KeyValuePair<int, string> kvp in DicSeg)
            {
                if (!string.IsNullOrEmpty(kvp.Value) && kvp.Value != "/")
                {
                    string[] segments = kvp.Value.TrimStart('/').Split('/');
                    if (segments.Length > 0)
                    {
                        docCount = docCount + 1;
                        string befWord = segments[0];
                        Words bef = DicAllWords[befWord];

                        for (int i = 1; i < segments.Length; i++)
                        {
                            string aftWord = segments[i];

                            Words aft = DicAllWords[aftWord];

                            if (!DicBefAftWord.Keys.Contains(bef.Num + "X" + aft.Num))
                            {
                                DicBefAftWord[bef.Num + "X" + aft.Num] = 1;
                                //segMutir.Rows[bef.Num][aft.Num] = 1;
                            }
                            else
                            {
                                DicBefAftWord[bef.Num + "X" + aft.Num] = DicBefAftWord[bef.Num + "X" + aft.Num] + 1;
                            }

                            befWord = aftWord;
                            bef = aft;
                        }
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }

            Dictionary<string, int> DicBefAftWord_SortedByKey = DicBefAftWord.Where(o => o.Value > 1).OrderBy(o => o.Value).ThenByDescending(o => o.Value).ToDictionary(p => p.Key, o => o.Value);
            foreach (KeyValuePair<string, int> kvp in DicBefAftWord_SortedByKey)
            {

                string word = kvp.Key;
                string[] wordList = word.Split('X');

                int befKey = int.Parse(wordList[0]);
                int aftKey = int.Parse(wordList[1]);

                string befWord = "";
                Dictionary<string, Words> list = DicAllWords.Where(o => o.Value.Num == befKey).ToDictionary(p => p.Key, p => p.Value);
                if (list.Count == 1)
                {
                    foreach (KeyValuePair<string, Words> kwp in list)
                    {
                        befWord = kwp.Value.Word;
                    }
                }
                else
                {

                }


                string aftWord = "";
                Dictionary<string, Words> list1 = DicAllWords.Where(o => o.Value.Num == aftKey).ToDictionary(p => p.Key, p => p.Value);
                if (list.Count == 1)
                {
                    foreach (KeyValuePair<string, Words> kwp1 in list1)
                    {
                        aftWord = kwp1.Value.Word;
                    }
                }
                else
                {

                }

                //Dictionary < string, Words> select =
                int adv = kvp.Value;

                //segMutir.Columns.Add(kvp.Value.Word);

                swwe.WriteLine(string.Format("{0,-40}{1,-15}", befWord + ":" + aftWord, adv)); //记录分词频率
                swwe.Flush();
            }
            swwe.Close();

            //foreach (KeyValuePair<int, string> kvp in DicBefAftWord)
            //{
            //    int index = kvp.Key;
            //    string path = kvp.Value;
            //    if (DicSeg.Keys.Contains(index) && DicTitle.Keys.Contains(index) && DicSeg.Keys.Contains(index))
            //    {
            //        string adv = DicAdv[index];
            //        string title = DicTitle[index];
            //        string seg = DicSeg[index];

            //        sw.WriteLine(index + "|" + title + "|" + path + "|" + adv + "|" + seg); //记录所有文件的编号、标题、路径、拟办、分词
            //        sw.Flush();
            //    }
            //}
            //sw.Close();


            //Dictionary<string, Words> dic2_SortedByKey = DicAllWords.OrderBy(o => o.Value.Num).ThenByDescending(o => o.Value.Num).ToDictionary(p => p.Key, o => o.Value);

            //foreach (KeyValuePair<string, Words> kvp in dic2_SortedByKey)
            //{

            //    segMutir.Columns.Add(kvp.Value.Word.ToString(), typeof(int));

            //}

            //foreach (KeyValuePair<string, Words> kvp in dic2_SortedByKey)
            //{
            //    DataRow dr = segMutir.NewRow();
            //    foreach (KeyValuePair<string, Words> kvp1 in dic2_SortedByKey)
            //    {
            //        dr[kvp1.Value.Num] = 0;
            //    }
            //        segMutir.Rows.Add(dr);

            //}


            //foreach (KeyValuePair<int, string> kvp in DicSeg)
            //{
            //    string[] segments = kvp.Value.Split('/');
            //    if (segments.Length > 0)
            //    {
            //        string befWord = segments[0];

            //        Words bef = DicAllWords[befWord];                    


            //        for (int i=1;i<segments.Length;i++)
            //        {
            //            string aftWord = segments[i];

            //            Words aft = DicAllWords[befWord];

            //            if (string.IsNullOrWhiteSpace(segMutir.Rows[bef.Num][aft.Num].ToString()))
            //            {
            //                segMutir.Rows[bef.Num][aft.Num] = 1;
            //            }
            //            else
            //            {
            //                segMutir.Rows[bef.Num][aft.Num] = int.Parse(segMutir.Rows[bef.Num][aft.Num].ToString()) + 1;
            //            }

            //        }
            //    }
            //}


            var dt = from n in DicAllWords
                     orderby n.Value.Count ascending
                     select n;

            //矩阵处理 ：http://www.cnblogs.com/twocold/p/5434509.html


            foreach (KeyValuePair<int, string> kvp in DicDocPath)
            {
                int index = kvp.Key;
                string path = kvp.Value;
                if (DicSeg.Keys.Contains(index) && DicTitle.Keys.Contains(index) && DicSeg.Keys.Contains(index))
                {
                    string adv = DicAdv[index];
                    string title = DicTitle[index];
                    string seg = DicSeg[index];

                    sw.WriteLine(index + "|" + title + "|" + path + "|" + adv + "|" + seg); //记录所有文件的编号、标题、路径、拟办、分词
                    sw.Flush();
                }
            }
            sw.Close();




        }

    }


    public class DocAdvSegInfor
    {
        int docNum = 0;
        string adv;
        string seg;
        public int DocNum
        {
            get
            {
                return docNum;
            }

            set
            {
                docNum = value;
            }
        }

        public string Adv
        {
            get
            {
                return adv;
            }

            set
            {
                adv = value;
            }
        }

        public string Seg
        {
            get
            {
                return seg;
            }

            set
            {
                seg = value;
            }
        }


    }

    public class Words
    {
        int num;
        string word;
        int count;

        /// <summary>
        /// 分词
        /// </summary>
        public string Word
        {
            get
            {
                return word;
            }

            set
            {
                word = value;
            }
        }

        /// <summary>
        /// 出现频率
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
            }
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Num
        {
            get
            {
                return num;
            }

            set
            {
                num = value;
            }
        }
    }

    public class WordsTimes
    {

        int wordsID;

        public int WordsID
        {
            get { return wordsID; }
            set { wordsID = value; }
        }

        string words;

        public string Words
        {
            get { return words; }
            set { words = value; }
        }

        int times;

        public int Times
        {
            get { return times; }
            set { times = value; }
        }

    }
}


