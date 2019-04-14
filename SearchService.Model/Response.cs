using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Model
{
    /// <summary>
    /// 描述：应答实体
    /// 作者：Sofia
    /// 日期：2016-2-1
    /// </summary>
    [Serializable]
    public partial class Response
    {

        /// <summary>
        /// 应答类型
        /// </summary>
        public string Q { get; set; }


        /// <summary>
        /// 应答数据
        /// </summary>
        public List<string> R { get; set; }
    }
}
