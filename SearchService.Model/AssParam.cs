using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Model
{
    /// <summary>
    /// 描述：联想请求参数
    /// </summary>
    public class AssParam
    {
        /// <summary>
        /// 输入的词
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// 返回总数
        /// </summary>
        public int Count { get; set; }
    }
}
