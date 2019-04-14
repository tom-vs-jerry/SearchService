using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using log4net;
using log4net.Config;

namespace SearchService.Common.Log
{
    /// <summary>
    /// log4net底层包装类
    /// 作者：Sofia
    /// 日期：2009-7-28
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// 日志
        /// </summary>
        //private static ILog errorLogger;

        private static ILog infoLogger;

        //private static ILog debugLogger;
        private static ILog logger;

        private static ILog errorLogger;
        private static ILog debugLogger;

        private static string configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        /// <summary>
        /// 初始化
        /// </summary>
        static Logger()
        {

            XmlConfigurator.Configure(new FileInfo(configPath));
            if (infoLogger == null)
            {
                infoLogger = LogManager.GetLogger("InfoLogger");
            }
            if (errorLogger == null)
            {
                errorLogger = LogManager.GetLogger("ErrorLogger");
            }
            if (debugLogger == null)
            {
                debugLogger = LogManager.GetLogger("DebugLogger");
            }
            if (logger == null)
            {
                logger = LogManager.GetLogger("CommonLogger");
            }

        }
        /// <summary>
        /// 写普通信息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Write(string msg)
        {
            infoLogger.Info(msg);
        }

        /// <summary>
        /// 写普通信息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void WriteInfo(string msg)
        {
            logger.Info(msg);
        }

        /// <summary>
        /// 写错误信息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        public static void WriteError(string msg, Exception ex)
        {
            errorLogger.Error(msg, ex);
        }

        /// <summary>
        /// 写错误信息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void WriteError(string msg)
        {
            errorLogger.Error(msg);
        }

        /// <summary>
        /// 写调试信息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void WriteDebug(string msg)
        {
            debugLogger.Debug(msg);
        }

    }
}
