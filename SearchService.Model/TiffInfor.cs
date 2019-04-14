using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TiffInfor
    {
        string tiffNum = "";
        /// <summary>
        /// 传真编号
        /// </summary>
        public string TiffNum
        {
            get { return tiffNum; }
            set { tiffNum = value; }
        }


        string clientSessionID = "";
        /// <summary>
        /// 客户端Session
        /// </summary>
        public string ClientSessionID
        {
            get { return clientSessionID; }
            set { clientSessionID = value; }
        }

        string senderTel = "";
        /// <summary>
        /// 发送者电话
        /// </summary>
        public string SenderTel
        {
            get { return senderTel; }
            set { senderTel = value; }
        }
        string receiverTel = "";
        /// <summary>
        /// 接收者电话
        /// </summary>
        public string ReceiverTel
        {
            get { return receiverTel; }
            set { receiverTel = value; }
        }
        int pageCount = 0;
        //总页数
        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }
        int type = 0;//0服务端 //1客户端
        /// <summary>
        /// 请求类型（0服务端 1客户端）
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
