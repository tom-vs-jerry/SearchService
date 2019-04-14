using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Segment.SegmentDict;// KTDictSeg;
using Segment.Algorithm;//FTAlgorithm;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;

namespace Segment.SegAnalyzer//Lucene.Net.Analysis.HBCommSeg
{
    /// <summary>
    /// 描述：分词分析器
    /// 作者：Sofia
    /// 日期：2016-01-10
    /// </summary>
    public class SegTokenizer : Tokenizer
    {
        //
        private static object m_LockObj = new object();
        //
        private static CSimpleDictSeg m_SimpleDictSeg;
        //
        private List<WordInfo> m_WordList = new List<WordInfo>();
        private int m_Position = -1; //词汇在缓冲中的位置.
        private bool _OriginalResult = false;
        private string _InputText;

        private string segmenterXMLConfigPaht = "";
        // this tokenizer generates three attributes:
        // offset, positionIncrement and type
        private ITermAttribute termAtt;
        private IOffsetAttribute offsetAtt;
        private IPositionIncrementAttribute posIncrAtt;
        private ITypeAttribute typeAtt;
        /// <summary>
        /// 获取当期应用程序的地址
        /// </summary>
        /// <returns>字符串</returns>
        private string GetAssemblyPath()
        {
            const string _PREFIX = @"file:///";
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            codeBase = codeBase.Substring(_PREFIX.Length, codeBase.Length - _PREFIX.Length).Replace("/", "\\");
            return System.IO.Path.GetDirectoryName(codeBase) + @"\";
        }

        public void Clear()
        {
            m_SimpleDictSeg = null;
        }
        /// <summary>
        /// 初始化词典
        /// </summary>
        private void InitSimpleDictSeg()
        {
            //Init SimpleDictSeg.
            if (m_SimpleDictSeg == null)
            {
                try
                {
                    m_SimpleDictSeg = new CSimpleDictSeg();
                    if (string.IsNullOrWhiteSpace(segmenterXMLConfigPaht))
                    {
                        m_SimpleDictSeg.LoadConfig(GetAssemblyPath() + "KTDictSeg.xml");
                    }
                    else
                    {
                        m_SimpleDictSeg.LoadConfig(segmenterXMLConfigPaht);
                    }
                    m_SimpleDictSeg.LoadDict();
                }
                catch (Exception e)
                {
                    m_SimpleDictSeg = null;
                    throw e;
                }
            }
        }

        public SegTokenizer(System.IO.TextReader input, bool originalResult)
            : this(input)
        {
            _OriginalResult = originalResult;
        }
        public SegTokenizer(System.IO.TextReader input, bool originalResult,string strConfigPath)
            : this(input)
        {
            _OriginalResult = originalResult;
            segmenterXMLConfigPaht = strConfigPath;

        }
        void Init()
        {
            InitSimpleDictSeg();
            termAtt = AddAttribute<ITermAttribute>();
            offsetAtt = AddAttribute<IOffsetAttribute>();
            posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
            typeAtt = AddAttribute<ITypeAttribute>();
        }

        public SegTokenizer()
        {
            lock (m_LockObj)
            {
                Init();
            }
        }

        public SegTokenizer(string xmlconfigPath )
        {
            segmenterXMLConfigPaht = xmlconfigPath;

            lock (m_LockObj)
            {
                Init();
            }
        }
        public SegTokenizer(System.IO.TextReader input)
            : base(input)
        {
            lock (m_LockObj)
            {
                Init();
            }

            _InputText = base.input.ReadToEnd();

            if (string.IsNullOrEmpty(_InputText))
            {
                char[] readBuf = new char[1024];

                int relCount = input.Read(readBuf, 0, readBuf.Length);

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



            lock (m_LockObj)
            {
                m_WordList = m_SimpleDictSeg.SegmentToWordInfos(_InputText);
            }
        }


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
                m_Position++;
                if (m_Position < m_WordList.Count)
                {
                    if (m_WordList[m_Position] != null)
                    {
                        length = m_WordList[m_Position].Word.Length;
                        start = m_WordList[m_Position].Position;
                        return new Token(m_WordList[m_Position].Word, start, start + length);
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

        public List<WordInfo> SegmentToWordInfos(String str)
        {
            lock (m_LockObj)
            {
                return m_SimpleDictSeg.SegmentToWordInfos(str);
            }
        }

        public List<String> Segment(String str)
        {
            lock (m_LockObj)
            {
                return m_SimpleDictSeg.Segment(str);
            }
        }
    }
}
