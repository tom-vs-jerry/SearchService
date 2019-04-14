using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using WebSocket;
using WebSocket.SubProtocol;

using SearchService.Model;
using SearchService.Common.Indexs;

using SearchService.Common.Log;

namespace SearchService.Server.Command
{
    /// <summary>
    /// 描述：联想功能
    /// 作者：段进雄
    /// 日期：2016-09-01
    /// </summary>
    public class SA : JsonSubCommand<AssParam>
    {
        protected override void ExecuteJsonCommand(WebSocketSession session, AssParam commandInfo)
        {
            try
            {
                List<string> resultList = null;
                if (!string.IsNullOrWhiteSpace(commandInfo.Input) && commandInfo.Count != 0)
                {
                    resultList = AllDocIndex.GetWordsByInputCount(commandInfo.Input, commandInfo.Count);

                    Response rs = new Response();
                    rs.Q = "SA";
                    rs.R = resultList;
                    SendJsonMessage(session, rs);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(commandInfo.Input))
                    {
                        Logger.WriteInfo("联想词为空。");
                    }

                    if (commandInfo.Count == 0)
                    {
                        Logger.WriteInfo("联想返回总数为零。");
                    }                    
                }
            }
            catch (Exception ex)
            {
                string funName = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name;
                Logger.WriteError(funName, ex);
            }
        }
    }
}
