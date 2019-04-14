using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;


namespace Segment.SegmentDict
{
    public class PosTraffic
    {
        Hashtable m_PosBinTbl = new Hashtable();
        ArrayList m_PosBinList = new ArrayList();

        CPOS m_POS;

        public CPOS POS
        {
            get
            {
                return m_POS;
            }

            set
            {
                m_POS = value;
            }
        }

        private void Hit(EnumPOSBin posBin)
        {
            EnumPOSBin bin = (EnumPOSBin)m_PosBinTbl[posBin.HashCode];
            if (bin == null)
            {
                bin = new EnumPOSBin(posBin.m_Pos1, posBin.m_Pos2);
                bin.m_Count = 1;
                m_PosBinTbl[bin.HashCode] = bin;
                m_PosBinList.Add(bin);
            }
            else
            {
                bin.m_Count++;
            }
        }

        public ArrayList GetPosBinGroup()
        {
            m_PosBinList.Sort();
            return m_PosBinList;
        }

        public void Traffic(List<String> words)
        {
            for (int i = 0; i < words.Count - 1; i++)
            {
                bool isReg;
                InnerPOS[] curPos = m_POS.GetPos((String)words[i], out isReg);
                InnerPOS[] nextPos = m_POS.GetPos((String)words[i + 1], out isReg);


                //ArrayList curList = m_POS.GetPosList(curPos);

                if (curPos.Length != 1)
                {
                    continue;
                }

                InnerPOS pos1 = curPos[0];

                if (pos1 == InnerPOS.POS_UNK)
                {
                    continue;
                }


                //ArrayList nextList = m_POS.GetPosList(nextPos);

                if (nextPos.Length != 1)
                {
                    continue;
                }

                InnerPOS pos2 = (InnerPOS)nextPos[0];

                if (pos2 == InnerPOS.POS_UNK)
                {
                    continue;
                }

                EnumPOSBin bin = new EnumPOSBin(pos1, pos2);

                Hit(bin);
            }
        }

    }
}
