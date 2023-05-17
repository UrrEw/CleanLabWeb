using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace LabWeb.Service
{
    public class MailService
    {
        private string google_account = "Fatduck0403@gmail.com";
        private string google_password = "qcohutshcefhujcv";
        private string google_email = "Fatduck0403@gmail.com";

        public string GetAuthCode()
        {
            string Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
            string AuthCode = "";
            Random rd = new Random();

            for (int i = 0; i < 10; i++)
            {
                AuthCode += Code[rd.Next(Code.Count())];
            }

            return AuthCode;
        }

        public string GetMailBody(string TempMail,string UserName,string ValidateUrl)
        {
            TempMail = TempMail.Replace("{{UserName}}", UserName);
            TempMail = TempMail.Replace("{{ValidateUrl}}", ValidateUrl);

            return TempMail;
        }

         public string GetResetMailBody(string TempMail,string password)
        {
            TempMail = TempMail.Replace("{{password}}", password);
            return TempMail;
        }

        public void SendRegisterMail(string MailBody,string ToEmail)
        {
            SmtpClient SmtpClient = new SmtpClient("smtp.gmail.com");
            SmtpClient.Port = 587;
            SmtpClient.Credentials = new System.Net.NetworkCredential(google_account, google_password);
            SmtpClient.EnableSsl = true;

            MailMessage Mail = new MailMessage();
            Mail.From = new MailAddress(google_email);
            Mail.To.Add(ToEmail);
            Mail.Subject = "LabWeb會員註冊";
            Mail.Body = MailBody;
            Mail.IsBodyHtml = true;

            SmtpClient.Send(Mail);
        }
    }
}