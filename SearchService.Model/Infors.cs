using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Model
{
    /// <summary>
    /// 描述:信息实体类 （搜索引擎的索引实体）
    /// 作者：段进雄
    /// 日期：2016-2-20
    /// </summary>
    [Serializable]
    public class Infors
    {

        /// <summary>
        /// 文档编号
        /// </summary>
        public string DocNum { get; set; }


        /// <summary>
        /// 文档分词
        /// </summary>
        public string Participle { get; set; }

        /// <summary>
        /// 当前页码数
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>  
        public int PageCount { get; set; }

        /// <summary>
        /// 信息文件ID
        /// </summary>
        public string InformationID
        {
            get;
            set;
        }
        /// <summary>
        /// 文件类型ID
        /// </summary>
        public string TypeID
        {
            get;
            set;
        }
        /// <summary>
        /// 文件的上传者ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }

       
        /// <summary>
        /// 信息标题
        /// </summary>
        public string Title
        {
            get;

            set;
        }

        /// <summary>
        /// 信息正文
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 信息摘要
        /// </summary>
        public string Summary
        {
            get;
            set;
        }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime Time
        {
            get;

            set;
        }
    }
}
