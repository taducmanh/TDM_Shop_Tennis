using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ShopBanDoTheThao.Models
{
    public class Mail
    {
        public void GuiMail(string email, string tieuDe, string noiDung)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.TargetName = "STARTTLS/smtp.gmail.com";
            mail.From = new MailAddress("Shopnova@gmail.com");
            mail.To.Add("Shopnova@gmail.com");
        
            mail.Subject = ""+tieuDe+" ";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";
            mail.Body += "<tr>";
            mail.Body += "<td>Xìn chào: </td><td> "+email+" </td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>"+noiDung+"</td>";
            mail.Body += "</tr>";
            mail.Body += "</table>";
            mail.Body += "</body>";
            mail.Body += "</html>";
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            ShopEntities shop=new ShopEntities();
            var caiDat = shop.CaiDats.SingleOrDefault(c => c.Id == 1);
            try
            {
                SmtpServer.Credentials = new System.Net.NetworkCredential(caiDat.EmailLienHe, caiDat.MatKhauMail);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
           
           

        }


    }
}