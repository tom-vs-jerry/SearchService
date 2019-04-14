using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Segment.Algorithm
{
    /// <summary>
    /// 匹配方向
    /// </summary>
    public enum MatchDirection
    {
        /// <summary>
        /// 从左到右
        /// </summary>
        LeftToRight = 0,

        /// <summary>
        /// 从右到左
        /// </summary>
        RightToLeft = 1,
    }

    /// <summary>
    /// 单词信息
    /// </summary>
    public class WordInfo : IComparable<WordInfo>
    {
        /// <summary>
        /// 单词
        /// </summary>
        public String Word;

        /// <summary>
        /// 单词首字符在全文中的位置
        /// </summary>
        public int Position;

        /// <summary>
        /// 单词的权重级别
        /// </summary>
        public int Rank;

        /// <summary>
        /// 单词对应的标记
        /// </summary>
        public object Tag;

        public override string ToString()
        {
            return Word;
        }

        public int GetEndPositon()
        {
            return Position + Word.Length;
        }

        #region IComparable<WordInfo> Members

        public int CompareTo(WordInfo other)
        {
            if (other == null)
            {
                return -1;
            }

            if (this.Position != other.Position)
            {
                return this.Position.CompareTo(other.Position);
            }

            if (other.Word == null)
            {
                return -1;
            }

            return this.Word.Length.CompareTo(other.Word.Length);
        }

        #endregion
    }

    public delegate bool CompareByPosFunc(List<WordInfo> words, List<int> pre, List<int> cur);
    public delegate bool SelectByFreqFunc(List<WordInfo> words, List<int> pre, List<int> cur);

    /// <summary>
    /// 从全文中提取指定的单词，及其位置
    /// </summary>
    public class CExtractWords
    {
        struct T_NodeList
        {
            public List<int> Indexes;
            public int Space;
            public int Deep;

            public T_NodeList(List<int> indexes, int space, int deep)
            {
                Indexes = indexes;
                Space = space;
                Deep = deep;
            }

            public override string ToString()
            {
                StringBuilder str = new StringBuilder();

                str.AppendFormat("Space={0} Deep={1} Indexes=", Space, Deep);
                foreach (int index in Indexes)
                {
                    str.AppendFormat("{0},", index);
                }

                return str.ToString();
            }

        }


        CWordDfa m_WordDfa;
        List<int> m_GameNodes = new List<int>(); //本算法的最大匹配分词输出的单词序列列表
        List<T_NodeList> m_MultiNodes = new List<T_NodeList>(); //多元分词输出的单词序列列表
        byte[] m_HitTable = null;
        byte[] m_IdxHitTable = null;
        int m_MinSpace;
        int m_MinDeep;

        MatchDirection m_MatchDirection;
        CompareByPosFunc m_CompareByPos;
        SelectByFreqFunc m_SelectByFreq;

        /// <summary>
        /// Get the hit table
        /// </summary>
        public byte[] HitTable
        {
            get
            {
                return m_HitTable;
            }
        }

        public CompareByPosFunc CompareByPosEvent
        {
            get
            {
                return m_CompareByPos;
            }

            set
            {
                m_CompareByPos = value;
            }
        }

        public SelectByFreqFunc SelectByFreqEvent
        {
            get
            {
                return m_SelectByFreq;
            }

            set
            {
                m_SelectByFreq = value;
            }
        }


        /// <summary>
        /// 匹配方向
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

        public CExtractWords()
        {
            m_MatchDirection = MatchDirection.LeftToRight;
            m_WordDfa = new CWordDfa();
        }

        public object GetTag(String word)
        {
            return m_WordDfa.GetTag(word);
        }

        public bool InsertWordToDfa(String word, object tag)
        {
            return m_WordDfa.InsertWordToDfa(word, tag);
        }


        private bool CompareGroup(List<WordInfo> words, List<int> pre, List<int> cur, MatchDirection direction)
        {
            int i;

            if (direction == MatchDirection.LeftToRight)
            {
                i = 0;
            }
            else
            {
                i = cur.Count - 1;
            }


            while ((direction == MatchDirection.LeftToRight && i < cur.Count) ||
                (direction == MatchDirection.RightToLeft && i >= 0))
            {
                if (i >= pre.Count)
                {
                    break;
                }

                int preId = (int)pre[i];
                int curId = (int)cur[i];

                if (((WordInfo)words[curId]).Word.Length > ((WordInfo)words[preId]).Word.Length)
                {
                    return true;
                }
                else if (((WordInfo)words[curId]).Word.Length < ((WordInfo)words[preId]).Word.Length)
                {
                    return false;
                }

                if (direction == MatchDirection.LeftToRight)
                {
                    i++;
                }
                else
                {
                    i--;
                }
            }

            return false;
        }

        private void InitHitTable(int len)
        {
            if (m_HitTable == null)
            {
                m_HitTable = new byte[((len / 256) + 1) * 256];
                return;
            }

            if (len > m_HitTable.Length)
            {
                m_HitTable = new byte[((len / 256) + 1) * 256];
                return;
            }

            for (int i = 0; i < len; i++)
            {
                m_HitTable[i] = 0;
            }
        }

        private void Hit(WordInfo wordInfo)
        {
            if (wordInfo.Word.Length == 1)
            {
                if (m_HitTable[wordInfo.Position] == 0)
                {
                    m_HitTable[wordInfo.Position] = 2;
                }

                return;
            }

            for (int i = wordInfo.Position; i < wordInfo.Position + wordInfo.Word.Length; i++)
            {
                m_HitTable[i] = 1;
            }
        }

        private void InitIdxHitTable(int len)
        {
            if (m_IdxHitTable == null)
            {
                m_IdxHitTable = new byte[((len / 256) + 1) * 256];
                return;
            }

            if (len > m_IdxHitTable.Length)
            {
                m_IdxHitTable = new byte[((len / 256) + 1) * 256];
                return;
            }

            for (int i = 0; i < len; i++)
            {
                m_IdxHitTable[i] = 0;
            }
        }

        private void IdxHit(int index)
        {
            m_IdxHitTable[index] = 1;
        }

        private bool IdxHited(int index)
        {
            return m_IdxHitTable[index] != 0;
        }

        /// <summary>
        /// 博弈树
        /// </summary>
        /// <param name="words"></param>
        /// <param name="nodes"></param>
        /// <param name="init"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="spaceNum"></param>
        /// <param name="deep"></param>
        /// <param name="multiSelect">多元分词选项</param>
        /// <returns></returns>
        private List<int> GameTree(List<WordInfo> words, List<int> nodes,
            bool init, int begin, int end, ref int spaceNum, ref int deep, bool multiSelect)
        {
            if (init)
            {
                int startPos = ((WordInfo)words[begin]).Position;
                for (int i = begin; i <= end; i++)
                {
                    WordInfo wordInfo = (WordInfo)words[i];
                    spaceNum = wordInfo.Position - startPos;
                    deep = 0;
                    List<int> oneNodes;

                    if (i == end)
                    {
                        oneNodes = new List<int>();
                        oneNodes.Add(i);
                        deep++;
                    }
                    else
                    {
                        oneNodes = GameTree(words, nodes, false, i, end, ref spaceNum, ref deep, multiSelect);
                    }

                    if (multiSelect)
                    {
                        //多元分词
                        if (oneNodes != null)
                        {
                            m_MultiNodes.Add(new T_NodeList(oneNodes, spaceNum, deep));
                        }
                    }

                    if (oneNodes != null)
                    {
                        bool select = false;

                        if (m_MinSpace > spaceNum ||
                            (m_MinSpace == spaceNum && deep < m_MinDeep))
                        {
                            select = true;

                            if (m_MinSpace == 0)
                            {
                                if (SelectByFreqEvent != null)
                                {
                                    select = SelectByFreqEvent(words, m_GameNodes, oneNodes);
                                }
                            }
                        }
                        else if (m_MinDeep == deep && m_MinSpace == spaceNum)
                        {
                            if (m_CompareByPos != null && m_MinSpace == 0)
                            {
                                select = m_CompareByPos(words, m_GameNodes, oneNodes);
                            }
                            else
                            {
                                select = CompareGroup(words, m_GameNodes, oneNodes, MatchDirection);
                            }
                        }


                        if (select)
                        {
                            m_MinDeep = deep;
                            m_MinSpace = spaceNum;
                            m_GameNodes.Clear();
                            foreach (int obj in oneNodes)
                            {
                                m_GameNodes.Add(obj);
                            }
                        }
                    }
                    deep = 0;
                    nodes.Clear();
                }
            }
            else
            {
                nodes.Add(begin);
                deep++;

                WordInfo last = (WordInfo)words[begin];

                bool nextStep = false;
                bool reach = false;
                int endPos = last.Position + last.Word.Length - 1;

                int oldDeep = deep;
                int oldSpace = spaceNum;

                for (int i = begin + 1; i <= end; i++)
                {
                    WordInfo cur = (WordInfo)words[i];

                    if (endPos < cur.Position + cur.Word.Length - 1)
                    {
                        endPos = cur.Position + cur.Word.Length - 1;
                    }


                    if (last.Position + last.Word.Length <= cur.Position)
                    {

                        nextStep = true;

                        if (reach)
                        {
                            reach = false;
                            spaceNum = oldSpace;
                            deep = oldDeep;
                            nodes.RemoveAt(nodes.Count - 1);
                        }

                        spaceNum += cur.Position - (last.Position + last.Word.Length);
                        List<int> oneNodes;
                        oneNodes = GameTree(words, nodes, false, i, end, ref spaceNum, ref deep, multiSelect);

                        if (multiSelect)
                        {
                            //多元分词
                            if (oneNodes != null)
                            {
                                m_MultiNodes.Add(new T_NodeList(oneNodes, spaceNum, deep));
                            }
                        }

                        if (oneNodes != null)
                        {
                            bool select = false;

                            if (m_MinSpace > spaceNum ||
                                (m_MinSpace == spaceNum && deep < m_MinDeep))
                            {
                                select = true;

                                if (m_MinSpace == 0)
                                {
                                    if (SelectByFreqEvent != null)
                                    {
                                        select = SelectByFreqEvent(words, m_GameNodes, oneNodes);
                                    }
                                }
                            }
                            else if (m_MinDeep == deep && m_MinSpace == spaceNum)
                            {
                                if (m_CompareByPos != null && m_MinSpace == 0)
                                {
                                    select = m_CompareByPos(words, m_GameNodes, oneNodes);
                                }
                                else
                                {
                                    select = CompareGroup(words, m_GameNodes, oneNodes, MatchDirection);
                                }
                            }


                            if (select)
                            {
                                reach = true;
                                nextStep = false;
                                m_MinDeep = deep;
                                m_MinSpace = spaceNum;
                                m_GameNodes.Clear();
                                foreach (int obj in oneNodes)
                                {
                                    m_GameNodes.Add(obj);
                                }
                            }
                            else
                            {
                                spaceNum = oldSpace;
                                deep = oldDeep;

                                nodes.RemoveRange(deep, nodes.Count - deep);
                            }
                        }
                        else
                        {
                            spaceNum = oldSpace;
                            deep = oldDeep;
                            nodes.RemoveRange(deep, nodes.Count - deep);
                        }
                    }
                }

                if (!nextStep)
                {
                    spaceNum += endPos - (last.Position + last.Word.Length - 1);

                    List<int> ret = new List<int>();

                    foreach (int obj in nodes)
                    {
                        ret.Add(obj);
                    }

                    return ret;
                }


            }

            return null;
        }

        /// <summary>
        /// 最大匹配提取全文中所有匹配的单词
        /// </summary>
        /// <param name="fullText">全文</param>
        /// <param name="multiSelect">多元分词选项</param>
        /// <param name="redundancy">冗余度</param>
        /// <returns>返回WordInfo[]数组，如果没有找到一个匹配的单词，返回长度为0的数组</returns>
        public List<WordInfo> ExtractFullTextMaxMatch(String fullText, bool multiSelect, int redundancy)
        {
            List<WordInfo> retWords = new List<WordInfo>();
            List<WordInfo> words = ExtractFullText(fullText);

            int i = 0;

            if (multiSelect)
            {
                InitHitTable(fullText.Length);
                InitIdxHitTable(words.Count);
            }

            while (i < words.Count)
            {
                WordInfo wordInfo = (WordInfo)words[i];

                int j;

                int rangeEndPos = 0;

                for (j = i; j < words.Count - 1; j++)
                {
                    if (j - i > 16)
                    {
                        //嵌套太多的情况一般很少发生，如果发生，强行中断，以免造成博弈树遍历层次过多
                        //降低系统效率
                        break;
                    }

                    if (rangeEndPos < ((WordInfo)words[j]).Position + ((WordInfo)words[j]).Word.Length - 1)
                    {
                        rangeEndPos = ((WordInfo)words[j]).Position + ((WordInfo)words[j]).Word.Length - 1;
                    }

                    if (rangeEndPos <
                        ((WordInfo)words[j + 1]).Position)
                    {
                        break;
                    }
                }

                if (j > i)
                {
                    int spaceNum = 0;
                    int deep = 0;

                    if (multiSelect)
                    {
                        m_MultiNodes.Clear();
                    }

                    m_GameNodes.Clear();


                    m_MinDeep = 65535;
                    m_MinSpace = 65535 * 256;

                    GameTree(words, new List<int>(), true, i, j, ref spaceNum, ref deep, multiSelect);

                    if (!multiSelect)
                    {
                        foreach (int index in m_GameNodes)
                        {
                            WordInfo info = (WordInfo)words[index];
                            retWords.Add(info);
                        }
                    }
                    else
                    {
                        foreach (T_NodeList nodeList in m_MultiNodes)
                        {
                            foreach (int index in nodeList.Indexes)
                            {
                                WordInfo info = (WordInfo)words[index];

                                Hit(info);

                                if ((nodeList.Deep == m_MinDeep && nodeList.Space == m_MinSpace) ||
                                    (nodeList.Deep <= m_MinDeep + redundancy && nodeList.Space == 0))
                                {
                                    bool highRank = false;

                                    foreach (int idx in m_GameNodes)
                                    {
                                        if (idx == index)
                                        {
                                            highRank = true;
                                            break;
                                        }
                                    }

                                    if (highRank)
                                    {
                                        info.Rank = 5;
                                    }
                                    else
                                    {
                                        if (!IdxHited(index))
                                        {
                                            info.Rank = 3 - (nodeList.Deep - m_MinDeep - 1);

                                            if (info.Rank > 3)
                                            {
                                                info.Rank = 3;
                                            }
                                            else if (info.Rank < 0)
                                            {
                                                info.Rank = 0;
                                            }
                                        }
                                    }

                                    if (!IdxHited(index))
                                    {
                                        retWords.Add(info);
                                        IdxHit(index);
                                    }
                                }
                            }
                        }
                    }

                    i = j + 1;
                    continue;
                }
                else
                {
                    if (!multiSelect)
                    {
                        retWords.Add(wordInfo);
                    }
                    else
                    {
                        Hit(wordInfo);

                        if (!IdxHited(i))
                        {
                            wordInfo.Rank = 1;
                            retWords.Add(wordInfo);
                            IdxHit(i);
                        }
                    }

                    i++;
                }


            }

            return retWords;
        }


        /// <summary>
        /// 提取全文
        /// </summary>
        /// <param name="fullText">全文</param>
        /// <returns>返回WordInfo[]数组，如果没有找到一个匹配的单词，返回长度为0的数组</returns>
        public List<WordInfo> ExtractFullText(String fullText)
        {
            List<WordInfo> words = new List<WordInfo>();

            if (fullText == null || fullText == "")
            {
                return words;
            }

            DfaUnit cur = null;
            bool find = false;
            int pos = 0;
            int i = 0;

            while (i < fullText.Length)
            {
                cur = m_WordDfa.Next(cur, fullText[i]);
                if (cur != null && !find)
                {
                    pos = i;
                    find = true;
                }

                if (find)
                {
                    if (cur == null)
                    {
                        find = false;
                        i = pos + 1; //有可能存在包含关系的词汇，所以需要回溯
                        continue;
                    }
                    else if (cur.QuitWord != null)
                    {
                        WordInfo wordInfo = new WordInfo();
                        wordInfo.Word = cur.QuitWord;
                        wordInfo.Position = pos;
                        wordInfo.Rank = m_WordDfa.GetRank(wordInfo.Word);
                        wordInfo.Tag = cur.Tag;
                        words.Add(wordInfo);

                        if (cur.Childs == null || i == fullText.Length - 1)
                        {
                            find = false;
                            cur = null;
                            i = pos + 1; //有可能存在包含关系的词汇，所以需要回溯
                            continue;
                        }
                    }
                }

                i++;
            }

            return words;
        }



    }
}
