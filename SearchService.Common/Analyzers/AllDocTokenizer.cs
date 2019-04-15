using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;

using Segment;

namespace SearchService.Common.Analyzers
{
    /// <summary>
    /// 描述：全文检索分词器
    /// 日期：2016年8月26日
    /// 作者：Sofia
    /// </summary>
    public class AllDocTokenizer : Tokenizer
    {
        private static object _LockObj = new object();
        private static bool _Inited = false;
        public static Segment.Segment segment = new Segment.Segment();

        private WordInfo[] _WordList;
        private int _Position = -1; //词汇在缓冲中的位置.
        private bool _OriginalResult = false;
        string _InputText;

        //private Segments.Dict.WordDictionary wordDictionary;

        //public Segments.Dict.WordDictionary WordDictionary
        //{
        //    get { return wordDictionary; }
        //    set { wordDictionary = value; }
        //}

        private ITermAttribute termAtt;
        private IOffsetAttribute offsetAtt;
        private IPositionIncrementAttribute posIncrAtt;
        private ITypeAttribute typeAtt;
        //termAtt = AddAttribute<ITermAttribute>();
        //    offsetAtt = AddAttribute<IOffsetAttribute>();
        //    posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
        //    typeAtt = AddAttribute<ITypeAttribute>();

        private string segmenterXMLConfigPath = "";//ConfigurationManager.AppSettings["AllWordsDicConfigPath"];
        /// <summary>
        /// 分词配置文件地址
        /// </summary>
        public string SegmenterXMLConfigPath
        {
            get { return segmenterXMLConfigPath; }
            set { segmenterXMLConfigPath = value; }
        }

        //~AllDocTokenizer()
        //{
        //    segment = new Segment();
        //}

        private void InitSegment()
        {

            //Init PanGu Segment.

            if (!_Inited)
            {
                //segment.
                segment.Init();

                _Inited = true;

                termAtt = AddAttribute<ITermAttribute>();
                offsetAtt = AddAttribute<IOffsetAttribute>();
                posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
                typeAtt = AddAttribute<ITypeAttribute>();
            }
        }

        /// <summary>
        /// Init PanGu Segment
        /// </summary>
        /// <param name="fileName">HBcomm.xml file path</param>
        public void InitSegment(string fileName)
        {
            lock (_LockObj)
            {
                //Init PanGu Segment.
                if (!_Inited)
                {
                    segment.Init(fileName);
                    _Inited = true;

                    termAtt = AddAttribute<ITermAttribute>();
                    offsetAtt = AddAttribute<IOffsetAttribute>();
                    posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
                    typeAtt = AddAttribute<ITypeAttribute>();
                }
            }
        }

        public AllDocTokenizer(System.IO.TextReader input, bool originalResult)
            : this(input)
        {
            _OriginalResult = originalResult;

            termAtt = AddAttribute<ITermAttribute>();
            offsetAtt = AddAttribute<IOffsetAttribute>();
            posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
            typeAtt = AddAttribute<ITypeAttribute>();
        }

        public AllDocTokenizer()
        {
            lock (_LockObj)
            {
                InitSegment();
            }
        }

        public AllDocTokenizer(string segmentConfigPath)
        {
            SegmenterXMLConfigPath = segmentConfigPath;
            lock (_LockObj)
            {
                InitSegment(segmentConfigPath);
            }
        }

        public AllDocTokenizer(System.IO.TextReader input)
            : base(input)
        {
            lock (_LockObj)
            {
                InitSegment();
            }

            _InputText = base.input.ReadToEnd();

            if (string.IsNullOrEmpty(_InputText))
            {
                char[] readBuf = new char[1024];

                int relCount = base.input.Read(readBuf, 0, readBuf.Length);

                StringBuilder inputStr = new StringBuilder(readBuf.Length);


                while (relCount > 0)
                {
                    inputStr.Append(readBuf, 0, relCount);

                    relCount = input.Read(readBuf, 0, readBuf.Length);
                }

                if (inputStr.Length > 0)
                {
                    _InputText = inputStr.ToString();
                }
            }

            if (string.IsNullOrEmpty(_InputText))
            {
                _WordList = new WordInfo[0];
            }
            else
            {
                // Segment segment = new Segment();
                ICollection<WordInfo> wordInfos = segment.DoSegment(_InputText);
                _WordList = new WordInfo[wordInfos.Count];
                wordInfos.CopyTo(_WordList, 0);
            }
        }



        #region 扩展方法
        public override bool IncrementToken()
        {
            ClearAttributes();
            Token word = Next();
            if (word != null)
            {
                termAtt.SetTermBuffer(word.Term);
                offsetAtt.SetOffset(word.StartOffset, word.EndOffset);
                typeAtt.Type = word.Type;
                return true;
            }
            End();
            return false;
        }
        //DotLucene的分词器简单来说，就是实现Tokenizer的Next方法，把分解出来的每一个词构造为一个Token，因为Token是DotLucene分词的基本单位。
        public Token Next()
        {
            if (_OriginalResult)
            {
                string retStr = _InputText;

                _InputText = null;

                if (retStr == null)
                {
                    return null;
                }

                return new Token(retStr, 0, retStr.Length);
            }

            int length = 0;    //词汇的长度.
            int start = 0;     //开始偏移量.

            while (true)
            {
                _Position++;
                if (_Position < _WordList.Length)
                {
                    if (_WordList[_Position] != null)
                    {
                        length = _WordList[_Position].Word.Length;
                        start = _WordList[_Position].Position;
                        return new Token(_WordList[_Position].Word, start, start + length);
                    }
                }
                else
                {
                    break;
                }
            }

            _InputText = null;
            return null;
        }
        #endregion
        //DotLucene的分词器简单来说，就是实现Tokenizer的Next方法，把分解出来的每一个词构造为一个Token，因为Token是DotLucene分词的基本单位。
        //public override Token Next()
        //{
        //    if (_OriginalResult)
        //    {
        //        string retStr = _InputText;

        //        _InputText = null;

        //        if (retStr == null)
        //        {
        //            return null;
        //        }

        //        return new Token(retStr, 0, retStr.Length);
        //    }

        //    int length = 0;    //词汇的长度.
        //    int start = 0;     //开始偏移量.

        //    while (true)
        //    {
        //        _Position++;
        //        if (_Position < _WordList.Length)
        //        {
        //            if (_WordList[_Position] != null)
        //            {
        //                length = _WordList[_Position].Word.Length;
        //                start = _WordList[_Position].Position;
        //                return new Token(_WordList[_Position].Word, start, start + length);
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    _InputText = null;
        //    return null;
        //}

        public ICollection<WordInfo> SegmentToWordInfos(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new LinkedList<WordInfo>();
            }


            return segment.DoSegment(str);
        }
    }
}
