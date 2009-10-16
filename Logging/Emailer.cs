using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;

namespace AIMHealth.Safari.Logging
{

        public class Emailer
        {

            public void SendAlert(
                string Subject
                , string Body
                , Log InvLog
                , Dictionary<string, object> ConfigList
                , bool Alert
                )
            {

                try
                {

                    SmtpClient mailClient = new SmtpClient();
                    mailClient.Host = ConfigList["EmailServer"].ToString();
                    mailClient.UseDefaultCredentials = false;

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("itopsalerts@aimhealth.com", "Invoicing Alerts");
                        message.Subject = Subject;
                        message.IsBodyHtml = false;

                        string body = Body + "\n\nThe logfile is located at: "
                                            + InvLog.FullName
                                            + ".\n";

                        body = body + "\n" + ConfigList["ConnectionString"].ToString() + "\n";

                        message.Body = body;

                        StringCollection EmailList;

                        if (Alert)
                        {
                            EmailList = (StringCollection)ConfigList["AlertEmailAddresses"];
                        }
                        else
                        {
                            EmailList = (StringCollection)ConfigList["EmailAddresses"];
                        }


                        foreach (string s in EmailList)
                        {

                            message.To.Add(s);
                        }

                        mailClient.Send(message);
                    }
                }
                catch (SmtpException ex)
                {
                    InvLog.WriteLogAlert("*****SMTP Exception sending alert email: " + ex.Message);
                }
                catch (Exception ex)
                {
                    InvLog.WriteLogAlert("*****Exception sending alert email: " + ex.Message);
                }
            }


            static public List<string> ExtractEmails(string Emails)
            {
                List<string> eList = new List<string>();

                int start = 0;
                int junction = 0;

                while (Emails.Length > start)
                {
                    junction = Emails.IndexOf(";", start);
                    if (junction == -1)
                    {
                        eList.Add(Emails.Substring(start));
                        start = Emails.Length;
                    }
                    else
                    {
                        eList.Add(Emails.Substring(start, junction));
                        start = junction + 1;
                    }

                }

                return eList;
            }





    }
}
