using System;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]//For Dict
namespace miniCoreFrame.Common
{
    /*
    <?xml version="1.0"?>
    <configuration>
      <configSections>
        <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
      </configSections>
      <log4net>
        <appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
          <param name="File" value="Logs/log" />
          <param name="AppendToFile" value="true" />
          <param name="MaxSizeRollBackups" value="1000" />
          <param name="DatePattern" value="yyyy-MM-dd&quot;.log&quot;" />
          <param name="StaticLogFileName" value="false" />
          <param name="RollingStyle" value="Date" />
          <layout type="log4net.Layout.PatternLayout">
            <param name="ConversionPattern" value="%d [%t] %-5p %c [%x] - %m%n%n" />
          </layout>
        </appender>
        <root>
          <appender-ref ref="RollingLogFileAppender" />
        </root>
      </log4net>
    </configuration>
    */


    public class Log
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("File");

        #region Debug
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message)
        {
            log.Debug(message);
        }

        public static void Debug(object message, Exception exp)
        {
            log.Debug(message, exp);
        }

        public static void DebugFormat(string format, object arg0)
        {
            log.DebugFormat(format, arg0);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.DebugFormat(provider, format, args);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            log.DebugFormat(format, arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            log.DebugFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            log.ErrorFormat(format, arg0);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.ErrorFormat(provider, format, args);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            log.ErrorFormat(format, arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            log.ErrorFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Fatal
        /// <summary>
        /// 致命的,毁灭性的
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public static void FatalFormat(string format, object arg0)
        {
            log.FatalFormat(format, arg0);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.FatalFormat(provider, format, args);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            log.FatalFormat(format, arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            log.FatalFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Info

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message)
        {
            log.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }

        public static void InfoFormat(string format, object arg0)
        {
            log.InfoFormat(format, arg0);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.InfoFormat(provider, format, args);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            log.InfoFormat(format, arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            log.InfoFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Warn

        /// <summary>
        /// 警告,注意,通知
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(object message)
        {
            log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public static void WarnFormat(string format, object arg0)
        {
            log.WarnFormat(format, arg0);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.WarnFormat(provider, format, args);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            log.WarnFormat(format, arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            log.WarnFormat(format, arg0, arg1, arg2);
        }
        #endregion
    }

}
