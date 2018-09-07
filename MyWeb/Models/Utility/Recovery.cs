using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace MyWeb.Models.Utility
{
    public class Recovery
    {
        public bool SendEmail(String Smtp, string FromEmail, string Password, string To, string Subject, string Message)
        {
            try
            {
                MailMessage MyMessage = new MailMessage();
                MyMessage.From = new MailAddress(FromEmail);
                MyMessage.To.Add(To);
                MyMessage.Subject = Subject;
                MyMessage.Body = Message;
                MyMessage.IsBodyHtml = true;
                MyMessage.Priority = MailPriority.High;
                //if (Attach != null)
                //{
                //    string fileName = Attach.FileName;
                //    string filePath = "D:\\" + fileName;
                //    Attach.SaveAs(filePath);
                //    Attachment attach = new Attachment(filePath);
                //    attach.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                //    MyMessage.Attachments.Add(attach);
                //}

                SmtpClient mysmtp = new SmtpClient(Smtp);
                mysmtp.UseDefaultCredentials = false;
                mysmtp.EnableSsl = true;
                mysmtp.Credentials = new NetworkCredential(FromEmail, Password);
                mysmtp.Port = 25;
                mysmtp.Send(MyMessage);
                return true;
            }
            catch
            {

                return false;
            }


        }

    }
}