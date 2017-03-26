using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace RealEstate.extModals
{
    public class sendEmailModal
    {
        public static bool sendEmail(string usEmail,string baseUrl,Guid guid)
        {
            string bodyemail = "<a href=" + baseUrl + "Activation/" + guid + ">Click here to Activate your Account</a>";
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("vaxjoestate@gmail.com");
            mail.To.Add(usEmail);
            mail.Subject = "Click below Link to Activate your Account";
            mail.Body = bodyemail;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("vaxjoestate@gmail.com", "vaxj@765=");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

            return true;
        }
    }
}