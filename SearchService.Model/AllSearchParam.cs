using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Model
{

    /// <summary>
    /// 描述：全文检索的参数
    /// 作者：Sofia
    /// 日期：2016-09-05
    /// </summary>
    [Serializable]
    public class AllSearchParam
    {

        /// <summary>
        /// 文档编号
        /// </summary>
        public string DocNum { get; set; }


        /// <summary>
        /// 输入的词
        /// </summary>
        public string InPutWord { get; set; }



        /// <summary>
        /// 搜索范围
        /// </summary>
        public SearchRange Range { get; set; }



        /// <summary>
        /// 输入的不含词
        /// </summary>
        public string NoneWord { get; set; }



        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }



        /// <summary>
        /// 拟办列表
        /// </summary>
        public string Advise { get; set; }


        /// <summary>
        /// 文件的上传者ID
        /// </summary>
        public string UserID { get; set; }



        /// <summary>
        /// 信息文件ID
        /// </summary>
        public string InformationID { get; set; }


        /// <summary>
        /// 文件类型ID
        /// </summary>
        public string TypeID { get; set; }



        /// <summary>
        /// 当前页码数
        /// </summary>
        public int PageNo { get; set; }


        /// <summary>
        /// 每页显示数量
        /// </summary>  
        public int PageSize { get; set; }


        /// <summary>
        /// 文件发送单位ID
        /// </summary>
        public List<string> SUnitID { get; set; }



        /// <summary>
        /// 文件接受单位ID
        /// </summary>
        public List<string> RUnitID { get; set; }

        /// <summary>
        /// 日期排序类型：2降序，1升序，0无排序
        /// </summary>
        public DateSortType DateSort { set; get; }
    }

    public enum DateSortType
    {
        No = 0,
        Asc = 1, //升序
        Dsc = 2, //降序
    }
    public enum SearchRange
    {
        Title = 2,
        Context = 1,
        TitleAndContext = 0
    }

}
