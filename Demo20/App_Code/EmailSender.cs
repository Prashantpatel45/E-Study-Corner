using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace Demo20.App_Code
{
    public class EmailSender
    {
        string MyEmailId, MyEmailPassCode;
            public EmailSender()
            {
                MyEmailId = "prashant1002patel@gmail.com";
                MyEmailPassCode = "qfdwvoggcxxpnabd";
            }

            internal bool SendMyEmail(string SendTo, string Subject, string Message)
            {
                try
                {
                    //setting protocol

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential nc = new NetworkCredential(MyEmailId,
                        MyEmailPassCode);
                    smtp.Credentials = nc;
                    smtp.EnableSsl = true;
                    //Setting MailMessage

                    MailMessage msg = new MailMessage();
                    MailAddress maFrom = new MailAddress(MyEmailId);
                    MailAddress mato = new MailAddress(SendTo);
                    msg.Sender = maFrom;
                    msg.To.Add(mato);
                    msg.Subject = Subject;
                    msg.From = maFrom;
                    msg.Body = Message;
                    smtp.Send(msg);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
    }