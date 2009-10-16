    using System;
    using System.Collections.Generic;
    using System.Text;
    using log4net.Appender;
    using log4net;
    using System.Reflection;
    using System.IO;
    using System.Configuration;


namespace AIMHealth.Safari.Logging
{
        public class Log
        {
            private static string _location;
            private static string _name;
            private static string _fullName;
            private Emailer _emailer;

            private static log4net.ILog _logger = LogManager.GetLogger(typeof(Log));


            public void CloseFile()
            {
                log4net.Appender.FileAppender fileAppender = new log4net.Appender.FileAppender();
                fileAppender.Close();
            }

            public void OpenFile()
            {
                log4net.Appender.FileAppender fileAppender = new log4net.Appender.FileAppender();
                fileAppender.AppendToFile = true;
                fileAppender.File = _fullName;

            }

            public Log(string Location, string Name)
            {
                //to return full name of log
                string logName = NameLog(Location, Name);

                _emailer = new Emailer();

                try
                {
                    //_logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); 
                    //Gets it to read from config file
                    //For Files
                    //log4net.Config.DOMConfigurator.Configure
                    //	(new FileInfo("Aim.Recovery.Invoicing.CreateInvoicer.dll.Config"));

                    //try this to configure on the fly and not in config file
                    // using a FileAppender with a PatternLayout
                    log4net.Config.BasicConfigurator.Configure(
                        new log4net.Appender.FileAppender(
                            new log4net.Layout.SimpleLayout(), logName));


                    InitializeLog();
                }
                catch (NotSupportedException ex)
                {
                    //The file format is not supported
                    throw new Exception("NotSupportedException: " + ex.Message, ex.InnerException);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.GetType().ToString() + ": " + ex.Message, ex.InnerException);
                }
                finally
                {
                }
            }


            public static string NameLog(string Location, string Name)
            {
                _location = Location;
                _name = Name;
                _fullName = _location + @"\" + _name;
                return _fullName;
            }

            public static void InitializeLog()
            {
                if (_fullName.Length < 1)
                {
                    throw new Exception("The file name has not been set");
                }

                log4net.Appender.FileAppender fileAppender = new log4net.Appender.FileAppender();
                fileAppender.AppendToFile = true;
                fileAppender.File = _fullName;
            }

            public void WriteLog(string TextToWrite)
            {
                _logger.Info(TextToWrite);
            }

            public void WriteLog(string TextToWrite, params object[] List)
            {
                string placeHolder;
                for (int i = 0; i < List.Length; i++)
                {
                    if (List[i] != null)
                    {
                        placeHolder = "{" + i.ToString() + "}";
                        TextToWrite = TextToWrite.Replace(placeHolder, List[i].ToString());
                    }
                }

                WriteLog(TextToWrite);
            }

            public void WriteLogAlert(string TextToWrite)
            {
                _logger.Error(TextToWrite);
            }

            public void WriteLogAlert(string TextToWrite, params object[] List)
            {
                string placeHolder;
                for (int i = 0; i < List.Length; i++)
                {
                    placeHolder = "{" + i.ToString() + "}";
                    TextToWrite = TextToWrite.Replace(placeHolder, List[i].ToString());
                }
                WriteLogAlert(TextToWrite);
            }


            public string Location
            {
                get { return _location; }
                set { Location = _location; }
            }

            public string FullName
            {
                get { return _fullName; }
                set { FullName = _fullName; }
            }

            public string Name
            {
                get { return _name; }
                set { Name = _name; }
            }

            public Emailer Emailer
            {
                get { return _emailer; }
                set { Emailer = _emailer; }
            }

            public void DocumentError(Log InvLog, string Message, Exception Ex)
            {
                InvLog.WriteLogAlert(Message);
                InvLog.WriteLogAlert(Ex.Source);
                InvLog.WriteLogAlert(Ex.Message);
                InvLog.WriteLogAlert(Ex.StackTrace);
                Exception Innerex = new Exception();
                if (Ex.InnerException != null)
                {
                    Innerex = Ex.InnerException;
                }
                while (Innerex != null)
                {
                    InvLog.WriteLogAlert(Innerex.Message);
                    InvLog.WriteLogAlert(Innerex.Source);
                    Innerex = Innerex.InnerException;
                }


            }


        }
    }



