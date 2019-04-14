using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

using Segment.Algorithm;

namespace Segment.SegmentDict
{
    /// <summary>
    /// 词典文件
    /// </summary>
    [Serializable]
    public class DictFile
    {
        public List<DictStruct> Dicts = new List<DictStruct>();
    }


    /// <summary>
    /// 词典结构
    /// </summary>
    [Serializable]
    public class DictStruct
    {
        /// <summary>
        /// 单词
        /// </summary>
        public String Word;

        /// <summary>
        /// 词性
        /// </summary>
        public int Pos;

        /// <summary>
        /// 词频
        /// </summary>
        public double Frequency;

        public override string ToString()
        {
            return Word;
        }
    }

    public class Dict
    {
        /// <summary>
        /// 从文件加载单字词
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public IEnumerable<string> LoadSingleWordFromFile(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                yield break;
            }

            string content = CFile.ReadFileToString(fileName, "utf-8");

            foreach (char c in content)
            {
                if (c >= 0x4e00 && c <= 0x9fa5)
                {
                    yield return c.ToString();
                }
            }
        }

        /// <summary>
        /// 从文本文件读取字典
        /// </summary>
        /// <param name="fileName"></param>
        static public DictFile LoadFromTextDict(String fileName)
        {
            DictFile dictFile = new DictFile();

            String dictStr = CFile.ReadFileToString(fileName, "utf-8");

            String[] words = CRegex.Split(dictStr, "\r\n");

            foreach (String word in words)
            {
                String[] wp = CRegex.Split(word, @"\|");

                if (wp == null)
                {
                    continue;
                }

                if (wp.Length != 2)
                {
                    continue;
                }

                int pos = 0;

                try
                {
                    pos = int.Parse(wp[1]);
                }
                catch
                {
                    continue;
                }

                DictStruct dict = new DictStruct();
                dict.Word = wp[0];
                dict.Pos = pos;

                if (dict.Word.Contains("一") || dict.Word.Contains("二") ||
                    dict.Word.Contains("三") || dict.Word.Contains("四") ||
                    dict.Word.Contains("五") || dict.Word.Contains("六") ||
                    dict.Word.Contains("七") || dict.Word.Contains("八") ||
                    dict.Word.Contains("九") || dict.Word.Contains("十"))
                {
                    dict.Pos |= (int)EnumPOS.POS_A_M;
                }

                if (dict.Word == "字典")
                {
                    dict.Pos = (int)EnumPOS.POS_D_N;
                }

                dictFile.Dicts.Add(dict);
            }

            return dictFile;
        }

        static public void SaveToTextFile(String fileNmae, DictFile dictFile)
        {
            if (dictFile.Dicts == null)
            {
                return;
            }

            StringBuilder dictStr = new StringBuilder();

            foreach (DictStruct dict in dictFile.Dicts)
            {
                dictStr.AppendFormat("{0}|{1}\r\n", dict.Word, dict.Pos);
            }

            CFile.WriteString(fileNmae, dictStr.ToString(), "utf-8");
        }

        static public void SaveToBinFile(String fileName, DictFile dictFile)
        {
            Stream s = CSerialization.SerializeBinary(dictFile);
            s.Position = 0;
            CFile.WriteStream(fileName, (MemoryStream)s);
        }

        static public DictFile LoadFromBinFile(String fileName)
        {
            MemoryStream s = CFile.ReadFileToStream(fileName);
            s.Position = 0;
            object obj;
            CSerialization.DeserializeBinary(s, out obj);
            return (DictFile)obj;
        }

        static public void SaveToBinFileEx(String fileName, DictFile dictFile)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            byte[] version = new byte[32];

            int i = 0;
            foreach (byte v in System.Text.Encoding.UTF8.GetBytes("KTDictSeg Dict V1.3"))
            {
                version[i] = v;
                i++;
            }

            fs.Write(version, 0, version.Length);

            foreach (DictStruct dict in dictFile.Dicts)
            {
                byte[] word = System.Text.Encoding.UTF8.GetBytes(dict.Word);
                byte[] pos = System.BitConverter.GetBytes(dict.Pos);
                byte[] frequency = System.BitConverter.GetBytes(dict.Frequency);
                byte[] length = System.BitConverter.GetBytes(word.Length + frequency.Length + pos.Length);

                fs.Write(length, 0, length.Length);
                fs.Write(word, 0, word.Length);
                fs.Write(pos, 0, pos.Length);
                fs.Write(frequency, 0, frequency.Length);
            }

            fs.Close();
        }

        static public DictFile LoadFromBinFileEx(String fileName)
        {
            DictFile dictFile = new DictFile();
            dictFile.Dicts = new List<DictStruct>();

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            byte[] version = new byte[32];
            fs.Read(version, 0, version.Length);
            String ver = Encoding.UTF8.GetString(version, 0, version.Length);

            String verNumStr = CRegex.GetMatch(ver, "KTDictSeg Dict V(.+)", true);

            if (verNumStr == null || verNumStr == "")
            {
                //1.3以前版本

                fs.Close();
                return LoadFromBinFile(fileName);
            }

            while (fs.Position < fs.Length)
            {
                byte[] buf = new byte[sizeof(int)];
                fs.Read(buf, 0, buf.Length);
                int length = BitConverter.ToInt32(buf, 0);

                buf = new byte[length];

                DictStruct dict = new DictStruct();

                fs.Read(buf, 0, buf.Length);

                dict.Word = Encoding.UTF8.GetString(buf, 0, length - sizeof(int) - sizeof(double));
                dict.Pos = BitConverter.ToInt32(buf, length - sizeof(int) - sizeof(double));
                dict.Frequency = BitConverter.ToDouble(buf, length - sizeof(double));
                dictFile.Dicts.Add(dict);
            }

            fs.Close();

            return dictFile;
        }

    }
}
