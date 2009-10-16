using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using System.IO;
using log4net.Layout;
using log4net.Core;
using System.Configuration;

namespace AIMHealth.Safari.Logging
{
    public class SimpleLogger
    {
        private static SimpleLoggerConfig config;
        private static log4net.ILog logger;

        static SimpleLogger()
        {
  //          config = (SimpleLoggerConfig)ConfigurationManager.GetSection("SimpleLogger");

 //           if (config == null)
                config = new SimpleLoggerConfig();

            logger = log4net.LogManager.GetLogger(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);
//            log4net.Config.XmlConfigurator.Configure(
//                new FileInfo(
//                    config.SimpleLoggerConfigFile
//                ));
        }

        public static void AddConsoleLogger()
        {
            ConsoleAppender consoleLog = new ConsoleAppender();
            PatternLayout patternLayout = new PatternLayout(config.PatternFormat);
            consoleLog.Layout = patternLayout;
            AddAppender(consoleLog);
        }

        public static void AddRollingFileLogger()
        {
            //Create a rolling file appender that is specific to this application.
            RollingFileAppender rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.MaxFileSize = config.RollingFileMaxFileSize;
            rollingFileAppender.MaxSizeRollBackups = config.RollingFileMaxRollBackups;
            rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
            rollingFileAppender.StaticLogFileName = true;
            rollingFileAppender.File = string.Format(
                config.SimpleLoggerFileNameFormat
                , System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);

            PatternLayout patternLayout = new PatternLayout(config.PatternFormat);
            rollingFileAppender.Layout = patternLayout;

            rollingFileAppender.ActivateOptions();

            AddAppender(rollingFileAppender);
        }

        public static void AddFileAppender(string filename)
        {
            FileAppender appender = new FileAppender();
            appender.AppendToFile = true;
            appender.File = filename;
            SimpleLogger.AddAppender(appender);
            
            
        }

        public static void AddAppender(IAppender appender)
        {
            //Create a rolling file appender that is specific to this application.
            IAppenderAttachable attacher = (IAppenderAttachable)logger.Logger;
            attacher.AddAppender(appender);
        }

        public static void Debug(object message, Exception exception)
        {
            logger.Debug(message, exception);
        }

        public static void Debug(object message)
        {
            logger.Debug(message);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            logger.DebugFormat(provider, format, args);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.DebugFormat(format, arg0, arg1, arg2);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            logger.DebugFormat(format, arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0)
        {
            logger.DebugFormat(format, arg0);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            logger.DebugFormat(format, args);
        }

        public static void Error(object message, Exception exception)
        {
            logger.Error(message, exception);
        }

        public static void Error(object message)
        {
            logger.Error(message);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            logger.ErrorFormat(provider, format, args);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            logger.ErrorFormat(format, arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            logger.ErrorFormat(format, arg0);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            logger.ErrorFormat(format, args);
        }

        public static void Fatal(object message, Exception exception)
        {
            logger.Fatal(message, exception);
        }

        public static void Fatal(object message)
        {
            logger.Fatal(message);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            logger.FatalFormat(provider, format, args);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.FatalFormat(format, arg0, arg1, arg2);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            logger.FatalFormat(format, arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0)
        {
            logger.FatalFormat(format, arg0);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            logger.FatalFormat(format, args);
        }

        public static void Info(object message, Exception exception)
        {
            logger.Info(message, exception);
        }

        public static void Info(object message)
        {
            logger.Info(message);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            logger.InfoFormat(provider, format, args);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.InfoFormat(format, arg0, arg1, arg2);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            logger.InfoFormat(format, arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0)
        {
            logger.InfoFormat(format, arg0);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            logger.InfoFormat(format, args);
        }

        public static bool IsDebugEnabled
        {
            get { return logger.IsDebugEnabled; }
        }

        public static bool IsErrorEnabled
        {
            get { return logger.IsErrorEnabled; }
        }

        public static bool IsFatalEnabled
        {
            get { return logger.IsFatalEnabled; }
        }

        public static bool IsInfoEnabled
        {
            get { return logger.IsInfoEnabled; }
        }

        public static bool IsWarnEnabled
        {
            get { return logger.IsWarnEnabled; }
        }

        public static void Warn(object message, Exception exception)
        {
            logger.Warn(message, exception);
        }

        public static void Warn(object message)
        {
            logger.Warn(message);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            logger.WarnFormat(provider, format, args);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.WarnFormat(format, arg0, arg1, arg2);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            logger.WarnFormat(format, arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0)
        {
            logger.WarnFormat(format, arg0);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            logger.WarnFormat(format, args);
        }
    }
}
