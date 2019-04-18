using System;
using System.Runtime.Serialization;

namespace SearchService.Model
{
    /// <summary>
    /// 描述:信息实体类 （搜索引擎的索引实体）
    /// 作者：Sofia
    /// 日期：2016-2-20
    /// </summary>
    [DataContract]
    public class Infors
    {

        /// <summary>
        /// 文档编号
        /// </summary>
        [DataMember]
        public string DocNum { get; set; }


        /// <summary>
        /// 文档分词
        /// </summary>
        [DataMember]
        public string Participle { get; set; }

        /// <summary>
        /// 当前页码数
        /// </summary>
        [DataMember]
        public int PageNo { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>  
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// 信息文件ID
        /// </summary>
        [DataMember]
        public string InformationID { get; set; }
        /// <summary>
        /// 文件类型ID
        /// </summary>
        [DataMember]
        public string TypeID { get; set; }
        /// <summary>
        /// 文件的上传者ID
        /// </summary>
        [DataMember]
        public string UserID { get; set; }


        /// <summary>
        /// 信息标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 信息正文
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 信息摘要
        /// </summary>
        [DataMember]
        public string Summary { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Urls { get; set; }
    }
}
