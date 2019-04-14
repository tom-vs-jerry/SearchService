using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Model
{

    /// <summary>
    /// 描述：全文检索的参数
    /// 作者：段进雄
    /// 日期：2016-09-05
    /// </summary>
    [Serializable]
    public class AllSearchParam
    {
        private string docNum;
        /// <summary>
        /// 文档编号
        /// </summary>
        public string DocNum
        {
            get { return docNum; }
            set { docNum = value; }
        }

        private string inPutWord;
        /// <summary>
        /// 输入的词
        /// </summary>
        public string InPutWord
        {
            get { return inPutWord; }
            set { inPutWord = value; }
        }


        private SearchRange range;
        /// <summary>
        /// 搜索范围
        /// </summary>
        public SearchRange Range
        {
            get { return range; }
            set { range = value; }
        }


        private string noneWord;
        /// <summary>
        /// 输入的不含词
        /// </summary>
        public string NoneWord
        {
            get { return noneWord; }
            set { noneWord = value; }
        }


        //开始时间
        private DateTime startTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        //日期
        private DateTime endTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }


        //拟办意见
        private string advise;
        /// <summary>
        /// 拟办列表
        /// </summary>
        public string Advise
        {
            get
            {
                return advise;
            }
            set
            {
                advise = value;
            }
        }


        //用户ID
        private string userid;
        /// <summary>
        /// 文件的上传者ID
        /// </summary>
        public string UserID
        {
            get
            {
                return userid;
            }
            set
            {
                userid = value;
            }
        }


        //信息ID
        private string infromationid;
        /// <summary>
        /// 信息文件ID
        /// </summary>
        public string InformationID
        {
            get
            {
                return infromationid;
            }
            set
            {
                infromationid = value;
            }
        }

        //信息类型ID
        private string typeid;
        /// <summary>
        /// 文件类型ID
        /// </summary>
        public string TypeID
        {
            get
            {
                return typeid;
            }
            set
            {
                typeid = value;
            }
        }


        //当前页数
        private int pageNo;
        /// <summary>
        /// 当前页码数
        /// </summary>
        public int PageNo
        {
            get
            {
                return pageNo;
            }
            set
            {
                pageNo = value;
            }
        }

        ///每页显示数量
        private int pageSize;
        /// <summary>
        /// 每页显示数量
        /// </summary>  
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        //发送单位ID
        private List<string> sUnitID;
        /// <summary>
        /// 文件发送单位ID
        /// </summary>
        public List<string> SUnitID
        {
            get { return sUnitID; }
            set { sUnitID = value; }
        }


        //文件接受单位ID
        private List<string> rUnitID;
        /// <summary>
        /// 文件接受单位ID
        /// </summary>
        public List<string> RUnitID
        {
            get { return rUnitID; }
            set { rUnitID = value; }
        }

    }

    public enum SearchRange
    {
        Title = 2,
        Context = 1,
        TitleAndContext = 0
    }

}
