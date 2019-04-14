using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Lucene.Net.Analysis;


namespace Segment.SegAnalyzer//Lucene.Net.Analysis.HBCommSeg
{
   
        public class SegAnalyzer : Analyzer
        {
            //private static Stopwatch m_Duration = new Stopwatch();

            private bool _OriginalResult = false;
            private string segmenterXMLConfigPaht = "";
            /// <summary>
            /// 统计分词占用时间
            /// </summary>
            //public static long Duration
            //{
            //    get
            //    {
            //        return m_Duration.ElapsedMilliseconds;
            //    }

            //    set
            //    {
            //        m_Duration.Reset();
            //    }
            //}

            public SegAnalyzer()
            {
            }

            /// <summary>
            /// Return original string.
            /// Does not use only segment
            /// </summary>
            /// <param name="originalResult"></param>
            public SegAnalyzer(bool originalResult)
            {
                _OriginalResult = originalResult;
            }
            public SegAnalyzer( string strConfigPath)
            {                
                segmenterXMLConfigPaht = strConfigPath;
            }
            public SegAnalyzer(bool originalResult,string strConfigPath)
            {
                _OriginalResult = originalResult;
                segmenterXMLConfigPaht = strConfigPath;
            }

            public override TokenStream TokenStream(string fieldName, TextReader reader)
            {
//#if DEBUG
//               // m_Duration.Start();
//#endif
                TokenStream result = new SegTokenizer(reader, _OriginalResult, segmenterXMLConfigPaht);
//#if DEBUG
//               // m_Duration.Stop();
//#endif
                result = new LowerCaseFilter(result);
                return result;
            }
        
    }
}
