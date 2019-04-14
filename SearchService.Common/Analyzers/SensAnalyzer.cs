using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using Lucene.Net.Analysis;

namespace SearchService.Common.Analyzers
{
    /// <summary>
    /// 描述：敏感词检索分析器
    /// 日期：2016年8月26日
    /// 作者：Sofia
    /// </summary>
    public class SensAnalyzer: Analyzer
    {
        private bool _OriginalResult = false;

        public SensAnalyzer()
        {
        }

        /// <summary>
        /// Return original string.
        /// Does not use only segment
        /// </summary>
        /// <param name="originalResult"></param>
        public SensAnalyzer(bool originalResult)
        {
            _OriginalResult = originalResult;
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            TokenStream result = new AllDocTokenizer(reader, _OriginalResult);
            result = new LowerCaseFilter(result);
            return result;
        }
    }
}
