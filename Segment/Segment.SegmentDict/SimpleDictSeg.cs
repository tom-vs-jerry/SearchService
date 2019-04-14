using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Reflection;

using Segment.Algorithm;
using Segment.SegmentDict.Rule;

namespace Segment.SegmentDict
{
    /// <summary>
    /// 简单字典分词
    /// </summary>
    public class CSimpleDictSeg
    {
        #region Private fields
        const string CHS_STOP_WORD_FILENAME = "chsstopwords.txt";
        const string ENG_STOP_WORD_FILENAME = "engstopwords.txt";

        IRule[] m_Rules;

        //中文停用词哈希表
        Hashtable m_ChsStopwordTbl = new Hashtable();

        //英文停用词哈希表
        Hashtable m_EngStopwordTbl = new Hashtable();

        CExtractWords m_ExtractWords;
        const string PATTERNS = @"[０-９\d]+\%|[０-９\d]{1,2}月|[０-９\d]{1,2}日|[０-９\d]{1,4}年|" +
            @"[０-９\d]{1,4}-[０-９\d]{1,2}-[０-９\d]{1,2}|" +
            @"[０-９\d]+|[^ａ-ｚＡ-Ｚa-zA-Z0-9０-９\u4e00-\u9fa5]|[ａ-ｚＡ-Ｚa-zA-Z]+|[\u4e00-\u9fa5]+";
        //const string PATTERNS = @"[a-zA-Z]+|\d+|[\u4e00-\u9fa5]+";

        String m_DictPath;

        CPOS m_POS;

        PosBinRule m_PosBinRule;
        MatchName m_MatchNameRule;

        /// <summary>
        /// 字典
        /// </summary>
        DictFile m_Dict;

        /// <summary>
        /// 字典管理
        /// </summary>
        DictMgr m_DictMgr = new DictMgr();


        /// <summary>
        /// 未登录词统计字典
        /// 用于统计未登录词的出现频率和词性。
        /// 目前主要统计未知词性的未登录词和
        /// 未知姓名
        /// </summary>
        DictFile m_UnknownWordsDict;

        /// <summary>
        /// 未登录词字典管理
        /// </summary>
        DictMgr m_UnknownWordsDictMgr = new DictMgr();

        private int m_UnknownWordsThreshold = 100;
        private bool m_FreqFirst = true;
        private bool m_AutoStudy = false;
        private bool m_AutoInsertUnknownWords = false;
        private bool m_MultiSelect = false; //多元分词选项
        private DateTime m_LastSaveTime; //上一次保存字典和统计信息的时间
        private int m_AutoSaveInterval = 24 * 3600; //间隔多少秒自动保存最新的字典和统计信息，AutoStudy = true时有效
        private String m_LogFileName = "KTDictSeg.log";
        private bool m_SingleWordsLoaded = false;
        private int m_Redundancy = 1; //冗余度

        #endregion

        #region Public property
        /// <summary>
        /// 未登录词阈值，当统计超过这个值时，自动将未登录词加入到
        /// 字典中
        /// </summary>
        public int UnknownWordsThreshold
        {
            get
            {
                return m_UnknownWordsThreshold;
            }

            set
            {
                if (value < 1)
                {
                    m_UnknownWordsThreshold = 1;
                }
                else
                {
                    m_UnknownWordsThreshold = value;
                }
            }
        }

        /// <summary>
        /// 自动插入超过统计阈值的未登录词
        /// </summary>
        public bool AutoInsertUnknownWords
        {
            get
            {
                return m_AutoInsertUnknownWords;
            }

            set
            {
                m_AutoInsertUnknownWords = value;
            }
        }


        /// <summary>
        /// 优先判断词频，
        /// 如果一个长的单词由多个短的单词组成，而长的单词词频较低
        /// 则忽略长的单词。
        /// 如 中央酒店的词频比中央和酒店的词频都要低，则忽略中央酒店。
        /// </summary>
        public bool FreqFirst
        {
            get
            {
                return m_FreqFirst;
            }

            set
            {
                m_FreqFirst = value;

                if (m_FreqFirst)
                {
                    m_ExtractWords.SelectByFreqEvent = SelectByFreq;
                }
                else
                {
                    m_ExtractWords.SelectByFreqEvent = null;
                }
            }
        }

        /// <summary>
        /// 自动学习
        /// </summary>
        public bool AutoStudy
        {
            get
            {
                return m_AutoStudy;
            }

            set
            {
                m_AutoStudy = value;
                m_MatchNameRule.AutoStudy = value;
            }
        }

        /// <summary>
        /// 间隔多少秒自动保存最新的字典和统计信息，AutoStudy = true时有效
        /// </summary>
        public int AutoSaveInterval
        {
            get
            {
                return m_AutoSaveInterval;
            }

            set
            {
                if (value <= 1)
                {
                    m_AutoSaveInterval = 1;
                }
                else
                {
                    m_AutoSaveInterval = value;
                }
            }
        }

        /// <summary>
        /// 多元分词选项
        /// </summary>
        public bool MultiSelect
        {
            get
            {
                return m_MultiSelect;
            }

            set
            {
                m_MultiSelect = value;
            }
        }

        /// <summary>
        /// 冗余度
        /// 0 为最低冗余
        /// 1 为中等冗余
        /// 2 为最高冗余
        /// </summary>
        public int Redundancy
        {
            get
            {
                return m_Redundancy;
            }

            set
            {
                m_Redundancy = value;

                if (m_Redundancy < 0)
                {
                    m_Redundancy = 0;
                }
                else if (m_Redundancy > 2)
                {
                    m_Redundancy = 2;
                }
            }
        }

        /// <summary>
        /// 字典文件所在路径
        /// </summary>
        public String DictPath
        {
            get
            {
                return m_DictPath;
            }

            set
            {
                m_DictPath = value;
            }
        }

        /// <summary>
        /// 日志文件名
        /// </summary>
        public String LogFileName
        {
            get
            {
                return m_LogFileName;
            }

            set
            {
                m_LogFileName = value;
            }
        }

        /// <summary>
        /// 词性
        /// </summary>
        public CPOS Pos
        {
            get
            {
                return m_POS;
            }
        }


        #endregion

        #region 配置文件

        private object Convert(String In, Type destType)
        {
            if (destType.Equals(typeof(bool)))
            {
                return System.Convert.ToBoolean(In);
            }
            else if (destType.Equals(typeof(byte)))
            {
                return System.Convert.ToByte(In);
            }
            else if (destType.Equals(typeof(char)))
            {
                return System.Convert.ToChar(In);
            }
            else if (destType.Equals(typeof(DateTime)))
            {
                return System.Convert.ToDateTime(In);
            }
            else if (destType.Equals(typeof(decimal)))
            {
                return System.Convert.ToDecimal(In);
            }
            else if (destType.Equals(typeof(double)))
            {
                return System.Convert.ToDouble(In);
            }
            else if (destType.Equals(typeof(Int16)))
            {
                return System.Convert.ToInt16(In);
            }
            else if (destType.Equals(typeof(Int32)))
            {
                return System.Convert.ToInt32(In);
            }
            else if (destType.Equals(typeof(Int64)))
            {
                return System.Convert.ToInt64(In);
            }
            else if (destType.Equals(typeof(SByte)))
            {
                return System.Convert.ToSByte(In);
            }
            else if (destType.Equals(typeof(Single)))
            {
                return System.Convert.ToSingle(In);
            }
            else if (destType.Equals(typeof(String)))
            {
                return In;
            }
            else if (destType.Equals(typeof(UInt16)))
            {
                return System.Convert.ToUInt16(In);
            }
            else if (destType.Equals(typeof(UInt32)))
            {
                return System.Convert.ToUInt32(In);
            }
            else if (destType.Equals(typeof(UInt64)))
            {
                return System.Convert.ToUInt64(In);
            }
            else
            {
                throw new Exception(String.Format("Unknown type:{0}", destType.Name));
            }
        }


        class CfgItem
        {
            public PropertyInfo Pi;
            public String Comment;

            public CfgItem(PropertyInfo pi, String comment)
            {
                Pi = pi;
                Comment = comment;
            }
        }

        CfgItem[] GetCfgItems()
        {
            CfgItem[] items = new CfgItem[11];
            items[0] = new CfgItem(this.GetType().GetProperty("UnknownWordsThreshold"), "未登录词阈值，当统计超过这个值时，自动将未登录词加入到字典中");
            items[1] = new CfgItem(this.GetType().GetProperty("AutoInsertUnknownWords"), "自动插入超过统计阈值的未登录词");
            items[2] = new CfgItem(this.GetType().GetProperty("FreqFirst"), "优先判断词频，如果一个长的单词由多个短的单词组成，而长的单词词频较低则忽略长的单词。如 中央酒店的词频比中央和酒店的词频都要低，则忽略中央酒店。");
            items[3] = new CfgItem(this.GetType().GetProperty("AutoStudy"), "自动统计姓名前后缀，自动统计未登录词，自动统计词频");
            items[4] = new CfgItem(this.GetType().GetProperty("AutoSaveInterval"), "间隔多少秒自动保存最新的字典和统计信息，AutoStudy = true时有效");
            items[5] = new CfgItem(this.GetType().GetProperty("DictPath"), "字典文件所在路径");
            items[6] = new CfgItem(this.GetType().GetProperty("LogFileName"), "日志文件名");
            items[7] = new CfgItem(this.GetType().GetProperty("MatchName"), "是否匹配汉语人名");
            items[8] = new CfgItem(this.GetType().GetProperty("FilterStopWords"), "是否过滤停用词");
            items[9] = new CfgItem(this.GetType().GetProperty("MultiSelect"), "是否启用多元分词");
            items[10] = new CfgItem(this.GetType().GetProperty("Redundancy"), "冗余度");

            return items;
        }

        private string GetFullPath(string baseDir, string path)
        {
            string currentWorkFolder = Environment.CurrentDirectory;
            Environment.CurrentDirectory = baseDir;
            path = System.IO.Path.GetFullPath(path);
            Environment.CurrentDirectory = currentWorkFolder;
            return path;
        }

        /// <summary>
        /// 从配置文件加载配置
        /// </summary>
        /// <param name="fileName">配置文件名</param>
        public void LoadConfig(String fileName)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            try
            {
                doc.Load(fileName);

                System.Xml.XmlNodeList list = doc.GetElementsByTagName("Item");

                System.Xml.XmlAttribute itemName = null;

                foreach (System.Xml.XmlNode node in list)
                {
                    try
                    {
                        itemName = node.Attributes["Name"];
                        System.Xml.XmlAttribute value = node.Attributes["Value"];
                        if (itemName == null || value == null)
                        {
                            continue;
                        }

                        string strValue;

                        if (itemName.Value.Equals("DictPath", StringComparison.CurrentCultureIgnoreCase) ||
                            itemName.Value.Equals("LogFileName", StringComparison.CurrentCultureIgnoreCase))
                        {
                            strValue = GetFullPath(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(fileName)), value.Value);
                        }
                        else
                        {
                            strValue = value.Value;
                        }

                        PropertyInfo pi = GetType().GetProperty(itemName.Value);
                        pi.SetValue(this, Convert(strValue, pi.PropertyType), null);
                    }
                    catch (Exception e1)
                    {
                        WriteLog(String.Format("Load Item={0} fail, errmsg:{1}",
                            itemName.Value, e1.Message));
                    }
                }
            }
            catch (Exception e)
            {
                WriteLog(String.Format("Load config fail, errmsg:{0}", e.Message));
            }

        }


        /// <summary>
        /// 保存配置到配置文件
        /// </summary>
        /// <param name="fileName">配置文件名</param>
        public void SaveConfig(String fileName)
        {
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(fileName, Encoding.UTF8);

            try
            {
                writer.Formatting = System.Xml.Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("KTDictSeg");


                foreach (CfgItem item in GetCfgItems())
                {
                    writer.WriteComment(item.Comment);
                    writer.WriteStartElement("Item");
                    writer.WriteAttributeString("Name", item.Pi.Name);
                    writer.WriteAttributeString("Value", item.Pi.GetValue(this, null).ToString());
                    writer.WriteEndElement(); //Item
                }

                writer.WriteEndElement(); //KTDictSeg
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                WriteLog(String.Format("Save config fail, errmsg:{0}", e.Message));
            }
        }

        #endregion

        #region Private methods

        private void WriteLog(String log)
        {
            try
            {
                CFile.WriteLine(LogFileName,
                    String.Format("{0} {1}", DateTime.Now, log), "utf-8");
            }
            catch
            {
            }
        }


        double GetFreqWeight(List<WordInfo> words, List<int> list)
        {
            double weight = 0;

            for (int i = 0; i < list.Count; i++)
            {
                WordInfo w = (WordInfo)words[(int)list[i]];
                DictStruct dict = (DictStruct)w.Tag;
                weight += dict.Frequency;
            }

            return weight;
        }

        int GetPosWeight(List<WordInfo> words, List<int> list)
        {
            int weight = 0;

            for (int i = 0; i < list.Count - 1; i++)
            {
                WordInfo w1 = (WordInfo)words[(int)list[i]];
                WordInfo w2 = (WordInfo)words[(int)list[i + 1]];
                if (m_PosBinRule.Match(w1.Word, w2.Word))
                {
                    weight++;
                }
            }

            return weight;
        }

        bool CompareByPos(List<WordInfo> words, List<int> pre, List<int> cur)
        {
            int posWeightPre = GetPosWeight(words, pre);
            int posWeightCur = GetPosWeight(words, cur);

            if (posWeightPre < posWeightCur)
            {
                return true;
            }

            if (posWeightPre > posWeightCur)
            {
                return false;
            }

            //词性比较相同的情况下比较词频
            return GetFreqWeight(words, pre) < GetFreqWeight(words, cur);
        }

        /// <summary>
        /// 按词频优先进行选择
        /// </summary>
        /// <param name="words"></param>
        /// <param name="pre"></param>
        /// <param name="cur"></param>
        /// <returns></returns>
        private bool SelectByFreq(List<WordInfo> words, List<int> pre, List<int> cur)
        {
            double minPreFreq = 1000000000;
            double minCurFreq = 1000000000;
            int maxPreLength = 0; //Pre中所有词的最大
            int maxCurLength = 0; //Cur中所有词的最大

            foreach (int index in pre)
            {
                double freq = ((DictStruct)words[index].Tag).Frequency;
                if (freq < minPreFreq)
                {
                    minPreFreq = freq;
                }

                if (words[index].Word.Length > maxPreLength)
                {
                    maxPreLength = words[index].Word.Length;
                }
            }

            foreach (int index in cur)
            {
                double freq = ((DictStruct)words[index].Tag).Frequency;
                if (freq < minCurFreq)
                {
                    minCurFreq = freq;
                }

                if (words[index].Word.Length > maxCurLength)
                {
                    maxCurLength = words[index].Word.Length;
                }

            }

            //对于全部由单个字组成的词，不进行词频优先统计
            if (maxPreLength <= 1 && maxCurLength > 1)
            {
                return true;
            }
            else if (maxPreLength > 1 && maxCurLength <= 1)
            {
                return false;
            }

            return minCurFreq > minPreFreq;
        }

        private void InitRules()
        {
            m_Rules = new IRule[3];
            m_PosBinRule = new PosBinRule(m_POS);
            m_Rules[0] = new MergeNumRule(m_POS);
            m_Rules[1] = m_PosBinRule;
            m_MatchNameRule = new MatchName(m_POS);
            m_Rules[2] = m_MatchNameRule;
        }

        /// <summary>
        /// 合并浮点数
        /// </summary>
        /// <param name="words"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private String MergeFloat(ArrayList words, int start, ref int end)
        {
            StringBuilder str = new StringBuilder();

            int dotCount = 0;
            end = start;
            int i;

            for (i = start; i < words.Count; i++)
            {
                string word = (string)words[i];

                if (word == "")
                {
                    break;
                }

                if ((word[0] >= '0' && word[0] <= '9')
                    || (word[0] >= '０' && word[0] <= '９'))
                {
                }
                else if (word[0] == '.' && dotCount == 0)
                {
                    dotCount++;
                }
                else
                {
                    break;
                }

                str.Append(word);
            }

            end = i;

            return str.ToString();
        }

        /// <summary>
        /// 合并Email
        /// </summary>
        /// <param name="words"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private String MergeEmail(ArrayList words, int start, ref int end)
        {
            StringBuilder str = new StringBuilder();

            int dotCount = 0;
            int atCount = 0;
            end = start;
            int i;

            for (i = start; i < words.Count; i++)
            {
                string word = (string)words[i];

                if (word == "")
                {
                    break;
                }

                if ((word[0] >= 'a' && word[0] <= 'z') ||
                    (word[0] >= 'A' && word[0] <= 'Z') ||
                    word[0] >= '0' && word[0] <= '9')
                {
                    dotCount = 0;
                }
                else if (word[0] == '@' && atCount == 0)
                {
                    atCount++;
                }
                else if (word[0] == '.' && dotCount == 0)
                {
                    dotCount++;
                }
                else
                {
                    break;
                }

                str.Append(word);

            }

            end = i;

            return str.ToString();
        }

        /// <summary>
        /// 合并英文专用词。
        /// 如果字典中有英文专用词如U.S.A, C++.C#等
        /// 需要对初步分词后的英文和字母进行合并
        /// </summary>
        /// <param name="words"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private String MergeEnglishSpecialWord(CExtractWords extractWords, ArrayList words, int start, ref int end)
        {
            StringBuilder str = new StringBuilder();

            int i;

            for (i = start; i < words.Count; i++)
            {
                string word = (string)words[i];

                //word 为空或者为空格回车换行等分割符号，中断扫描
                if (word.Trim() == "")
                {
                    break;
                }

                //如果遇到中文，中断扫描
                if (word[0] >= 0x4e00 && word[0] <= 0x9fa5)
                {
                    break;
                }

                str.Append(word);
            }

            String mergeString = str.ToString();
            List<WordInfo> exWords = extractWords.ExtractFullText(mergeString);

            if (exWords.Count == 1)
            {
                WordInfo info = (WordInfo)exWords[0];
                if (info.Word.Length == mergeString.Length)
                {
                    end = i;
                    return mergeString;
                }
            }

            return null;

        }

        #endregion

        #region 维护停用词

        /// <summary>
        /// 从停用词字典中加载停用词
        /// 停用词字典的格式：
        /// 文本文件格式，一个词占一行
        /// </summary>
        /// <param name="chsFileName">中文停用词</param>
        /// <param name="engFileName">英文停用词</param>
        /// <remarks>对文件存取的异常不做异常处理，由调用者进行异常处理</remarks>
        public void LoadStopwordsDict(String chsFileName, String engFileName)
        {
            int numChrStop = 0;//统计中文停用词数目，并作为Value值插入哈希表
            int numEngStop = 0;//统计英文停用词数目，并作为Value值插入哈希表

            try
            {
                StreamReader swChrFile = new StreamReader(chsFileName, Encoding.GetEncoding("UTF-8"));
                StreamReader swEngFile = new StreamReader(engFileName, Encoding.GetEncoding("UTF-8"));

                //加载中文停用词
                while (!swChrFile.EndOfStream)
                {
                    //按行读取中文停用词
                    string strChrStop = swChrFile.ReadLine();

                    //如果哈希表中不包括该停用词则添加到哈希表中
                    if (!m_ChsStopwordTbl.Contains(strChrStop))
                    {
                        m_ChsStopwordTbl.Add(strChrStop, numChrStop);
                        numChrStop++;
                    }
                }

                //加载英文停用词
                while (!swEngFile.EndOfStream)
                {
                    //按行读取中文停用词
                    string strEngStop = swEngFile.ReadLine();

                    //如果哈希表中不包括该停用词则添加到哈希表中
                    if (!m_EngStopwordTbl.Contains(strEngStop))
                    {
                        m_EngStopwordTbl.Add(strEngStop, numEngStop);
                        numEngStop++;
                    }
                }

                swChrFile.Close();
                swEngFile.Close();
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 将中文停用词保存到文件中 
        /// </summary>
        /// <param name="fileName">要保存文件名</param>
        /// <remarks>对文件存取的异常不做异常处理，由调用者进行异常处理</remarks>
        public void SaveChsStopwordDict(String fileName)
        {
            try
            {
                //创建一个新的存储中文停用词的文本文件，若该文件存在则覆盖
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));


                //遍历中文停用词表，写入文件
                foreach (DictionaryEntry i in m_ChsStopwordTbl)
                {
                    sw.WriteLine(i.Key.ToString());
                }

                sw.Close();
                fs.Close();
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 将英文停用词保存到文件中 
        /// </summary>
        /// <param name="fileName">要保存文件名</param>
        /// <remarks>对文件存取的异常不做异常处理，由调用者进行异常处理</remarks>
        public void SaveEngStopwordDict(String fileName)
        {
            try
            {
                //创建一个新的存储英文停用词的文本文件，若该文件存在则覆盖
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));


                //遍历英文停用词表，写入文件
                foreach (DictionaryEntry i in m_EngStopwordTbl)
                {
                    sw.WriteLine(i.Key.ToString());
                }
                sw.Close();
                fs.Close();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 增加一个中文停用词
        /// </summary>
        /// <param name="word"></param>
        public void AddChsStopword(String word)
        {
            //如果原来词库中已存在，则不做任何操作
            if (m_ChsStopwordTbl.Contains(word))
            {
                return;
            }
            else
            {
                m_ChsStopwordTbl.Add(word, m_ChsStopwordTbl.Count);

            }

        }


        /// <summary>
        /// 删除一个中文停用词
        /// </summary>
        /// <param name="word"></param>
        public void DelChsStopword(String word)
        {
            //如果原来词库中不存在，则不做任何操作
            m_ChsStopwordTbl.Remove(word);
        }


        /// <summary>
        /// 增加一个英文停用词
        /// </summary>
        /// <param name="word"></param>
        public void AddEngStopword(String word)
        {
            //如果原来词库中已存在，则不做任何操作
            if (m_EngStopwordTbl.Contains(word))
            {
                return;
            }
            else
            {
                m_EngStopwordTbl.Add(word, m_EngStopwordTbl.Count);
            }
        }


        /// <summary>
        /// 删除一个英文停用词
        /// </summary>
        /// <param name="word"></param>
        public void DelEngStopword(String word)
        {
            //如果原来词库中不存在，则不做任何操作
            m_EngStopwordTbl.Remove(word);
        }

        #endregion

        #region 加载字典

        public void LoadDict()
        {
            LoadDict(true);
        }

        /// <summary>
        /// 加载单字词
        /// </summary>
        private void LoadSingleWords()
        {
            if (m_SingleWordsLoaded)
            {
                return;
            }

            foreach (string sWord in Dict.LoadSingleWordFromFile(m_DictPath + "SingleWords.txt"))
            {
                DictStruct word = new DictStruct();
                word.Word = sWord;
                word.Frequency = 0;
                word.Pos = 0;

                if (m_ExtractWords.InsertWordToDfa(word.Word, word))
                {
                    m_POS.AddWordPos(word.Word, word.Pos);
                }
            }

            m_SingleWordsLoaded = true;
        }

        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="clear">是否清除词频</param>
        public void LoadDict(bool clear)
        {
            //var po = new KTDictSeg.CPOS();
            //KTDictSeg.MatchName mn = new KTDictSeg.MatchName(po);
            //mn.LoadNameTraffic(@"D:\工具\v1.4.02\Release\Data\Name.dct");
            
            ////mn.m_ChsNameTraffic
            //m_MatchNameRule.chsNameTraffic.
           // m_MatchNameRule.
            //加载姓名前缀后缀统计表
            //m_MatchNameRule.SaveNameTraffic(@"c:\\Name.dct");
            m_MatchNameRule.LoadNameTraffic(m_DictPath + "Name.dct");

            //加载字典
            m_Dict = Dict.LoadFromBinFileEx(m_DictPath + "Dict.dct");
            m_DictMgr.Dict = m_Dict;

            foreach (DictStruct word in m_Dict.Dicts)
            {
                if (clear)
                {
                    word.Frequency = 0;
                }

                if (m_ExtractWords.InsertWordToDfa(word.Word, word))
                {
                    m_POS.AddWordPos(word.Word, word.Pos);
                }
            }

            //加载未登录词统计字典
            if (File.Exists(m_DictPath + "UnknownWords.dct"))
            {
                m_UnknownWordsDict = Dict.LoadFromBinFileEx(m_DictPath + "UnknownWords.dct");
            }
            else
            {
                m_UnknownWordsDict = new DictFile();
            }

            m_UnknownWordsDictMgr.Dict = m_UnknownWordsDict;

            if (clear)
            {
                m_MatchNameRule.ClearNameTraffic();
            }

            m_MatchNameRule.TrafficUnknownWordHandle = TrafficUnknownWord;
        }

        public void SaveDict()
        {
            try
            {
                m_MatchNameRule.SaveNameTraffic(m_DictPath + "Name.dct");

                foreach (DictStruct word in m_Dict.Dicts)
                {
                    DictStruct dict = (DictStruct)m_ExtractWords.GetTag(word.Word);
                    if (dict != null)
                    {
                        word.Frequency = dict.Frequency;
                    }
                }

                if (AutoInsertUnknownWords)
                {
                    Dict.SaveToBinFileEx(m_DictPath + "Dict.dct", m_Dict);
                }

                Dict.SaveToBinFileEx(m_DictPath + "UnknownWords.dct", m_UnknownWordsDict);
            }
            catch (Exception e)
            {
                WriteLog(string.Format("Save Dict fail. Errmsg:{0} Stack={1}", e.Message, e.StackTrace));
            }
        }

        #endregion

        #region 分词属性
        bool m_MatchName;

        /// <summary>
        /// 是否匹配汉语人名
        /// </summary>
        public bool MatchName
        {
            get
            {
                return m_MatchName;
            }

            set
            {
                m_MatchName = value;
            }
        }

        MatchDirection m_MatchDirection;

        /// <summary>
        /// 匹配方向
        /// 默认为从左至右匹配,即正向匹配
        /// </summary>
        public MatchDirection MatchDirection
        {
            get
            {
                return m_MatchDirection;
            }

            set
            {
                m_MatchDirection = value;
            }
        }


        bool m_FilterStopWords;

        /// <summary>
        /// 是否过滤停用词
        /// </summary>
        public bool FilterStopWords
        {
            get
            {
                return m_FilterStopWords;
            }

            set
            {
                if (value)
                {
                    if (m_ChsStopwordTbl.Count == 0 || m_EngStopwordTbl.Count == 0)
                    {
                        LoadStopwordsDict(m_DictPath + CHS_STOP_WORD_FILENAME, m_DictPath + ENG_STOP_WORD_FILENAME);
                    }
                }

                m_FilterStopWords = value;

            }
        }


        #endregion

        #region 构造函数

        public CSimpleDictSeg()
        {
            m_MatchName = false;
            m_FilterStopWords = false;
            m_MatchDirection = MatchDirection.LeftToRight;
            m_ExtractWords = new CExtractWords();
            m_ExtractWords.CompareByPosEvent = CompareByPos;
            m_POS = new CPOS();
            m_LastSaveTime = DateTime.Now;
            InitRules();

        }

        #endregion

        #region 分词

        private void InsertWordToArray(int pos, WordInfo wordInfo, List<WordInfo> arr)
        {
            wordInfo.Position = pos;

            arr.Add(wordInfo);
        }

        private void InsertWordToArray(int pos, String word, int rank, List<WordInfo> arr)
        {
            WordInfo wordInfo = new WordInfo();
            wordInfo.Position = pos;
            wordInfo.Word = word;
            wordInfo.Rank = rank;
            wordInfo.Tag = null;

            arr.Add(wordInfo);
        }

        private void InsertWordToArray(String word, List<String> arr)
        {
            arr.Add(word);
        }

        /// <summary>
        /// 预分词
        /// </summary>
        /// <param name="str">要分词的句子</param>
        /// <param name="multiSelect">多元分词选项</param>
        /// <returns>预分词后的字符串输出</returns>
        private List<String> PreSegment(String str)
        {
            ArrayList initSeg = new ArrayList();


            if (!CRegex.GetSingleMatchStrings(str, PATTERNS, true, ref initSeg))
            {
                return new List<String>();
            }

            List<String> retWords = new List<String>();

            int i = 0;

            m_ExtractWords.MatchDirection = MatchDirection;

            while (i < initSeg.Count)
            {
                String word = (String)initSeg[i];
                if (word == "")
                {
                    word = " ";
                }

                if (i < initSeg.Count - 1)
                {
                    bool mergeOk = false;
                    if (((word[0] >= '0' && word[0] <= '9') || (word[0] >= '０' && word[0] <= '９')) &&
                        ((word[word.Length - 1] >= '0' && word[word.Length - 1] <= '9') ||
                         (word[word.Length - 1] >= '０' && word[word.Length - 1] <= '９'))
                        )
                    {
                        //合并浮点数
                        word = MergeFloat(initSeg, i, ref i);
                        mergeOk = true;
                    }
                    else if ((word[0] >= 'a' && word[0] <= 'z') ||
                             (word[0] >= 'A' && word[0] <= 'Z')
                             )
                    {
                        //合并成英文专业名词
                        String specialEnglish = MergeEnglishSpecialWord(m_ExtractWords, initSeg, i, ref i);

                        if (specialEnglish != null)
                        {
                            InsertWordToArray(specialEnglish, retWords);
                            continue;
                        }

                        //合并邮件地址
                        if ((String)initSeg[i + 1] != "")
                        {
                            if (((String)initSeg[i + 1])[0] == '@')
                            {
                                word = MergeEmail(initSeg, i, ref i);
                                mergeOk = true;
                            }
                        }
                    }

                    if (mergeOk)
                    {
                        InsertWordToArray(word, retWords);
                        continue;
                    }
                }


                if (word[0] < 0x4e00 || word[0] > 0x9fa5)
                {
                    //英文或符号，直接加入
                    InsertWordToArray(word, retWords);
                }
                else
                {

                    List<WordInfo> words = m_ExtractWords.ExtractFullTextMaxMatch(word, MultiSelect, Redundancy);
                    int lastPos = 0;
                    bool lstIsName = false; //前一个词是人名

                    foreach (WordInfo wordInfo in words)
                    {

                        if (lastPos < wordInfo.Position)
                        {
                            /*
                                                        String unMatchWord = word.Substring(lastPos, wordInfo.Position - lastPos);

                                                        InsertWordToArray(unMatchWord, retWords);
                            */
                            //中间有未匹配词，将单个字逐个加入
                            for (int j = lastPos; j < wordInfo.Position; j++)
                            {
                                InsertWordToArray(word[j].ToString(), retWords);
                            }

                        }


                        lastPos = wordInfo.Position + wordInfo.Word.Length;

                        //统计中文姓名的后缀
                        if (AutoStudy && lstIsName)
                        {
                            DictStruct wordDict = (DictStruct)wordInfo.Tag;
                            if ((wordDict.Pos & (int)EnumPOS.POS_A_NR) == 0)
                            {
                                m_MatchNameRule.AddBefore(wordInfo.Word);
                            }

                            lstIsName = false;
                        }

                        //统计中文姓名的前缀
                        //如总统，主席等
                        if ((((DictStruct)wordInfo.Tag).Pos & (int)EnumPOS.POS_A_NR) != 0)
                        {
                            if (wordInfo.Word.Length > 1 && wordInfo.Word.Length <= 4 && retWords.Count > 0 && AutoStudy && !lstIsName)
                            {
                                DictStruct wordDict = (DictStruct)wordInfo.Tag;
                                m_MatchNameRule.AddBefore(retWords[retWords.Count - 1]);
                            }

                            lstIsName = true;
                        }


                        InsertWordToArray(wordInfo.Word, retWords);


                    }

                    if (lastPos < word.Length)
                    {
                        //尾部有未匹配词，将单个字逐个加入
                        for (int j = lastPos; j < word.Length; j++)
                        {
                            InsertWordToArray(word[j].ToString(), retWords);
                        }

                        //InsertWordToArray(word.Substring(lastPos, word.Length - lastPos), retWords);
                    }
                }

                i++;
            }

            return retWords;
        }

        private void TrafficUnknownWord(String word, EnumPOS Pos)
        {
            if (word.Length <= 1 || word.Length > 3)
            {
                return;
            }

            DictStruct unknownWord = m_UnknownWordsDictMgr.GetWord(word);


            if (unknownWord == null)
            {
                m_UnknownWordsDictMgr.InsertWord(word, 1, (int)Pos);
                return;
            }

            //如果是屏蔽的未登录词，则不加入
            //屏蔽的未登录词用词性等于0来表示
            if (unknownWord.Pos == 0)
            {
                return;
            }

            unknownWord.Pos |= (int)Pos;
            unknownWord.Frequency++;

            if (unknownWord.Frequency > UnknownWordsThreshold && AutoInsertUnknownWords)
            {
                DictStruct w = m_DictMgr.GetWord(word);
                if (w == null)
                {
                    m_DictMgr.InsertWord(word, unknownWord.Frequency, unknownWord.Pos);

                    m_ExtractWords.InsertWordToDfa(word, unknownWord);
                    m_POS.AddWordPos(word, unknownWord.Pos);

                }
                else
                {
                    w.Pos |= unknownWord.Pos;
                    w.Frequency += unknownWord.Frequency;
                }

                unknownWord.Frequency = 0;
            }
        }

        /// <summary>
        /// 召回未登录词
        /// </summary>
        /// <returns></returns>
        private List<String> RecoverUnknowWord(List<String> words)
        {
            List<String> retWords = new List<String>();

            int i = 0;
            int j = 0;

            while (i < words.Count)
            {
                String w = (String)words[i];

                if (i == words.Count - 1)
                {
                    retWords.Add(w);
                    break;
                }

                if (m_POS.IsUnknowOneCharWord(w))
                {
                    String word = w;
                    i++;

                    while (m_POS.IsUnknowOneCharWord(words[i]))
                    {
                        word += (String)words[i];
                        i++;
                        if (i >= words.Count)
                        {
                            break;
                        }
                    }

                    if (AutoStudy)
                    {
                        TrafficUnknownWord(word, EnumPOS.POS_A_NZ);

                        //将所有连续单字组成一个词，假设其为未登录词，进行统计
                        if (j < i && w[0] >= 0x4e00 && w[0] <= 0x9fa5)
                        {
                            j = i;

                            if (j < words.Count)
                            {
                                String longWord = word;

                                while (words[j].Length == 1 && words[j][0] >= 0x4e00 && words[j][0] <= 0x9fa5)
                                {
                                    longWord += words[j];
                                    j++;

                                    if (j >= words.Count)
                                    {
                                        break;
                                    }
                                }

                                TrafficUnknownWord(longWord, EnumPOS.POS_A_NZ);
                            }
                        }
                    }

                    retWords.Add(word);
                    continue;
                }
                else
                {
                    if (AutoStudy)
                    {
                        //将所有连续单字组成一个词，假设其为未登录词，进行统计
                        if (j <= i && w.Length == 1 && w[0] >= 0x4e00 && w[0] <= 0x9fa5)
                        {
                            j = i + 1;
                            String word = w;

                            if (j < words.Count)
                            {
                                while (words[j].Length == 1 && words[j][0] >= 0x4e00 && words[j][0] <= 0x9fa5)
                                {
                                    word += words[j];
                                    j++;

                                    if (j >= words.Count)
                                    {
                                        break;
                                    }
                                }

                                TrafficUnknownWord(word, EnumPOS.POS_A_NZ);
                            }
                        }
                    }

                    retWords.Add(w);
                }

                i++;
            }

            return retWords;

        }

        /// <summary>
        /// 多元分词，不屏蔽停用词
        /// </summary>
        /// <param name="str">输入文本</param>
        /// <returns></returns>
        private List<WordInfo> SegmentNoStopWordMultiSelect(string str)
        {
            ArrayList initSeg = new ArrayList();
            int curPosition = 0;

            if (!CRegex.GetSingleMatchStrings(str, PATTERNS, true, ref initSeg))
            {
                return new List<WordInfo>();
            }

            List<WordInfo> retWords = new List<WordInfo>();

            int i = 0;

            m_ExtractWords.MatchDirection = MatchDirection;

            while (i < initSeg.Count)
            {
                String word = (String)initSeg[i];
                if (word == "")
                {
                    curPosition++;
                    i++;
                    continue;
                }

                if (i < initSeg.Count - 1)
                {
                    bool mergeOk = false;
                    if (((word[0] >= '0' && word[0] <= '9') || (word[0] >= '０' && word[0] <= '９')) &&
                        ((word[word.Length - 1] >= '0' && word[word.Length - 1] <= '9') ||
                         (word[word.Length - 1] >= '０' && word[word.Length - 1] <= '９'))
                        )
                    {
                        //合并浮点数
                        word = MergeFloat(initSeg, i, ref i);
                        mergeOk = true;
                    }
                    else if ((word[0] >= 'a' && word[0] <= 'z') ||
                             (word[0] >= 'A' && word[0] <= 'Z')
                             )
                    {
                        //合并成英文专业名词
                        String specialEnglish = MergeEnglishSpecialWord(m_ExtractWords, initSeg, i, ref i);

                        if (specialEnglish != null)
                        {
                            curPosition = str.IndexOf(specialEnglish, curPosition);
                            InsertWordToArray(curPosition, specialEnglish, 5, retWords);
                            curPosition += specialEnglish.Length;
                            continue;
                        }

                        //合并邮件地址
                        if ((String)initSeg[i + 1] != "")
                        {
                            if (((String)initSeg[i + 1])[0] == '@')
                            {
                                word = MergeEmail(initSeg, i, ref i);
                                mergeOk = true;
                            }
                        }
                    }

                    if (mergeOk)
                    {
                        curPosition = str.IndexOf(word, curPosition);
                        InsertWordToArray(curPosition, word, 5, retWords);
                        curPosition += word.Length;
                        continue;
                    }
                }


                if (word[0] < 0x4e00 || word[0] > 0x9fa5)
                {
                    //英文或符号，直接加入
                    curPosition = str.IndexOf(word, curPosition);
                    InsertWordToArray(curPosition, word, 5, retWords);
                    curPosition += word.Length;
                }
                else
                {
                    List<WordInfo> words = m_ExtractWords.ExtractFullTextMaxMatch(word, MultiSelect, Redundancy);

                    foreach (WordInfo wordInfo in words)
                    {
                        if (Redundancy == 0 && wordInfo.Word.Length <= 1)
                        {
                            continue;
                        }

                        InsertWordToArray(curPosition + wordInfo.Position, wordInfo, retWords);
                    }

                    //合并未登录词
                    int unknowWordLen = 0;
                    for (int j = 0; j < word.Length; j++)
                    {
                        if (m_ExtractWords.HitTable[j] == 0)
                        {
                            if (Redundancy > 0)
                            {
                                InsertWordToArray(curPosition + j, word[j].ToString(), 0, retWords);
                            }

                            unknowWordLen++;
                        }
                        else if (m_ExtractWords.HitTable[j] == 2)
                        {
                            unknowWordLen++;
                        }
                        else if (m_ExtractWords.HitTable[j] == 1)
                        {
                            if (unknowWordLen > 1 || (unknowWordLen == 1 && Redundancy == 0))
                            {
                                int start = j - unknowWordLen;

                                //Traffic unknown word
                                if (AutoStudy && unknowWordLen > 1)
                                {
                                    TrafficUnknownWord(word.Substring(start, unknowWordLen), EnumPOS.POS_A_NZ);
                                }

                                InsertWordToArray(curPosition + start, word.Substring(start, unknowWordLen), 2, retWords);
                            }

                            unknowWordLen = 0;
                        }
                    }

                    if (unknowWordLen > 1 || (unknowWordLen == 1 && Redundancy == 0))
                    {
                        int start = word.Length - unknowWordLen;

                        //Traffic unknown word
                        if (AutoStudy && unknowWordLen > 1)
                        {
                            TrafficUnknownWord(word.Substring(start, unknowWordLen), EnumPOS.POS_A_NZ);
                        }

                        InsertWordToArray(curPosition + start, word.Substring(start, unknowWordLen), 2, retWords);
                    }

                    curPosition += word.Length;
                }

                i++;
            }

            return retWords;
        }

        /// <summary>
        /// 分词,不屏蔽停用词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<String> SegmentNoStopWord(String str)
        {
            List<String> preWords = PreSegment(str);
            List<String> retWords = new List<String>();

            int index = 0;
            while (index < preWords.Count)
            {
                int next = -1;
                foreach (IRule rule in m_Rules)
                {
                    if (!m_MatchName && rule is MatchName)
                    {
                        continue;
                    }

                    next = rule.ProcRule(preWords, index, retWords);
                    if (next > 0)
                    {
                        index = next;
                        break;
                    }
                }

                if (next > 0)
                {
                    continue;
                }

                retWords.Add(preWords[index]);
                index++;
            }

            //return retWords;
            List<String> retStrings = RecoverUnknowWord(retWords);

            if (AutoStudy)
            {
                foreach (String word in retStrings)
                {
                    DictStruct dict = (DictStruct)m_ExtractWords.GetTag(word);

                    if (dict != null)
                    {
                        dict.Frequency++;
                    }

                }
            }

            return retStrings;
        }

        /// <summary>
        /// 定期保存最新的字典和统计信息
        /// </summary>
        private void SaveDictOnTime()
        {
            if (!AutoStudy)
            {
                return;
            }

            TimeSpan s = DateTime.Now - m_LastSaveTime;

            if (s.TotalSeconds > AutoSaveInterval)
            {
                m_LastSaveTime = DateTime.Now;
                SaveDict();
            }
        }

        private List<WordInfo> FilterStopWordsForMultiSelect(List<WordInfo> words)
        {
            for (int i = 0; i < words.Count; i++)
            {
                WordInfo word = words[i];

                if (m_FilterStopWords)
                {
                    if (m_ChsStopwordTbl[word.Word] != null || m_EngStopwordTbl[word.Word] != null)
                    {
                        words[i] = null;
                        continue;
                    }
                }
            }

            return words;
        }

        #endregion


        #region Public Methods
        /// <summary>
        /// 分词并输出单词信息列表 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<WordInfo> SegmentToWordInfos(String str)
        {
            //定时保存字典
            SaveDictOnTime();

            if (MultiSelect)
            {
                LoadSingleWords();
                return FilterStopWordsForMultiSelect(SegmentNoStopWordMultiSelect(str));
            }

            List<String> words = SegmentNoStopWord(str);

            List<WordInfo> retWords = new List<WordInfo>();
            int position = 0;

            foreach (String word in words)
            {
                if (m_FilterStopWords)
                {
                    if (m_ChsStopwordTbl[word] != null || m_EngStopwordTbl[word] != null)
                    {
                        position += word.Length;
                        continue;
                    }
                }

                WordInfo wordInfo = new WordInfo();
                wordInfo.Word = word;
                wordInfo.Position = position;
                retWords.Add(wordInfo);
                position += word.Length;
            }

            return retWords;
        }

        /// <summary>
        /// 分词只输出单词列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<String> Segment(String str)
        {
            //定时保存字典
            SaveDictOnTime();

            List<String> words = SegmentNoStopWord(str);

            if (!m_FilterStopWords)
            {
                return words;
            }
            else
            {
                List<String> retWords = new List<String>();

                foreach (String word in words)
                {
                    if (m_ChsStopwordTbl[word] != null || m_EngStopwordTbl[word] != null)
                    {
                        continue;
                    }

                    retWords.Add(word);
                }

                return retWords;
            }
        }

        #endregion
    }
}
