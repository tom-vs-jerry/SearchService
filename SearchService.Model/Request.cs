using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Model
{
    /// <summary>
    /// 描述：请求实体
    /// 作者：Sofia
    /// 日期：2019-04-11
    /// </summary>
    [Serializable]
    public partial class Request
    {

        /// <summary>
        /// 请求类型 K(获取联想的关键词)、F(获取分词数据)
        /// </summary>
        public string Q { get; set; }


        /// <summary>
        /// 请求参数
        /// </summary>
        public List<string> P { get; set; }
    }
}
