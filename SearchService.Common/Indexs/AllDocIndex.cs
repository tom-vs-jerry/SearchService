using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;

using Segment;
using Segment.Dict;

using SearchService.Model;
using SearchService.Common.Analyzers;
using SearchService.Common.Log;

namespace SearchService.Common.Indexs
{
    public class AllDocIndex
    {
        #region 属性
        //全文索引路径
        private static string AllIndexDir = ConfigurationManager.AppSettings["AllWordsIndexPath"];
        //全文词典配置路径
        private static string AllSegmentConfigPath = ConfigurationManager.AppSettings["AllWordsDicConfigPath"];

        //敏感词索引路径
        //private static string SensIndexDir = ConfigurationManager.AppSettings["SensWordsIndexPath"];
        //敏感词词典配置文件
        //private static string SenssegmentConfigPath = ConfigurationManager.AppSettings["SensWordsDicConfigPath"];

        //全文索引最大结果数
        private static int SearchAllResultLength = Convert.ToInt32(ConfigurationManager.AppSettings["SearchAllResultLength"]);
        private static int SearchSensResultLength = Convert.ToInt32(ConfigurationManager.AppSettings["SearchSensResultLength"]);

        //读写全文索引
        private static IndexWriter writer = null;
        private static IndexSearcher searcher = null;

        //读写敏感词索引
        //private static IndexWriter SensWriter = null;
        //private static IndexSearcher SensSearcher = null;

        //全文索引分析器
        private static Analyzers.AllDocTokenizer tokenizer = null;//new Analyzers.AllDocTokenizer(segmentConfigPath);
        private static Analyzers.AllDocAnalyzer analyzer = null;// new Analyzers.AllDocAnalyzer();

        //敏感词索引分析器
        //private static Analyzers.SensTokenizer Senstokenizer = null;//new Analyzers.AllDocTokenizer(segmentConfigPath);
        //private static Analyzers.SensAnalyzer Sensanalyzer = null;


        //全文索引文件缓存
        private static FSDirectory idxDoc = null;
        //全文索引分词词典
        private static Segment.Dict.WordDictionary Dic = null;

        //敏感词索引文件缓存
        //private static FSDirectory idxSensDoc = null;
        //敏感词分词词典
        //private static Segment.Dict.WordDictionary SensDic = null;

        private static HighLight.SimpleHTMLFormatter simpleHTMLFormatter = null;// new SimpleHTMLFormatter("<span style='color:red'>", "</span>");
        private static HighLight.AllDocHighLighter highlighter = null;// new Highlighter(simpleHTMLFormatter, tokenizer);

        private static HighLight.SimpleHTMLFormatter SenssimpleHTMLFormatter = null;// new SimpleHTMLFormatter("<span style='color:red'>", "</span>");
        private static HighLight.SensHighLighter Senshighlighter = null;// new Highlighter(simpleHTMLFormatter, tokenizer);

        //private static List<string> SensDicList = null;
        #endregion

        #region 消息事件
        //消息事件
        public delegate void AllDocMessageEventHandler(object sender, EventArgs e);

        public static event AllDocMessageEventHandler AllDocMessageEvent;
        static void OnMessages(object sender, EventArgs e)
        {
            if (AllDocMessageEvent != null)
            {
                AllDocMessageEvent(sender, e);
            }
        }
        #endregion

        #region 初始化
        public static void Start()
        {
            object message = "";

            #region 加载全文词库
            try
            {
                message = "……全文词库加载……";
                OnMessages(message, null);
                if (tokenizer == null)
                {
                    tokenizer = new Analyzers.AllDocTokenizer(AllSegmentConfigPath);
                    Dic = Analyzers.AllDocTokenizer.segment.WordDictionary;
                    analyzer = new Analyzers.AllDocAnalyzer();
                }
                message = string.Format("——全文词库加载完成，已加载{0}——", Dic.Count.ToString());
                OnMessages(message, null);
            }
            catch (Exception ex)
            {
                message = string.Format("——全文词库加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion

            //#region 加载词库
            //try
            //{
            //    message = "……敏感词词库加载……";
            //    OnMessages(message, null);
            //    if (Senstokenizer == null)
            //    {
            //        Senstokenizer = new Analyzers.SensTokenizer(SenssegmentConfigPath);
            //        SensDic = Analyzers.SensTokenizer.segment.WordDictionary;
            //        Sensanalyzer = new Analyzers.SensAnalyzer();

            //        SensDicList = new List<string>();

            //        foreach (char key in SensDic._FirstCharDict.Keys)
            //        {
            //            WordAttribute wab = SensDic._FirstCharDict[key];
            //            SensDicList.Add(wab.Word);
            //        }
            //        foreach (uint key in SensDic._DoubleCharDict.Keys)
            //        {
            //            WordAttribute wab = SensDic._DoubleCharDict[key];
            //            SensDicList.Add(wab.Word);
            //        }
            //        foreach (string key in SensDic._WordDict.Keys)
            //        {
            //            WordAttribute wab = SensDic._WordDict[key];
            //            SensDicList.Add(wab.Word);
            //        }
            //    }
            //    message = string.Format("——敏感词词库加载完成，已加载{0}——", SensDic.Count.ToString());
            //    OnMessages(message, null);
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词词库加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            //#endregion

            #region 将全文索引放入内存
            try
            {
                if (idxDoc == null)
                {
                    idxDoc = FSDirectory.Open(AllIndexDir);// new RAMDirectory(FSDirectory.Open(IndexDir));
                   
                }
            }
            catch (Exception ex)
            {
                message = string.Format("——全文索引文件加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }

            //将全文索引放入内存
            //try
            //{
            //    if (idxSensDoc == null)
            //    {
            //        idxSensDoc = FSDirectory.Open(SensIndexDir);// new RAMDirectory(FSDirectory.Open(IndexDir));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词索引文件加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            #endregion

            #region 判断索引文件是否存在
            string[] exts = new string[] { ".fdt", ".fdx", ".fnm", ".frq", ".nrm", ".prx", ".tii", ".tis" };           
            bool isExitsFiles = System.IO.Directory.GetFiles(AllIndexDir).Where(s => exts.Contains(System.IO.Path.GetExtension(s).ToLower())).Count() > 0 ? true : false;
            //bool isExitsSensFiles = System.IO.Directory.GetFiles(SensIndexDir).Where(s => exts.Contains(System.IO.Path.GetExtension(s).ToLower())).Count() > 0 ? true : false;
            #endregion

            #region 写索引
            try
            {
                message = "……全文搜索写入服务启动中……。";
                OnMessages(message, null);
                if (writer == null)
                {
                    writer = new IndexWriter(idxDoc, analyzer, !isExitsFiles, IndexWriter.MaxFieldLength.UNLIMITED);
                }
                message = "——全文搜索写入服务加载已完成。——";
                OnMessages(message, null);
            }
            catch (Exception ex)
            {
                message = string.Format("——全文搜索写入服务加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion

            #region 搜索索引
            try
            {
                message = "……全文搜索查询服务启动中……。";
                OnMessages(message, null);
                if (searcher == null)
                {
                    simpleHTMLFormatter = new HighLight.SimpleHTMLFormatter("<span style='color:red'>", "</span>");
                    highlighter = new HighLight.AllDocHighLighter(simpleHTMLFormatter, tokenizer);

                    searcher = new IndexSearcher(new RAMDirectory(idxDoc), false);

                    message = "——全文搜索查询服务加载已完成。——";
                    OnMessages(message, null);
                }
            }
            catch (Exception ex)
            {
                message = string.Format("——全文搜索查询服务加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion

            //#region 写敏感词索引
            //try
            //{
            //    message = "……敏感词搜索写入服务启动中……。";
            //    OnMessages(message, null);
            //    if (SensWriter == null)
            //    {
            //        SensWriter = new IndexWriter(idxSensDoc, Sensanalyzer, !isExitsSensFiles, IndexWriter.MaxFieldLength.UNLIMITED);
            //    }
            //    message = "——敏感词搜索写入服务加载已完成。——";
            //    OnMessages(message, null);
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词搜索写入服务加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            //#endregion

            //#region  敏感词搜索索引
            //try
            //{
            //    message = "……敏感词搜索查询服务启动中……。";
            //    OnMessages(message, null);
            //    if (SensSearcher == null)
            //    {
            //        SenssimpleHTMLFormatter = new HighLight.SimpleHTMLFormatter("<span style='color:red'>", "</span>");
            //        Senshighlighter = new HighLight.SensHighLighter(SenssimpleHTMLFormatter, Senstokenizer);

            //        SensSearcher = new IndexSearcher(new RAMDirectory(idxSensDoc), false);

            //        message = "——敏感词搜索查询服务加载已完成。——";
            //        OnMessages(message, null);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词搜索查询服务加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            //#endregion
        }
        #endregion

        #region 重新加载
        /// <summary>
        /// 重新加载
        /// </summary>
        /// <param name="indexDir"></param>
        public static void Rebuild()
        {
            //全文索引相关          

            if (searcher != null)
            {
                searcher.Dispose();
                searcher = null;
            }
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
            if (idxDoc != null)
            {
                idxDoc.Dispose();
                idxDoc = null;
            }
            //if (Dic != null)
            //{
            //    Dic.Clear();
            //    Dic = null;
            //}
            if (analyzer != null)
            {
                analyzer.Dispose();
                analyzer = null;
            }
            if (tokenizer != null)
            {
                tokenizer.Dispose();
                tokenizer = null;
            } 

           
            //敏感词相关
            //if (SensSearcher != null)
            //{
            //    SensSearcher.Dispose();
            //    SensSearcher = null;
            //} 
            //if (SensWriter != null)
            //{
            //    SensWriter.Dispose();
            //    SensWriter = null;
            //}
            //if (idxSensDoc != null)
            //{
            //    idxSensDoc.Dispose();
            //    idxSensDoc = null;
            //}
            //if (SensDic != null)
            //{
            //    SensDic.Clear();
            //    SensDic = null;
            //}
            //if (Sensanalyzer != null)
            //{
            //    Sensanalyzer.Dispose();
            //    Sensanalyzer=null;
            //}
            //if (Senstokenizer != null)
            //{
            //    Senstokenizer.Dispose();
            //    Senstokenizer = null;
            //}
            //if (SensDicList != null)
            //{
            //    SensDicList.Clear();
            //    SensDicList = null;
            //}

            Start();

        }

        /// <summary>
        /// 重新加载分析器
        /// </summary>
        public static void RebuildAnalyzer()
        {
            

            if (analyzer!=null)
            {
                analyzer.Dispose();
                analyzer = null;
            }

            //if (Senstokenizer != null)
            //{
            //    Senstokenizer.Dispose();
            //    Senstokenizer = null;
            //}

            //if (SensDicList != null) {
            //    SensDicList.Clear();
            //    SensDicList = null;
            //}

            //if(SensDic != null)
            //{
            //    SensDic.Clear();
            //    SensDic = null;
            //}
            //全文索引相关
            if (tokenizer != null)
            {
                tokenizer.Dispose();
                tokenizer = null;
            }
            //敏感词相关
            //if (Senstokenizer != null)
            //{
            //    Senstokenizer.Dispose();
            //    Senstokenizer = null;
            //}

            string message = "";

            #region 加载全文词库
            try
            {
                message = "……全文词库重新加载……";
                OnMessages(message, null);
                if (tokenizer == null)
                {
                    tokenizer = new Analyzers.AllDocTokenizer(AllSegmentConfigPath);
                    Dic = Analyzers.AllDocTokenizer.segment.WordDictionary;
                    analyzer = new Analyzers.AllDocAnalyzer();
                }
                message = string.Format("——全文词库重新加载完成，已加载{0}——", Dic.Count.ToString());
                OnMessages(message, null);
            }
            catch (Exception ex)
            {
                message = string.Format("——全文词库重新加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion
            #region 加载词库
            //try
            //{
                //message = "……敏感词词库重新加载……";
                //OnMessages(message, null);
                //if (Senstokenizer == null)
                //{
                //    Senstokenizer = new Analyzers.SensTokenizer(SenssegmentConfigPath);
                //    SensDic = Analyzers.SensTokenizer.segment.WordDictionary;
                //    Sensanalyzer = new Analyzers.SensAnalyzer();

                //    SensDicList = new List<string>();

                //    foreach (char key in SensDic._FirstCharDict.Keys)
                //    {
                //        WordAttribute wab = SensDic._FirstCharDict[key];
                //        SensDicList.Add(wab.Word);
                //    }
                //    foreach (uint key in SensDic._DoubleCharDict.Keys)
                //    {
                //        WordAttribute wab = SensDic._DoubleCharDict[key];
                //        SensDicList.Add(wab.Word);
                //    }
                //    foreach (string key in SensDic._WordDict.Keys)
                //    {
                //        WordAttribute wab = SensDic._WordDict[key];
                //        SensDicList.Add(wab.Word);
                //    }
                //}
                //message = string.Format("——敏感词词库重新加载完成，已加载{0}——", SensDic.Count.ToString());
                //OnMessages(message, null);
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词词库重新加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            #endregion

        }

        /// <summary>
        /// 重新加载敏感词索引
        /// </summary>
        //public static void RebuildSensSearch()
        //{          
           
        //    if (SensWriter != null)
        //    {
        //        SensWriter.Dispose();
        //        SensWriter = null;
        //    }

        //    if (idxSensDoc != null)
        //    {
        //        idxSensDoc.Dispose();
        //        idxSensDoc = null;
        //    }

        //    if (SensSearcher != null)
        //    {
        //        SensSearcher.Dispose();
        //        SensSearcher = null;
        //    }

        //    string message = "";
        //    try
        //    {
        //        if (idxSensDoc == null)
        //        {
        //            idxSensDoc = FSDirectory.Open(SensIndexDir);// new RAMDirectory(FSDirectory.Open(IndexDir));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = string.Format("——敏感词索引文件重新加载失败。失败原因：{0}——", ex.Message);
        //        OnMessages(message, null);
        //    }

        //    #region 写敏感词索引
        //    try
        //    {
        //        message = "……敏感词搜索写入服务重新启动中……。";
        //        OnMessages(message, null);
        //        if (SensWriter == null)
        //        {
        //            SensWriter = new IndexWriter(idxSensDoc, Sensanalyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
        //        }
        //        message = "——敏感词搜索写入服务重新加载已完成。——";
        //        OnMessages(message, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        message = string.Format("——敏感词搜索写入服务重新加载失败。失败原因：{0}——", ex.Message);
        //        OnMessages(message, null);
        //    }
        //    #endregion

        //    #region  敏感词搜索索引
        //    try
        //    {
        //        message = "……敏感词搜索查询服务重新启动中……。";
        //        OnMessages(message, null);
        //        if (SensSearcher == null)
        //        {
        //            SenssimpleHTMLFormatter = new HighLight.SimpleHTMLFormatter("<span style='color:red'>", "</span>");
        //            Senshighlighter = new HighLight.SensHighLighter(SenssimpleHTMLFormatter, Senstokenizer);

        //            SensSearcher = new IndexSearcher(new RAMDirectory(idxSensDoc), false);

        //            message = "——敏感词搜索查询服务重新加载已完成。——";
        //            OnMessages(message, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = string.Format("——敏感词搜索查询服务重新加载失败。失败原因：{0}——", ex.Message);
        //        OnMessages(message, null);
        //    }
        //    #endregion
        //}
        /// <summary>
        /// 重新加载全文索引
        /// </summary>
        public static void RebuildAllSearch()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }

            if (idxDoc != null)
            {
                idxDoc.Dispose();
                idxDoc = null;
            }

            if (searcher != null)
            {
                searcher.Dispose();
                searcher = null;
            }
            
            #region 将全文索引放入内存

            string message = "";
            try
            {
                if (idxDoc == null)
                {
                    idxDoc = FSDirectory.Open(AllIndexDir);// new RAMDirectory(FSDirectory.Open(IndexDir));
                }
            }
            catch (Exception ex)
            {
                message = string.Format("——全文索引文件重新加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }

            
            #endregion

            #region 写索引
            try
            {
                message = "……全文搜索写入服务重新启动中……。";
                OnMessages(message, null);
                if (writer == null)
                {
                    writer = new IndexWriter(idxDoc, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
                }
                message = "——全文搜索写入服务重新加载已完成。——";
                OnMessages(message, null);
            }
            catch (Exception ex)
            {
                message = string.Format("——全文搜索写入服务重新加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion

            #region 搜索索引
            try
            {
                message = "……全文搜索查询服务重新启动中……。";
                OnMessages(message, null);
                if (searcher == null)
                {
                    simpleHTMLFormatter = new HighLight.SimpleHTMLFormatter("<span style='color:red'>", "</span>");
                    highlighter = new HighLight.AllDocHighLighter(simpleHTMLFormatter, tokenizer);

                    searcher = new IndexSearcher(new RAMDirectory(idxDoc), false);

                    message = "——全文搜索查询服务重新加载已完成。——";
                    OnMessages(message, null);
                }
            }
            catch (Exception ex)
            {
                message = string.Format("——全文搜索查询服务重新加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion             
                       
        }

        /// <summary>
        /// 重新加载全文索和敏感词索引
        /// </summary>
        public static void RebuildSearch()
        {
            //if (SensSearcher != null)
            //{
            //    SensSearcher.Dispose();
            //    SensSearcher = null;
            //}

            if (searcher != null)
            {
                searcher.Dispose();
                searcher = null;
            }

            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }

            //if (SensWriter != null)
            //{
            //    SensWriter.Dispose();
            //    SensWriter = null;
            //}

            if (idxDoc != null)
            {
                idxDoc.Dispose();
                idxDoc = null;
            }
            
            //if (idxSensDoc != null)
            //{
            //    idxSensDoc.Dispose();
            //    idxSensDoc = null;
            //}

            #region 加载全文索引

            string message = "";
            try
            {
                if (idxDoc == null)
                {
                    idxDoc = FSDirectory.Open(AllIndexDir);// new RAMDirectory(FSDirectory.Open(IndexDir));
                }
            }
            catch (Exception ex)
            {
                message = string.Format("——全文索引文件重新加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }


            #endregion

            #region 加载敏感词索引
            //string message = "";
            //try
            //{
            //    if (idxSensDoc == null)
            //    {
            //        idxSensDoc = FSDirectory.Open(SensIndexDir);// new RAMDirectory(FSDirectory.Open(IndexDir));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词索引文件重新加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            #endregion

            #region 写敏感词索引
            //try
            //{
            //    message = "……敏感词搜索写入服务重新启动中……。";
            //    OnMessages(message, null);
            //    if (SensWriter == null)
            //    {
            //        SensWriter = new IndexWriter(idxSensDoc, Sensanalyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
            //    }
            //    message = "——敏感词搜索写入服务重新加载已完成。——";
            //    OnMessages(message, null);
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词搜索写入服务重新加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            #endregion

            #region 写索引
            try
            {
                message = "……全文搜索写入服务重新启动中……。";
                OnMessages(message, null);
                if (writer == null)
                {
                    writer = new IndexWriter(idxDoc, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
                }
                message = "——全文搜索写入服务重新加载已完成。——";
                OnMessages(message, null);
            }
            catch (Exception ex)
            {
                message = string.Format("——全文搜索写入服务重新加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion

            #region  敏感词搜索索引
            //try
            //{
            //    message = "……敏感词搜索查询服务重新启动中……。";
            //    OnMessages(message, null);
            //    if (SensSearcher == null)
            //    {
            //        SenssimpleHTMLFormatter = new HighLight.SimpleHTMLFormatter("<span style='color:red'>", "</span>");
            //        Senshighlighter = new HighLight.SensHighLighter(SenssimpleHTMLFormatter, Senstokenizer);

            //        SensSearcher = new IndexSearcher(new RAMDirectory(idxSensDoc), false);

            //        message = "——敏感词搜索查询服务重新加载已完成。——";
            //        OnMessages(message, null);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词搜索查询服务重新加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            #endregion          
            
            #region 搜索索引
            try
            {
                message = "……全文搜索查询服务重新启动中……。";
                OnMessages(message, null);
                if (searcher == null)
                {
                    simpleHTMLFormatter = new HighLight.SimpleHTMLFormatter("<span style='color:red'>", "</span>");
                    highlighter = new HighLight.AllDocHighLighter(simpleHTMLFormatter, tokenizer);

                    searcher = new IndexSearcher(new RAMDirectory(idxDoc), false);

                    message = "——全文搜索查询服务重新加载已完成。——";
                    OnMessages(message, null);
                }
            }
            catch (Exception ex)
            {
                message = string.Format("——全文搜索查询服务重新加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion

        }

        /// <summary>
        /// 重新加载全文索和敏感词索引
        /// </summary>
        public static void ReOpenSearch()
        {
            //if (SensSearcher != null)
            //{
            //    SensSearcher.Dispose();
            //    SensSearcher = null;
            //}

            if (searcher != null)
            {
                searcher.Dispose();
                searcher = null;
            }

            
            //if (idxDoc != null)
            //{
            //    idxDoc.Dispose();
            //    idxDoc = null;
            //}

            //if (idxSensDoc != null)
            //{
            //    idxSensDoc.Dispose();
            //    idxSensDoc = null;
            //}

            //#region 加载全文索引

            //string message = "";
            //try
            //{
            //    if (idxDoc == null)
            //    {
            //        idxDoc = FSDirectory.Open(AllIndexDir);// new RAMDirectory(FSDirectory.Open(IndexDir));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——全文索引文件重新加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}


            //#endregion

            //#region 加载敏感词索引
            ////string message = "";
            //try
            //{
            //    if (idxSensDoc == null)
            //    {
            //        idxSensDoc = FSDirectory.Open(SensIndexDir);// new RAMDirectory(FSDirectory.Open(IndexDir));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词索引文件重新加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            //#endregion                      
            string message = "";
            #region  敏感词搜索索引
            //try
            //{
            //    message = "……敏感词搜索查询服务重新启动中……。";
            //    OnMessages(message, null);
            //    if (SensSearcher == null)
            //    {
            //        SenssimpleHTMLFormatter = new HighLight.SimpleHTMLFormatter("<span style='color:red'>", "</span>");
            //        Senshighlighter = new HighLight.SensHighLighter(SenssimpleHTMLFormatter, Senstokenizer);

            //        SensSearcher = new IndexSearcher(new RAMDirectory(idxSensDoc), false);

            //        message = "——敏感词搜索查询服务重新加载已完成。——";
            //        OnMessages(message, null);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    message = string.Format("——敏感词搜索查询服务重新加载失败。失败原因：{0}——", ex.Message);
            //    OnMessages(message, null);
            //}
            #endregion

            #region 搜索索引
            try
            {
                message = "……全文搜索查询服务重新启动中……。";
                OnMessages(message, null);
                if (searcher == null)
                {
                    simpleHTMLFormatter = new HighLight.SimpleHTMLFormatter("<span style='color:red'>", "</span>");
                    highlighter = new HighLight.AllDocHighLighter(simpleHTMLFormatter, tokenizer);

                    searcher = new IndexSearcher(new RAMDirectory(idxDoc), false);

                    message = "——全文搜索查询服务重新加载已完成。——";
                    OnMessages(message, null);
                }
            }
            catch (Exception ex)
            {
                message = string.Format("——全文搜索查询服务重新加载失败。失败原因：{0}——", ex.Message);
                OnMessages(message, null);
            }
            #endregion

        }
        #endregion 

        #region 写索引方法

        /// <summary>
        /// 给索引添加内容
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="title">标题</param>
        /// <param name="time">日期</param>
        /// <param name="content">内容</param>
        /// <returns>总数</returns>
        public static int IndexString(string url, string title, DateTime time, string content)
        {
            //
            Document doc = new Document();
            Field field = new Field("url", url, Field.Store.YES, Field.Index.NO);
            doc.Add(field);
            field = new Field("title", title, Field.Store.YES, Field.Index.ANALYZED);
            doc.Add(field);
            field = new Field("time", time.ToString("yyyyMMdd"), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(field);
            field = new Field("contents", content, Field.Store.YES, Field.Index.ANALYZED);
            doc.Add(field);
            //
            writer.AddDocument(doc);
            int numtmep = 0;
            int num = writer.GetDocCount(numtmep);
            return num;
        }

        /// <summary>
        /// 给全文索引索引添加内容
        /// </summary>
        /// <param name="news">索引信息</param>
        /// <returns>总数</returns>
        public static int AllIndexString(Infors news)
        {
            try
            {
                Document doc = InforToDocument(news);

                Term t = new Term("inforid", news.InformationID);

                writer.UpdateDocument(t, doc);//每次都更新索引库
                // writer.AddDocument(doc);

                #region 实时更新搜索
                IndexReader index = writer.GetReader();
                if (!index.IsCurrent())
                {
                    searcher.IndexReader.Reopen();
                }
                #endregion
            }
            catch (Exception ex)
            {

                string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                Logger.WriteError(funName, ex);
            }
            int numtmep = 0;
            int num = writer.GetDocCount(numtmep);
            return num;
        }

        /// <summary>
        /// 给敏感词索引添加内容
        /// </summary>
        /// <param name="news">索引信息</param>
        /// <returns>总数</returns>
        //public static int SensIndexString(Infors news)
        //{
        //    try
        //    {
        //        Document doc = InforToDocument(news);

        //        Term t = new Term("inforid", news.InformationID);

        //        SensWriter.UpdateDocument(t, doc);//每次都更新索引库
        //        // writer.AddDocument(doc);
        //    }
        //    catch (Exception ex)
        //    {

        //        string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        //        Logger.WriteError(funName, ex);
        //    }
        //    int numtmep = 0;
        //    int num = writer.GetDocCount(numtmep);
        //    return num;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infor"></param>
        /// <returns></returns>
        private static Document InforToDocument(Infors infor)
        {
            Document doc = new Document();
            try
            {
                //必填 信息ID
                Field field = new Field("inforid", infor.InformationID, Field.Store.YES, Field.Index.NOT_ANALYZED);
                doc.Add(field);

                //必填 类型ID
                field = new Field("typeid", infor.TypeID, Field.Store.YES, Field.Index.NOT_ANALYZED);
                doc.Add(field);

                //必填 标题
                field = new Field("title", infor.Title, Field.Store.YES, Field.Index.ANALYZED);
                doc.Add(field);

                //必填 内容
                field = new Field("contents", infor.Content, Field.Store.YES, Field.Index.ANALYZED);
                doc.Add(field);

                //必填 时间
                NumericField fieldNum = new NumericField("time", Field.Store.YES, true).SetIntValue(int.Parse(infor.Time.ToString("yyyyMMdd")));
                doc.Add(fieldNum);

                if (!string.IsNullOrWhiteSpace(infor.UserID))
                {
                    field = new Field("userid", infor.UserID, Field.Store.YES, Field.Index.NOT_ANALYZED);
                    doc.Add(field);
                }


                //必填 URL地址
                field = new Field("urls", infor.Urls, Field.Store.YES, Field.Index.NOT_ANALYZED);
                doc.Add(field);

                //发文单位ID 写入到收文单位属性中，方便查询；分隔符为
                StringBuilder RUnitID = new StringBuilder();
                StringBuilder SUnitID = new StringBuilder();
                //收文单位
                //if (infor.SUnitID != null)
                //{
                //    if (infor.SUnitID.Count > 0)
                //    {
                //        //StringBuilder RUnitID = new StringBuilder();
                //        foreach (string sunit in infor.SUnitID)
                //        {
                //            //string stemp = CommonFun.NumberToChinese(sunit);
                //            SUnitID.Append(sunit + "|");
                //        }
                //        //发文单位只有唯一一个
                //        field = new Field("sunitid", SUnitID.ToString(), Field.Store.YES, Field.Index.ANALYZED);
                //        doc.Add(field);
                //        //RUnitID.Append(infor.SUnitID + ";");
                //    }

                //}

                //收文单位
                //if (infor.RUnitID != null)
                //{
                //    if (infor.RUnitID.Count > 0)
                //    {
                //        //StringBuilder RUnitID = new StringBuilder();
                //        foreach (string sunit in infor.RUnitID)
                //        {
                //            //string stemp = CommonFun.NumberToChinese(sunit);
                //            RUnitID.Append(sunit + "|");
                //        }
                //        field = new Field("runitid", RUnitID.ToString(), Field.Store.YES, Field.Index.ANALYZED);
                //        doc.Add(field);
                //    }
                //}

                //if (!string.IsNullOrWhiteSpace(infor.Participle))
                //{
                //    field = new Field("participle", infor.Participle, Field.Store.YES, Field.Index.NOT_ANALYZED);
                //    doc.Add(field);
                //}

                //if (infor.Advise != null)
                //{
                //    if (infor.Advise.Count > 0)
                //    {
                //        string advise = string.Join("|", infor.Advise.ToArray());
                //        field = new Field("advise", advise, Field.Store.YES, Field.Index.NOT_ANALYZED);
                //        doc.Add(field);
                //    }
                //}

                if (!string.IsNullOrWhiteSpace(infor.Summary))
                {
                    field = new Field("summary", infor.Summary, Field.Store.YES, Field.Index.NOT_ANALYZED);
                    doc.Add(field);
                }

                if (!string.IsNullOrWhiteSpace(infor.DocNum))
                {
                    field = new Field("docnum", infor.DocNum, Field.Store.YES, Field.Index.NOT_ANALYZED);
                    doc.Add(field);
                }

            }
            catch (Exception ex)
            {
                string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                Logger.WriteError(funName, ex);
            }
            return doc;
        }

        /// <summary>
        /// 不优化，关闭索引
        /// </summary>
        public static void CloseWriterWithoutOptimize()
        {
            //writer.Commit();
            writer.Dispose();
            //SensWriter.Dispose();
        }

        /// <summary>
        /// 提交信息并关闭索引
        /// </summary>
        public static void CloseWriter()
        {
            if (writer != null)
            {
                //全文索引
                writer.Optimize();
                writer.Commit();
                //writer.Close();
                //writer.Commit();


                tokenizer.Dispose();
                writer.Dispose();
            }

            //if (SensWriter != null)
            //{
            //    //敏感词
            //    SensWriter.Optimize();
            //    SensWriter.Commit();
            //    //敏感词
            //    //SensWriter.Close();
            //    Senstokenizer.Dispose();
            //    SensWriter.Dispose();
            //}
        }
        /// <summary>
        /// 提交信息
        /// </summary>
        public static void CommitWriter()
        {
            //全文索引提交
            writer.Commit();

            //敏感词索引提交
            //SensWriter.Commit();
        }

        /// <summary>
        /// 提交全文索引数据信息
        /// </summary>
        public static void CommitAllWriter()
        {            
            writer.Commit();            
        }

        /// <summary>
        /// 提交敏感词索引数据信息
        /// </summary>
        public static void CommitSensWriter()
        {
            //SensWriter.Commit();
        }
        /// <summary>
        /// 优化信息并提交
        /// </summary>
        public static void OptimizeWriter()
        {
            //全文索引;
            writer.Optimize();
            writer.Commit();

            //敏感词
            //SensWriter.Optimize();
            //SensWriter.Commit();
        }

        /// <summary>
        /// 优化信息并提交
        /// </summary>
       
        #endregion

        #region 查索引方法




        /// <summary>
        /// 提交信息并关闭索引
        /// </summary>
        public static void CloseSearcher()
        {
            searcher.Dispose();
            //SensSearcher.Dispose();
        }


        /// <summary>
        /// 分离全文分词关键词，用空格隔开
        /// </summary>
        /// <param name="keywords">关键词</param>       
        /// <returns>关键词</returns>
        public static string GetKeyWordsSplitBySpace(string keywords)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                //分离关键词并列出所有关键词的词频与词性
                List<WordInfo> words = tokenizer.SegmentToWordInfos(keywords).ToList<WordInfo>();

                foreach (WordInfo word in words)
                {
                    if (word == null)
                    {
                        continue;
                    }

                    result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
                }
            }
            catch (Exception ex)
            {
                string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                Logger.WriteError(funName, ex);
            }
            return result.ToString().Trim();
        }

        /// <summary>
        /// 分离全文分词关键词，用“/”隔开
        /// </summary>
        /// <param name="keywords">关键词</param>
        /// <returns>关键词</returns>
        public static string GetKeyWordsSplitBySlash(string keywords)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                //分离关键词并列出所有关键词的词频与词性
                List<WordInfo> words = tokenizer.SegmentToWordInfos(keywords).ToList<WordInfo>();

                foreach (WordInfo word in words)
                {
                    if (word == null)
                    {
                        continue;
                    }

                    result.AppendFormat("{0}/", word.Word);
                }
            }
            catch (Exception ex)
            {
                string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                Logger.WriteError(funName, ex);
            }
            return result.ToString().Trim();
        }
        /// <summary>
        /// 分离全文分词关键词
        /// </summary>
        /// <param name="keywords">关键词</param>        
        /// <returns>关键词列表</returns>
        private static List<string> GetKeyWordsListSplitBySpace(string keywords)
        {
            //StringBuilder result = new StringBuilder();
            List<string> result = new List<string>();
            try
            {
                List<WordInfo> words = tokenizer.SegmentToWordInfos(keywords).ToList<WordInfo>();

                foreach (WordInfo word in words)
                {
                    if (word != null)
                    {
                        result.Add(word.Word);
                    }

                    //result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
                }
            }
            catch (Exception ex)
            {
                string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                Logger.WriteError(funName, ex);
            }
            return result;
        }

        /// <summary>
        /// 分离敏感词关键词，用“/”隔开
        /// </summary>
        /// <param name="keywords">关键词</param>
        /// <returns>关键词</returns>
        //public static string GetSensWordsSplitBySlash(string keywords)
        //{
        //    StringBuilder result = new StringBuilder();

        //    try
        //    {
        //        //分离关键词并列出所有关键词的词频与词性
        //        List<WordInfo> words = Senstokenizer.SegmentToWordInfos(keywords).ToList<WordInfo>();

        //        foreach (WordInfo word in words)
        //        {
        //            if (word == null)
        //            {
        //                continue;
        //            }

        //            result.AppendFormat("{0}/", word.Word);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        //        Logger.WriteError(funName, ex);
        //    }
        //    return result.ToString().Trim();
        //}

        /// <summary>
        /// 分离敏感词关键词
        /// </summary>
        /// <param name="keywords">关键词</param>        
        /// <returns>关键词列表</returns>
        //private static List<string> GetSensWordsListSplitBySpace(string keywords)
        //{
        //    List<string> result = new List<string>();
        //    try
        //    {
        //        List<WordInfo> words = Senstokenizer.SegmentToWordInfos(keywords).ToList<WordInfo>();

        //        foreach (WordInfo word in words)
        //        {
        //            if (word != null)
        //            {
        //                result.Add(word.Word);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        //        Logger.WriteError(funName, ex);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 从输入的字符串中，获取所有敏感词
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>分词结果集</returns>
        public static string GetSensSegementByInput(string input)
        {

            List<string> contextSeg = GetKeyWordsListSplitBySpace(input);
            List<string> ResultList = new List<string>();
            StringBuilder Result = new StringBuilder();
            //if (SensDicList == null)
            //{
            //    //获取敏感词
            //    SensDicList = new List<string>();

            //    foreach (char key in SensDic._FirstCharDict.Keys)
            //    {
            //        WordAttribute wab = SensDic._FirstCharDict[key];
            //        SensDicList.Add(wab.Word);
            //    }
            //    foreach (uint key in SensDic._DoubleCharDict.Keys)
            //    {
            //        WordAttribute wab = SensDic._DoubleCharDict[key];
            //        SensDicList.Add(wab.Word);
            //    }
            //    foreach (string key in SensDic._WordDict.Keys)
            //    {
            //        WordAttribute wab = SensDic._WordDict[key];
            //        SensDicList.Add(wab.Word);
            //    }

            //}

            //if (contextSeg.Count > 0)
            //{

            //    for (int i = 0; i < contextSeg.Count; i++)
            //    {
            //        string ds = contextSeg[i];
            //        if (SensDicList.Contains(ds) && !ResultList.Contains(ds))//Result.ToString().IndexOf(ds) < 0)
            //        {
            //            ResultList.Add(ds);

            //            //Result.Append(ds + ",");
            //        }
            //    }
            //}

            //
            string strresult = "";
            //if (ResultList.Count > 0)
            //{
            //    ResultList.Sort(new ListSensCompare());
            //    for (int i = 0; i < ResultList.Count; i++)
            //    {
            //        if (Result.ToString().IndexOf(ResultList[i]) < 0)
            //        {
            //            Result.Append(ResultList[i] + ",");
            //        }
            //    }
            //    strresult = Result.ToString();
            //    strresult = strresult.Remove(strresult.Length - 1, 1);
            //}
            return strresult;
        }
        /// <summary>
        /// 根据输入的词，搜索含有该词的信息
        /// </summary>
        /// <param name="q">输入的词</param>
        /// <param name="pageLen">每页长度</param>
        /// <param name="pageNo">当前页码</param>
        /// <param name="recCount">结果总数</param>
        /// <returns>信息列表</returns>
        public static List<Infors> KeySearch(string q, string n, string stime, string etime, string inforid, string typeid, string userid, string unitid, int pageLen, int pageNo, out int recCount)
        {
            string keywords = q;
            string nonewords = n;
            DateTime inputstarttime;
            DateTime inputendtime;
            try
            {
                inputstarttime = DateTime.ParseExact(stime, "yyyy-MM-dd HH:mm:ss", null);
                inputendtime = DateTime.ParseExact(etime, "yyyy-MM-dd HH:mm:ss", null);
            }
            catch
            {
                inputstarttime = DateTime.Now.AddYears(-100);
                inputendtime = System.DateTime.Now;
            }
            string[] strstart = inputstarttime.ToString("yyyy/MM/dd HH:mm:ss").Split('/', ' ', ':');
            long star = Convert.ToInt64(strstart[0] + strstart[1] + strstart[2]);
            string[] strsend = inputendtime.ToString("yyyy/MM/dd HH:mm:ss").Split('/', ' ', ':');
            long end = Convert.ToInt64(strsend[0] + strsend[1] + strsend[2]);
            IndexSearcher[] searchers = ConvertToPath(strstart[0], strsend[0]);
            MultiSearcher multisearcher = new MultiSearcher(searchers);
            //int sss = int.Parse(strsend[0]);
            recCount = 0;
            List<Infors> result = new List<Infors>();
            int hm = pageLen * pageNo;
            TopScoreDocCollector res = TopScoreDocCollector.Create(hm, false);
            try
            {
                #region 多字段查询
                //PerFieldAnalyzerWrapper wrapper = new PerFieldAnalyzerWrapper(analyzer);
                //wrapper.AddAnalyzer("title", analyzer);
                ////wrapper.AddAnalyzer("Author", analyzer);
                //wrapper.AddAnalyzer("content", analyzer);
                //string[] fields = { "title", "content" };

                //QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, wrapper);
                //Query query = parser.Parse(keywords);
                //

                #endregion
                #region 多条件控制
                List<string> strlist = GetKeyWordsListSplitBySpace(q);
                QueryParser queryParsers = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "contents", analyzer);
                BooleanQuery blq = new BooleanQuery();
                if (strlist.Count > 0)
                {
                    for (int i = 0; i < strlist.Count; i++)
                    {
                        Query queryflag = queryParsers.Parse(strlist[i]);
                        blq.Add(queryflag, Occur.MUST);
                    }
                }
                if (star < end && star > 0)
                {
                    Query numericrq = NumericRangeQuery.NewLongRange("time", star, end, true, true);
                    blq.Add(numericrq, Occur.MUST);
                }
                if (!string.IsNullOrEmpty(nonewords))
                {
                    Query qnflag = queryParsers.Parse(nonewords);
                    blq.Add(qnflag, Occur.MUST_NOT);//非运算
                }
                if (!string.IsNullOrEmpty(typeid))
                {
                    Query query1 = new TermQuery(new Term("typeid", typeid));
                    blq.Add(query1, Occur.MUST);
                }
                if (!string.IsNullOrEmpty(inforid))
                {
                    Query query2 = new TermQuery(new Term("informationid", inforid));
                    blq.Add(query2, Occur.MUST);
                }
                if (!string.IsNullOrEmpty(userid))
                {
                    Query query3 = new TermQuery(new Term("userid", userid));
                    blq.Add(query3, Occur.MUST);
                }
                if (!string.IsNullOrEmpty(unitid))
                {
                    Query query4 = new TermQuery(new Term("unitid", unitid));
                    blq.Add(query4, Occur.MUST);
                }
                multisearcher.Search(blq, res);
                #endregion
                #region 多词与或非
                //if (!string.IsNullOrEmpty(nonewords))
                //{
                //    List<string> list = GetKeyWordsListSplitBySpace(q, tokenizer);
                //    QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "contents", analyzer);
                //    BooleanQuery bq = new BooleanQuery();
                //    if (list.Count > 0)
                //    {
                //        for (int i = 0; i < list.Count; i++)
                //        {
                //            Query qflag = queryParser.Parse(list[i]);
                //            bq.Add(qflag, Occur.MUST);//与运算  
                //        }
                //    }
                //    if (star < end && star > 0)
                //    {
                //        Query numericrq = NumericRangeQuery.NewLongRange("time", star, end, true, true);
                //        bq.Add(numericrq, Occur.MUST);
                //    }
                //    Query qnflag = queryParser.Parse(nonewords);
                //    bq.Add(qnflag, Occur.MUST_NOT);//非运算
                //    search.Search(bq, res);
                //}
                //else
                //{
                //    //拆分关键词
                //    q = GetKeyWordsSplitBySpace(q, tokenizer);
                //    //初始化查询
                //    QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "contents", analyzer);
                //    //为关键词配置区域（contents）
                //    BooleanQuery bq = new BooleanQuery();
                //    Query query = queryParser.Parse(q);
                //    bq.Add(query, Occur.MUST);
                //    if (star < end && star > 0)
                //    {
                //        Query numericrq = NumericRangeQuery.NewLongRange("time", star, end, true, true);
                //        bq.Add(numericrq, Occur.MUST);
                //    }
                //    //通过调用Search方法搜索其出现在文件的编号与出现频率
                //    search.Search(bq, res);
                //}
                #endregion


                //前（每页放置容量）项数据
                TopDocs tds = res.TopDocs(pageLen * (pageNo - 1), pageLen);

                //搜索结果总量
                recCount = res.TotalHits;
                //int i = (pageNo - 1) * pageLen;

                //while (i < recCount && result.Count < pageLen)
                //{
                ScoreDoc[] sd = tds.ScoreDocs;
                for (int i = 0; i < sd.Length; i++)
                {
                    Infors news = null;

                    try
                    {
                        news = new Infors();
                        //查询文件
                        Document doc = multisearcher.Doc(sd[i].Doc);
                        news.Title = doc.Get("title");
                        news.Content = doc.Get("contents");
                        //news.Url = doc.Get("url");
                        string strTime = doc.Get("time");
                        news.Time = DateTime.ParseExact(strTime, "yyyyMMdd", null);
                        //news.DocPath = doc.Get("docpath");
                        //news.TifPath = doc.Get("tifpath");
                        //news.Length = Convert.ToInt32(doc.Get("length"));
                        news.Urls = doc.Get("urls");
                        news.InformationID = doc.Get("informationid");
                        news.TypeID = doc.Get("typeid");
                        news.UserID = doc.Get("userid");
                        //news.UnitID = doc.Get("unitid");
                        //文件摘要长度限制（来自配置文件）
                        int summerySize = SearchAllResultLength;
                        if (news.Content.Length < summerySize)
                        {
                            summerySize = news.Content.Length;
                        }

                        highlighter.FragmentSize = summerySize;
                        //全文中将关键词标红显示
                        news.Summary = highlighter.GetBestFragment(keywords, news.Content);
                        result.Add(news);
                    }
                    catch (Exception e)
                    {
                        string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                        Logger.WriteError(funName, e);
                        //Log.LogHandle.LogError("关键词转换失败！", "SearchService.Common.Index.AllSearch.KeySearch1", e);
                    }
                }
            }
            catch (Exception ex)
            {
                string funName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
                Logger.WriteError(funName, ex);
                //Log.LogHandle.LogError("关键词搜索失败！", "SearchService.Common.Index.AllSearch.KeySearch", ex);
            }
            //search.Dispose();
            return result;
        }
        
        /// <summary>
        /// 根据输入的参数，查询全文索引
        /// </summary>
        /// <param name="allSearchParam">参数</param>
        /// <param name="recCount">总记录数</param>
        /// <returns>信息列表</returns>
        public static List<Infors> AllSearchByInput(AllSearchParam allSearchParam, out int recCount)
        {

            recCount = 0;
            List<Infors> result = new List<Infors>();

            string keywords = allSearchParam.InPutWord;
            string nonewords = allSearchParam.NoneWord;

            #region 时间
            bool timeFlag = false;
            long star = 0;
            long end = 0;

            string starYear = "";
            string endYear = "";

            if ((allSearchParam.StartTime == DateTime.MaxValue && allSearchParam.EndTime == DateTime.MaxValue)
                || (allSearchParam.StartTime == DateTime.MinValue && allSearchParam.EndTime == DateTime.MinValue))
            {
                timeFlag = false;
            }
            else
            {               
                star = long.Parse(allSearchParam.StartTime.ToString("yyyyMMddHHmmss"));
                starYear = allSearchParam.StartTime.ToString("yyyy");
                end = long.Parse(allSearchParam.EndTime.ToString("yyyyMMddHHmmss"));
                endYear = allSearchParam.EndTime.ToString("yyyy");

                if (end > star)
                {
                    timeFlag = true;
                }
            }

            //if (allSearchParam.StartTime != DateTime.MinValue && allSearchParam.EndTime != DateTime.MinValue)
            //{
            //    if (allSearchParam.EndTime < allSearchParam.StartTime)
            //    {
            //        //
            //        recCount = 0;
            //        return null;
            //    }
            //    else
            //    {
            //        star = long.Parse(allSearchParam.StartTime.ToString("yyyyMMddHHmmss"));
            //        starYear = allSearchParam.StartTime.ToString("yyyy");
            //        end = long.Parse(allSearchParam.EndTime.ToString("yyyyMMddHHmmss"));
            //        endYear = allSearchParam.EndTime.ToString("yyyy");
            //    }
            //}
            //else if (allSearchParam.StartTime == DateTime.MinValue && allSearchParam.EndTime != DateTime.MinValue)
            //{
            //    star = long.Parse(allSearchParam.EndTime.AddDays(-7).ToString("yyyyMMddHHmmss"));
            //    end = long.Parse(allSearchParam.EndTime.ToString("yyyyMMddHHmmss"));
            //    starYear = allSearchParam.EndTime.AddDays(-7).ToString("yyyy");
            //    endYear = allSearchParam.EndTime.ToString("yyyy");
            //}
            //else if (allSearchParam.StartTime != DateTime.MinValue && allSearchParam.EndTime == DateTime.MinValue)
            //{
            //    star = long.Parse(allSearchParam.StartTime.ToString("yyyyMMddHHmmss"));
            //    end = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //    starYear = allSearchParam.StartTime.ToString("yyyy");
            //    endYear = DateTime.Now.ToString("yyyy");
            //}
            //else
            //{
            //    star = long.Parse(DateTime.Now.AddDays(-7).ToString("yyyyMMddHHmmss"));
            //    end = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

            //    starYear = DateTime.Now.AddDays(-7).ToString("yyyy");
            //    endYear = DateTime.Now.ToString("yyyy");
            //}
            #endregion

            //加载多索引文件
            //IndexSearcher[] searchers = ConvertToPath(starYear, endYear);
            //MultiSearcher multisearcher = new MultiSearcher(searchers);
               
            try
            {
                #region 多字段查询
                //PerFieldAnalyzerWrapper wrapper = new PerFieldAnalyzerWrapper(analyzer);
                //wrapper.AddAnalyzer("title", analyzer);
                ////wrapper.AddAnalyzer("Author", analyzer);
                //wrapper.AddAnalyzer("content", analyzer);
                //string[] fields = { "title", "content" };

                //QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, wrapper);
                //Query query = parser.Parse(keywords);
                //

                #endregion
                #region 多条件控制

                BooleanQuery blq = new BooleanQuery();
                
                #region 单位
                //收文单位
                //List<string> RSUnitID = new List<string>();
                ////发文单位
                //List<string> SSUnitID = new List<string>();

                ////添加涉及单位 涉及单位：发送单位，接收单位
                //if (allSearchParam.SUnitID != null)
                //{
                //    SSUnitID.AddRange(allSearchParam.SUnitID);
                //}
                //if (allSearchParam.RUnitID != null)
                //{

                //    RSUnitID.AddRange(allSearchParam.RUnitID);
                //}
                //if (RSUnitID != null)
                //{
                //    if (RSUnitID.Count > 0)
                //    {
                //        //单位ID Query
                //        BooleanQuery blqrRUnitID = new BooleanQuery();

                //        QueryParser queryParsersRange = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "runitid", analyzer);
                //        //queryParsersRange.DefaultOperator = QueryParser.Operator.OR;
                //        foreach (string runitid in RSUnitID)
                //        {
                //            Query contentQuery = queryParsersRange.Parse(runitid);
                //            blqrRUnitID.Add(contentQuery, Occur.SHOULD);//或运算
                //        }
                //        //设置满足或运算的最少个数
                //        blqrRUnitID.MinimumNumberShouldMatch = 1;
                //        //
                //        blq.Add(blqrRUnitID, Occur.MUST);
                //    }
                //}

                //if (SSUnitID != null)
                //{
                //    if (SSUnitID.Count > 0)
                //    {
                //        //单位ID Query
                //        BooleanQuery blqSUnitID = new BooleanQuery();

                //        QueryParser queryParsersRange = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "sunitid", analyzer);
                //        //queryParsersRange.DefaultOperator = QueryParser.Operator.OR;
                //        foreach (string runitid in RSUnitID)
                //        {
                //            Query contentQuery = queryParsersRange.Parse(runitid);
                //            blqSUnitID.Add(contentQuery, Occur.SHOULD);//或运算
                //        }
                //        //设置满足或运算的最少个数
                //        blqSUnitID.MinimumNumberShouldMatch = 1;
                //        //
                //        blq.Add(blqSUnitID, Occur.MUST);
                //    }
                //}
                
                #endregion

                #region 关键词
                List<string> strlist = GetKeyWordsListSplitBySpace(keywords);
               // List<string> files = new List<string> { "contents", "title" };
                if (allSearchParam.Range == 0)
                {
                    allSearchParam.Range = SearchRange.TitleAndContext;
                }

                //关键词 Query
                BooleanQuery blqKeyWords= new BooleanQuery();
                QueryParser queryParsers;
                if (allSearchParam.Range == SearchRange.Title)
                {
                    queryParsers = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "title", analyzer);
                    //queryParsers.DefaultOperator = QueryParser.Operator.AND;
                    if (strlist.Count > 0)
                    {
                       
                        for (int i = 0; i < strlist.Count; i++)
                        {
                            Query queryflag = queryParsers.Parse(strlist[i]);
                            blqKeyWords.Add(queryflag, Occur.MUST);
                        }
                        blq.Add(blqKeyWords, Occur.MUST);
                    }
                }
                else
                {
                    queryParsers = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "contents", analyzer);
                    //queryParsers.DefaultOperator = QueryParser.Operator.AND;
                    if (strlist.Count > 0)
                    {
                        for (int i = 0; i < strlist.Count; i++)
                        {
                            Query queryflag = queryParsers.Parse(strlist[i]);
                            blqKeyWords.Add(queryflag, Occur.MUST);// Occur.SHOULD);
                        }
                        ////设置满足或运算的最少个数
                        //blq.MinimumNumberShouldMatch = 1;
                        blq.Add(blqKeyWords, Occur.MUST);
                    }
                }

               
                //关键词中，需排除的词
                //if (!string.IsNullOrEmpty(nonewords))
                //{
                //    Query qnflag = queryParsers.Parse(nonewords);
                //    blq.Add(qnflag, Occur.MUST_NOT);//非运算
                //}
                #endregion

                ////时间范围
                //if (timeFlag)
                //{
                //    Query numericrq = NumericRangeQuery.NewLongRange("time", star, end, true, true);
                //    blq.Add(numericrq, Occur.MUST);
                //}
                ////类型
                //if (!string.IsNullOrEmpty(allSearchParam.TypeID))
                //{
                //    Query query = new TermQuery(new Term("typeid", allSearchParam.TypeID));
                //    blq.Add(query, Occur.MUST);
                //}
                ////文件标识
                //if (!string.IsNullOrEmpty(allSearchParam.InformationID))
                //{
                //    Query query = new TermQuery(new Term("inforid", allSearchParam.InformationID));
                //    blq.Add(query, Occur.MUST);
                //}
                
                ////文档变化
                //if(!string.IsNullOrWhiteSpace(allSearchParam.DocNum))
                //{
                //    Query query = new TermQuery(new Term("docnum", allSearchParam.DocNum));
                //    blq.Add(query, Occur.MUST);
                //}

                //查询总数
                int hm = allSearchParam.PageNo * allSearchParam.PageSize;
                TopScoreDocCollector res = TopScoreDocCollector.Create(hm, false);

                searcher.Search(blq, res);
                //multisearcher.Search(blq, res);
                #endregion
               
                //前（每页放置容量）项数据
                TopDocs tds = res.TopDocs(allSearchParam.PageSize * (allSearchParam.PageNo - 1), allSearchParam.PageSize);

                //搜索结果总量
                recCount = res.TotalHits;
                //int i = (pageNo - 1) * pageLen;

                //while (i < recCount && result.Count < pageLen)
                //{
                ScoreDoc[] sd = tds.ScoreDocs;
                for (int i = 0; i < sd.Length; i++)
                {
                    Infors news = null;

                    try
                    {

                        //"inforid","typeid","title","contents","time","userid","sunitid","runitid","participle","advise","summary"

                        news = new Infors();
                        //查询文件
                        Document doc = searcher.Doc(sd[i].Doc);

                        news.InformationID = doc.Get("inforid");
                        news.TypeID = doc.Get("typeid");
                        news.Title = doc.Get("title");
                        news.Content = doc.Get("contents");
                        news.Urls = doc.Get("urls");
                        string strTime = doc.Get("time");
                        news.Time = DateTime.ParseExact(strTime, "yyyyMMdd", null);

                        news.UserID = doc.Get("userid");
                        //news.SUnitID = doc.Get("sunitid");
                       //string sunitid= doc.Get("sunitid");
                        
                       // if (sunitid.IndexOf(';') > 0 && sunitid.IndexOf('|') > 0)
                       // {
                       //     if (sunitid.EndsWith("|"))
                       //     {
                       //         sunitid = sunitid.Substring(0, sunitid.Length - 1);
                       //     }
                       //     char[] splitFlag = { ';', '|' }; ;
                       //     List<string> SUnitid = sunitid.Split(splitFlag).ToList<string>();

                       //     //RUnitid.Remove(news.SUnitID);
                       //     news.SUnitID = SUnitid;
                       // }


                       // string runitid = doc.Get("runitid");

                       // if (runitid.IndexOf(';') > 0 && runitid.IndexOf('|')>0)
                       // {
                       //     if (runitid.EndsWith("|"))
                       //     {
                       //         runitid = runitid.Substring(0, runitid.Length - 1);
                       //     }
                       //     char[] splitFlag = { ';', '|' }; ;
                       //     List<string> RSUnitid = runitid.Split(splitFlag).ToList<string>();

                       //     //RSUnitid.Remove(news.SUnitID);
                       //     news.RUnitID = RSUnitid;
                       // }

                       
                        news.Participle = doc.Get("participle");

                        //string advise = doc.Get("advise");
                        //if (advise != null)
                        //{
                        //    news.Advise = doc.Get("advise").Split('|').ToList<string>();
                        //}

                        //文件摘要长度限制（来自配置文件）
                        int summerySize = SearchAllResultLength;
                        if (news.Content.Length < summerySize)
                        {
                            summerySize = news.Content.Length;
                        }

                        highlighter.FragmentSize = summerySize;
                        //全文中将关键词标红显示
                        news.Summary = highlighter.GetBestFragment(keywords, news.Content);
                        result.Add(news);
                    }
                    catch (Exception e)
                    {
                        Logger.WriteError("关键词转换失败！ SearchService.Common.Index.AllSearch.KeySearch1", e);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError("关键词搜索失败！ SearchService.Common.Index.AllSearch.KeySearch", ex);
            }
            //search.Dispose();
            return result;
        }

        /// <summary>
        /// 根据输入的参数，查询敏感词索引
        /// </summary>
        /// <param name="allSearchParam">参数</param>
        /// <param name="recCount">总记录数</param>
        /// <returns>信息列表</returns>
        //public static List<Infors> SensSearchByInput(AllSearchParam allSearchParam, out int recCount)
        //{

        //    recCount = 0;
        //    List<Infors> result = new List<Infors>();

        //    string keywords = allSearchParam.InPutWord;
        //    string nonewords = allSearchParam.NoneWord;

        //    #region 时间
        //    bool timeFlag = false;
        //    long star = 0;
        //    long end = 0;

        //    string starYear = "";
        //    string endYear = "";

        //    if ((allSearchParam.StartTime == DateTime.MaxValue && allSearchParam.EndTime == DateTime.MaxValue)
        //        || (allSearchParam.StartTime == DateTime.MinValue && allSearchParam.EndTime == DateTime.MinValue))
        //    {
        //        timeFlag = false;
        //    }
        //    else
        //    {
        //        star = long.Parse(allSearchParam.StartTime.ToString("yyyyMMddHHmmss"));
        //        starYear = allSearchParam.StartTime.ToString("yyyy");
        //        end = long.Parse(allSearchParam.EndTime.ToString("yyyyMMddHHmmss"));
        //        endYear = allSearchParam.EndTime.ToString("yyyy");

        //        if (end > star)
        //        {
        //            timeFlag = true;
        //        }
        //    }

        //    //if (allSearchParam.StartTime != DateTime.MinValue && allSearchParam.EndTime != DateTime.MinValue)
        //    //{
        //    //    if (allSearchParam.EndTime < allSearchParam.StartTime)
        //    //    {
        //    //        //
        //    //        recCount = 0;
        //    //        return null;
        //    //    }
        //    //    else
        //    //    {
        //    //        star = long.Parse(allSearchParam.StartTime.ToString("yyyyMMddHHmmss"));
        //    //        starYear = allSearchParam.StartTime.ToString("yyyy");
        //    //        end = long.Parse(allSearchParam.EndTime.ToString("yyyyMMddHHmmss"));
        //    //        endYear = allSearchParam.EndTime.ToString("yyyy");
        //    //    }
        //    //}
        //    //else if (allSearchParam.StartTime == DateTime.MinValue && allSearchParam.EndTime != DateTime.MinValue)
        //    //{
        //    //    star = long.Parse(allSearchParam.EndTime.AddDays(-7).ToString("yyyyMMddHHmmss"));
        //    //    end = long.Parse(allSearchParam.EndTime.ToString("yyyyMMddHHmmss"));
        //    //    starYear = allSearchParam.EndTime.AddDays(-7).ToString("yyyy");
        //    //    endYear = allSearchParam.EndTime.ToString("yyyy");
        //    //}
        //    //else if (allSearchParam.StartTime != DateTime.MinValue && allSearchParam.EndTime == DateTime.MinValue)
        //    //{
        //    //    star = long.Parse(allSearchParam.StartTime.ToString("yyyyMMddHHmmss"));
        //    //    end = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
        //    //    starYear = allSearchParam.StartTime.ToString("yyyy");
        //    //    endYear = DateTime.Now.ToString("yyyy");
        //    //}
        //    //else
        //    //{
        //    //    star = long.Parse(DateTime.Now.AddDays(-7).ToString("yyyyMMddHHmmss"));
        //    //    end = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

        //    //    starYear = DateTime.Now.AddDays(-7).ToString("yyyy");
        //    //    endYear = DateTime.Now.ToString("yyyy");
        //    //}
        //    #endregion

        //    //加载多索引文件
        //    //IndexSearcher[] searchers = ConvertToPath(starYear, endYear);
        //    //MultiSearcher multisearcher = new MultiSearcher(searchers);

        //    try
        //    {
        //        #region 多字段查询
        //        //PerFieldAnalyzerWrapper wrapper = new PerFieldAnalyzerWrapper(analyzer);
        //        //wrapper.AddAnalyzer("title", analyzer);
        //        ////wrapper.AddAnalyzer("Author", analyzer);
        //        //wrapper.AddAnalyzer("content", analyzer);
        //        //string[] fields = { "title", "content" };

        //        //QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, wrapper);
        //        //Query query = parser.Parse(keywords);
        //        //

        //        #endregion
        //        #region 多条件控制

        //        BooleanQuery blq = new BooleanQuery();

        //        #region 单位
        //        List<string> RSUnitID = new List<string>();
        //        //添加涉及单位 涉及单位：发送单位，接收单位
        //        if (allSearchParam.SUnitID != null)
        //        {
        //            RSUnitID.AddRange(allSearchParam.SUnitID);
        //        }
        //        if (allSearchParam.RUnitID != null)
        //        {

        //            RSUnitID.AddRange(allSearchParam.RUnitID);
        //        }
        //        if (RSUnitID != null)
        //        {
        //            if (RSUnitID.Count > 0)
        //            {
        //                QueryParser queryParsersRange = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "runitid", analyzer);
        //                foreach (string runitid in RSUnitID)
        //                {
        //                    Query contentQuery = queryParsersRange.Parse(runitid);
        //                    blq.Add(contentQuery, Occur.SHOULD);//或运算
        //                }
        //                //设置满足或运算的最少个数
        //                blq.MinimumNumberShouldMatch = 1;
        //            }
        //        }
        //        #endregion

        //        #region 关键词
        //        List<string> strlist = GetKeyWordsListSplitBySpace(keywords);
        //        // List<string> files = new List<string> { "contents", "title" };
        //        if (allSearchParam.Range == 0)
        //        {
        //            allSearchParam.Range = SearchRange.TitleAndContext;
        //        }

        //        QueryParser queryParsers;
        //        if (allSearchParam.Range == SearchRange.Title)
        //        {
        //            queryParsers = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "title", analyzer);
        //            if (strlist.Count > 0)
        //            {
        //                for (int i = 0; i < strlist.Count; i++)
        //                {
        //                    Query queryflag = queryParsers.Parse(strlist[i]);
        //                    blq.Add(queryflag, Occur.MUST);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            queryParsers = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "contents", analyzer);

        //            if (strlist.Count > 0)
        //            {
        //                for (int i = 0; i < strlist.Count; i++)
        //                {
        //                    Query queryflag = queryParsers.Parse(strlist[i]);
        //                    blq.Add(queryflag, Occur.MUST);
        //                }
        //            }
        //        }


        //        //关键词中，需排除的词
        //        if (!string.IsNullOrEmpty(nonewords))
        //        {
        //            Query qnflag = queryParsers.Parse(nonewords);
        //            blq.Add(qnflag, Occur.MUST_NOT);//非运算
        //        }
        //        #endregion

        //        //时间范围
        //        if (timeFlag)
        //        {
        //            Query numericrq = NumericRangeQuery.NewLongRange("time", star, end, true, true);
        //            blq.Add(numericrq, Occur.MUST);
        //        }
        //        //类型
        //        if (!string.IsNullOrEmpty(allSearchParam.TypeID))
        //        {
        //            Query query = new TermQuery(new Term("typeid", allSearchParam.TypeID));
        //            blq.Add(query, Occur.MUST);
        //        }
        //        //文件标识
        //        if (!string.IsNullOrEmpty(allSearchParam.InformationID))
        //        {
        //            Query query = new TermQuery(new Term("inforid", allSearchParam.InformationID));
        //            blq.Add(query, Occur.MUST);
        //        }

        //        //文档变化
        //        if (!string.IsNullOrWhiteSpace(allSearchParam.DocNum))
        //        {
        //            Query query = new TermQuery(new Term("docnum", allSearchParam.DocNum));
        //            blq.Add(query, Occur.MUST);
        //        }

        //        //查询总数
        //        int hm = allSearchParam.PageNo * allSearchParam.PageSize;
        //        TopScoreDocCollector res = TopScoreDocCollector.Create(hm, false);

        //       SensSearcher.Search(blq, res);
        //        //multisearcher.Search(blq, res);
        //        #endregion

        //        //前（每页放置容量）项数据
        //        TopDocs tds = res.TopDocs(allSearchParam.PageSize * (allSearchParam.PageNo - 1), allSearchParam.PageSize);

        //        //搜索结果总量
        //        recCount = res.TotalHits;
        //        //int i = (pageNo - 1) * pageLen;

        //        //while (i < recCount && result.Count < pageLen)
        //        //{
        //        ScoreDoc[] sd = tds.ScoreDocs;
        //        for (int i = 0; i < sd.Length; i++)
        //        {
        //            Infors news = null;

        //            try
        //            {

        //                //"inforid","typeid","title","contents","time","userid","sunitid","runitid","participle","advise","summary"

        //                news = new Infors();
        //                //查询文件
        //                Document doc = searcher.Doc(sd[i].Doc);

        //                news.InformationID = doc.Get("inforid");
        //                news.TypeID = doc.Get("typeid");
        //                news.Title = doc.Get("title");
        //                news.Content = doc.Get("contents");
        //                news.Urls = doc.Get("urls");

        //                string strTime = doc.Get("time");
        //                news.Time = DateTime.ParseExact(strTime, "yyyyMMddHHmmss", null);

        //                news.UserID = doc.Get("userid");
        //                //news.SUnitID = doc.Get("sunitid");

        //                //string sunitid = doc.Get("sunitid");
        //                //if (sunitid.IndexOf(';') > 0 && sunitid.IndexOf('|') > 0)
        //                //{
        //                //    if (sunitid.EndsWith("|"))
        //                //    {
        //                //        sunitid = sunitid.Substring(0, sunitid.Length - 1);
        //                //    }
        //                //    char[] splitFlag = { ';', '|' }; ;
        //                //    List<string> SUnitid = sunitid.Split(splitFlag).ToList<string>();

        //                //    //RUnitid.Remove(news.SUnitID);
        //                //    news.RUnitID = SUnitid;
        //                //}


        //                //string runitid = doc.Get("runitid");

        //                //if (runitid.IndexOf(';') > 0 && runitid.IndexOf('|') > 0)
        //                //{
        //                //    if (runitid.EndsWith("|"))
        //                //    {
        //                //        runitid = runitid.Substring(0, runitid.Length - 1);
        //                //    }
        //                //    char[] splitFlag = { ';', '|' }; ;
        //                //    List<string> RSUnitid = runitid.Split(splitFlag).ToList<string>();

        //                //    //RSUnitid.Remove(news.SUnitID);
        //                //    news.RUnitID = RSUnitid;
        //                //}


        //                news.Participle = doc.Get("participle");

        //                //string advise = doc.Get("advise");
        //                //if (advise != null)
        //                //{
        //                //    news.Advise = doc.Get("advise").Split('|').ToList<string>();
        //                //}

        //                //文件摘要长度限制（来自配置文件）
        //                int summerySize = SearchSensResultLength;
        //                if (news.Content.Length < summerySize)
        //                {
        //                    summerySize = news.Content.Length;
        //                }

        //                highlighter.FragmentSize = summerySize;
        //                //全文中将关键词标红显示
        //                news.Summary = highlighter.GetBestFragment(keywords, news.Content);
        //                result.Add(news);
        //            }
        //            catch (Exception e)
        //            {
        //                Logger.WriteError("关键词转换失败！ SearchService.Common.Index.AllSearch.KeySearch1", e);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteError("关键词搜索失败！ SearchService.Common.Index.AllSearch.KeySearch", ex);
        //    }
        //    //search.Dispose();
        //    return result;
        //}
        /// <summary>
        /// 输入两个数返回多个地址
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static IndexSearcher[] ConvertToPath(string aa, string bb)
        {
            int a = int.Parse(aa); int b = int.Parse(bb);
            int i;
            // IndexSearcher[] result = new IndexSearcher[i];
            List<string> Path = new List<string>();
            string paht;
            if (a > 2014)
            {
                i = b - a + 1;
                for (int j = a; j < a + i; j++)
                {
                    paht = AllIndexDir + "\\" + j;
                    Path.Add(paht);
                }
                IndexSearcher[] result = new IndexSearcher[i];
                for (int k = 0; k < i; k++)
                {
                    FSDirectory FS = FSDirectory.Open(new DirectoryInfo(Path[k]), new NoLockFactory());
                    result[k] = new IndexSearcher(FS);
                }
                return result;
            }
            else if (b > 2014)
            {
                i = b - 2013;
                for (int j = 2014; j < 2014 + i; j++)
                {
                    if (j == 2014)
                    {
                        paht = AllIndexDir + "\\" + "OldYear";
                        Path.Add(paht);
                    }
                    else
                    {
                        paht = AllIndexDir + "\\" + j;
                        Path.Add(paht);
                    }
                }
                IndexSearcher[] result = new IndexSearcher[i];
                for (int k = 0; k < i; k++)
                {
                    FSDirectory FS = FSDirectory.Open(new DirectoryInfo(Path[k]), new NoLockFactory());
                    result[k] = new IndexSearcher(FS);
                }
                return result;
            }
            else
            {
                i = 1;
                paht = AllIndexDir + "\\" + "OldYear";
                Path.Add(paht);
                IndexSearcher[] result = new IndexSearcher[i];
                FSDirectory FS = FSDirectory.Open(new DirectoryInfo(Path[0]), new NoLockFactory());
                result[0] = new IndexSearcher(FS);
                return result;
            }
        }
        #endregion

        #region 词典文件处理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicPath"></param>
        public static void SaveDic(string dicPath)
        {
            AllDocTokenizer.segment.SaveDic();
            SensTokenizer.segment.SaveDic();
            //Dic.Save(dicPath);
        }

        /// <summary>
        /// 添加词到全文词库
        /// </summary>
        /// <param name="word"></param>
        public static void InsertWordToAllDic(string word)
        {
            AllDocTokenizer.segment.WordDictionary.InsertWord(word, 0, (POS)0);
        }
        /// <summary>
        /// 添加词到敏感词词库
        /// </summary>
        /// <param name="word"></param>
        public static void InsertWordToSensDic(string word)
        {
            SensTokenizer.segment.WordDictionary.InsertWord(word, 0, (POS)0);
            AllDocTokenizer.segment.WordDictionary.InsertWord(word, 0, (POS)0);
        }

        /// <summary>
        /// 根据输入的词和返回数据总数，查询含有输入词的的的词集合
        /// </summary>
        /// <param name="input">输入的词</param>
        /// <param name="count">返回数量</param>
        /// <returns>字符串列表</returns>
        public static List<string> GetWordsByInputCount(string input, int count)
        {
            int selectCount = count;
            string message = input;
            List<string> ResultList = new List<string>();
            List<SearchWordResult> searchResult = AllDocTokenizer.segment.WordDictionary.Search(message);
            if (selectCount > 0)
            {
                if (searchResult.Count > selectCount)
                {
                    for (int i = 0; i < selectCount; i++)
                    {
                        ResultList.Add(searchResult[i].Word.Word);
                    }
                }
                else
                {
                    for (int i = 0; i < searchResult.Count; i++)
                    {
                        ResultList.Add(searchResult[i].Word.Word);
                    }
                }
            }
            else
            {
                ResultList = null;
            }
            return ResultList;
        }
        #endregion
    }
        /// <summary>
        /// 长度排序
        /// </summary>
    class ListSensCompare : IComparer<string>
    {
        public int Compare(string a, string b)
        {
            return b.Length.CompareTo(a.Length);
        }
    }
}
