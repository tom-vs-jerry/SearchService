using System;
using System.Collections.Generic;
using System.Text;
using Segment.Framework;

namespace Segment.Setting
{
    [Serializable, System.Xml.Serialization.XmlRoot(Namespace = "http://www.codeplex.com/")] 
    public class SegmentSettings
    {
        #region static members
        private static SegmentSettings _Config;
        
        public static SegmentSettings Config
        {
            get
            {
                return _Config;
            }
        }

        static public void Load(string fileName)
        {
            //获取分词配置文件信息
            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open,
                         System.IO.FileAccess.Read))
                    {
                        _Config = XmlSerialization<SegmentSettings>.Deserialize(fs);
                    }
                }
                catch
                {
                    _Config = new SegmentSettings();
                }
            }
            else
            {
                //获取默认信息
                _Config = new SegmentSettings();
            }
        }

        static public void Save(string fileName)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create,
                 System.IO.FileAccess.ReadWrite))
            {
                XmlSerialization<SegmentSettings>.Serialize(Config, Encoding.UTF8, fs);
            }
        }

        #endregion

        public string GetDictionaryPath()
        {
            string path = DictionaryPath;

            string currentDir = System.IO.Directory.GetCurrentDirectory();
            System.IO.Directory.SetCurrentDirectory(Framework.Path.GetAssemblyPath());
            path = System.IO.Path.GetFullPath(path);
            System.IO.Directory.SetCurrentDirectory(currentDir);

            return Path.AppendDivision(path, '\\');

        }

        #region Properties

        private string _DictionaryPath = "Dict";

        public string DictionaryPath
        {
            get
            {
                return _DictionaryPath;
            }

            set
            {
                _DictionaryPath = value;
            }
        }

        private Match.MatchOptions _MatchOptions = new Match.MatchOptions();

        public Match.MatchOptions MatchOptions
        {
            get
            {
                return _MatchOptions;
            }

            set
            {
                _MatchOptions = value;
            }
        }

        private Match.MatchParameter _Parameters = new Match.MatchParameter();

        public Match.MatchParameter Parameters
        {
            get
            {
                return _Parameters;
            }

            set
            {
                _Parameters = value;
            }
        }

        #endregion

        public Match.MatchOptions GetOptionsCopy()
        {
            Match.MatchOptions options = new Match.MatchOptions();

            options.ChineseNameIdentify = this.MatchOptions.ChineseNameIdentify;
            options.FrequencyFirst = this.MatchOptions.FrequencyFirst;
            options.MultiDimensionality = this.MatchOptions.MultiDimensionality;
            options.FilterStopWords = this.MatchOptions.FilterStopWords;
            options.IgnoreSpace = this.MatchOptions.IgnoreSpace;
            options.ForceSingleWord = this.MatchOptions.ForceSingleWord;
            options.TraditionalChineseEnabled = this.MatchOptions.TraditionalChineseEnabled;
            options.OutputSimplifiedTraditional = this.MatchOptions.OutputSimplifiedTraditional;

            return options;
        }

        public Match.MatchParameter GetParameterCopy()
        {
            Match.MatchParameter parameter = new Match.MatchParameter();

            parameter.Redundancy = this.Parameters.Redundancy;
            parameter.UnknowRank = this.Parameters.UnknowRank;
            parameter.BestRank = this.Parameters.BestRank;
            parameter.SecRank = this.Parameters.SecRank;
            parameter.ThirdRank = this.Parameters.ThirdRank;
            parameter.SingleRank = this.Parameters.SingleRank;
            parameter.NumericRank = this.Parameters.NumericRank;
            parameter.EnglishRank = this.Parameters.EnglishRank;
            parameter.SymbolRank = this.Parameters.SymbolRank;
            parameter.SimplifiedTraditionalRank = this.Parameters.SimplifiedTraditionalRank;

            return parameter;
        }

    }
}
