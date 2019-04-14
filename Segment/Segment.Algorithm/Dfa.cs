using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Segment.Algorithm
{
    /// <summary>
    /// 有穷自动机(单元结构)
    /// </summary>
    class DfaUnit
    {
        public DfaUnit Childs; //该节点的后趋节点
        public DfaUnit NextFriend; //该节点的下一个伙伴节点
        public String QuitWord; //结束时应返回的字符串,如果为null，表示没有结束
        public object Tag; //对于技术字符串的标签
        public Char Char; //当前字符
        public bool NeedTrans; //是否需要转义
    }

    class InnerInfo
    {
        public int Rank;
        public object Tag;
    }

    /// <summary>
    /// 单词有穷自动机
    /// </summary>
    class CWordDfa
    {
        bool m_UseRank;
        Hashtable m_WordsTbl; //存储需要提取的单词的表
        Hashtable m_FstCharTbl; //首字Hash表,作为有穷自动机的入口

        private DfaUnit AddChar(DfaUnit cur, Char c, String quitWord, bool needTrans, object tag)
        {
            DfaUnit unit = new DfaUnit();
            unit.Char = c;
            unit.NeedTrans = needTrans;
            unit.Childs = null;
            unit.QuitWord = quitWord;
            if (quitWord != null)
            {
                unit.Tag = tag;
            }

            unit.NextFriend = null;

            if (cur == null)
            {
                //Debug.Assert(m_FstCharTbl[c] == null);
                m_FstCharTbl[c] = unit;
            }
            else
            {
                if (cur.Childs == null)
                {
                    cur.Childs = unit;
                }
                else
                {
                    DfaUnit friend = cur.Childs;
                    DfaUnit oldFriend = friend;
                    while (friend != null)
                    {
                        oldFriend = friend;
                        friend = friend.NextFriend;
                    }

                    oldFriend.NextFriend = unit;
                }
            }

            return unit;
        }

        /// <summary>
        /// 转义符号比较
        /// </summary>
        /// <param name="trans">转义符号</param>
        /// <param name="c">实际字符</param>
        /// <returns>相等返回true</returns>
        private bool TransCharEqual(Char trans, Char c)
        {
            return false;
        }

        /// <summary>
        /// 获取单词对应的标签
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public object GetTag(String word)
        {
            InnerInfo innerInfo = (InnerInfo)m_WordsTbl[word];
            if (innerInfo != null)
            {
                return innerInfo.Tag;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取单词对应的权重级别
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns>级别,未找到单词，返回0</returns>
        public int GetRank(String word)
        {
            if (!m_UseRank)
            {
                return 0;
            }

            InnerInfo innerInfo = (InnerInfo)m_WordsTbl[word];
            if (innerInfo != null)
            {
                return innerInfo.Rank;
            }
            else
            {
                return 0;
            }
        }

        public DfaUnit Next(DfaUnit cur, Char c)
        {
            if (cur == null)
            {
                DfaUnit unit = (DfaUnit)m_FstCharTbl[c];
                if (unit == null)
                {
                    return null;
                }
                else
                {
                    return unit;
                }
            }
            else
            {
                DfaUnit unit = cur.Childs;
                while (unit != null)
                {
                    if (unit.NeedTrans)
                    {
                        if (TransCharEqual(unit.Char, c))
                        {
                            return cur;
                        }
                    }

                    if (unit.Char == c)
                    {
                        return unit;
                    }

                    unit = unit.NextFriend;
                }
            }


            return null;
        }


        /// <summary>
        /// 遍历有穷自动机，获取最后一个和输入单词匹配的单元
        /// </summary>
        /// <param name="word">单词</param>
        /// <param name="pos">输出位置</param>
        /// <returns>最后一个匹配单元，如果第一个字符就不能匹配，返回null</returns>
        private DfaUnit GetLastMatchUnit(String word, out int pos, object tag)
        {
            pos = 0;
            DfaUnit cur = null;
            DfaUnit last = null;

            while (pos < word.Length)
            {
                last = cur;
                cur = Next(cur, word[pos]);
                if (cur == null)
                {
                    return last;
                }

                pos++;
            }

            cur.QuitWord = word;
            cur.Tag = tag;
            return cur;
        }


        /// <summary>
        /// 向有穷自动机输入单词
        /// </summary>
        /// <param name="word">单词</param>
        /// <param name="rank">单词的权重</param>
        public void InsertWordToDfa(String word)
        {
            InsertWordToDfa(word, 0, null);
        }

        /// <summary>
        /// 向有穷自动机输入单词
        /// </summary>
        /// <param name="word">单词</param>
        /// <param name="tag">标签</param>
        /// <returns>如果插入失败返回false</returns>
        public bool InsertWordToDfa(String word, object tag)
        {
            return InsertWordToDfa(word, 0, tag);
        }


        /// <summary>
        /// 向有穷自动机输入单词
        /// </summary>
        /// <param name="word">单词</param>
        /// <param name="rank">单词的权重</param>
        /// <param name="tag">标签</param>
        /// <returns>如果插入失败返回false</returns>
        public bool InsertWordToDfa(String word, int rank, object tag)
        {
            if (word == null || word == "")
            {
                return false;
            }

            if (rank != 0)
            {
                m_UseRank = true;
            }

            if (m_WordsTbl[word] != null)
            {
                return false;
            }

            InnerInfo innerInfo = new InnerInfo();
            innerInfo.Rank = rank;
            innerInfo.Tag = tag;
            m_WordsTbl[word] = innerInfo;

            int pos;
            DfaUnit unit = GetLastMatchUnit(word, out pos, tag);

            bool needTrans = false;
            for (int i = pos; i < word.Length; i++)
            {
                if (!needTrans && word[i] == '\\')
                {
                    if (i == word.Length - 1)
                    {
                        //最后一个字符是转义符号
                        throw (new Exception("Last char is trans char!"));
                    }
                    //转义
                    needTrans = true;
                    continue;
                }

                if (i == word.Length - 1)
                {
                    unit = AddChar(unit, word[i], word, needTrans, tag);
                }
                else
                {
                    unit = AddChar(unit, word[i], null, needTrans, tag);
                }

                needTrans = false;
            }

            return true;
        }

        public CWordDfa()
        {
            m_WordsTbl = new Hashtable();
            m_FstCharTbl = new Hashtable();
            m_UseRank = false;
        }


    }
}
