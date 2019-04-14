using System;
using System.Collections.Generic;
using System.Text;


namespace Segment.SegmentDict.Rule
{
    class MergeNumRule : IRule
    {
        CPOS POS;

        public MergeNumRule(CPOS pos)
        {
            POS = pos;
        }

        #region IRule 成员

        public int ProcRule(List<String> preWords, int index, List<String> retWords)
        {
            String word = (String)preWords[index];
            bool isReg;
            int pos = CPOS.GetPosFromInnerPosList(POS.GetPos(word, out isReg));
            String num;

            if ((pos & (int)EnumPOS.POS_A_M) == (int)EnumPOS.POS_A_M)
            {
                num = word;
                int i = 0;

                for (i = index + 1; i < preWords.Count; i++)
                {
                    String next = (String)preWords[i];
                    int nextPos = CPOS.GetPosFromInnerPosList(POS.GetPos(next, out isReg));
                    if ((nextPos & (int)EnumPOS.POS_A_M) == (int)EnumPOS.POS_A_M)
                    {
                        num += next;
                    }
                    else
                    {
                        break;
                    }
                }

                if (num == word)
                {
                    return -1;
                }
                else
                {
                    retWords.Add(num);

                    return i;
                }
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }
}
