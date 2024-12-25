using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanucServoConnection.Log
{
    public class Logger
    {
        /// <summary>
        /// 程序逻辑日志
        /// </summary>
        public static ILog Logic = LogManager.GetLogger("logerror");
        /// <summary>
        /// 业务事务日志
        /// </summary>
        public static ILog Event = LogManager.GetLogger("logevent");
        /// <summary>
        /// 控制台日志
        /// </summary>
        public static ILog Console = LogManager.GetLogger("console");

        /// <summary>
        /// 输出DEBUG级别日志[Logic]
        /// </summary>
        /// <param name="message">日志</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Debug(string message, bool toConsole = true)
        {
            Logic.Debug(message);
            if (toConsole)
            {
                Console.Debug(message);
            }
        }

        /// <summary>
        /// 输出INFO级别日志[Logic]
        /// </summary>
        /// <param name="message">日志</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Info(string message, bool toConsole = true)
        {
            Logic.Info(message);
            if (toConsole)
            {
                Console.Info(message);
            }
        }

        /// <summary>
        /// 输出INFO级别日志[Logic]
        /// </summary>
        /// <param name="message">日志</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void EventInfo(string message, bool toConsole = true)
        {
            Event.Info(message);
            if (toConsole)
            {
                Console.Info(message);
            }
        }

        /// <summary>
        /// 输出WARN级别日志[Logic]
        /// </summary>
        /// <param name="message">日志</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Warn(string message, bool toConsole = true)
        {
            Logic.Warn(message);
            if (toConsole)
            {
                Console.Warn(message);
            }
        }

        /// <summary>
        /// 输出ERROR级别日志[Logic]
        /// </summary>
        /// <param name="message">日志</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Error(string message, bool toConsole = true)
        {
            Logic.Error(message);
            if (toConsole)
            {
                Console.Error(message);
            }
        }
        /// <summary>
        /// 输出ERROR级别日志[Logic]
        /// </summary>
        /// <param name="ex">错误对象</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Error(string message, Exception ex, bool toConsole = true)
        {
            Logic.Error(message, ex);
            if (toConsole)
            {
                Console.Error(message, ex);
            }
        }
        /// <summary>
        /// 输出ERROR级别日志[Logic]
        /// </summary>
        /// <param name="ex">错误对象</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Error(Exception ex, bool toConsole = true)
        {
            Logic.Error(ex);
            if (toConsole)
            {
                Console.Error(ex);
            }
        }

        /// <summary>
        /// 输出FATAL级别日志[Logic]
        /// </summary>
        /// <param name="message">日志</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Fatal(string message, bool toConsole = true)
        {
            Logic.Fatal(message);
            if (toConsole)
            {
                Console.Fatal(message);
            }
        }

        /// <summary>
        /// 输出FATAL级别日志[Logic]
        /// </summary>
        /// <param name="message">日志</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Fatal(string message, Exception ex, bool toConsole = true)
        {
            Logic.Fatal(message, ex);
            if (toConsole)
            {
                Console.Fatal(message, ex);
            }
        }

        /// <summary>
        /// 输出ERROR级别日志[Logic]
        /// </summary>
        /// <param name="ex">错误对象</param>
        /// <param name="toConsole">是否同步输出到控制台</param>
        public static void Fatal(Exception ex, bool toConsole = true)
        {
            Logic.Fatal(ex);
            if (toConsole)
            {
                Console.Fatal(ex);
            }
        }

        /// <summary>
        /// 日志记录[Console]
        /// </summary>
        /// <param name="message">日志信息</param>
        public static void ConsoleInfo(string message)
        {
            Console.Info(message);
        }

        /// <summary>
        /// 输出INFO级别日志[Flow]
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toConsole"></param>
        public static void FlowInfo(string message, bool toConsole = false)
        {
            Event.Info(message);
            if (toConsole)
            {
                Console.Info(message);
            }
        }
    }
}
