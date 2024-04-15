using Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactFormModel model)
        {
            //Read SMTP section from Web.Config.
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            using (MailMessage mm = new MailMessage(smtpSection.From, "sachin.threepin@gmail.com"))
            {
                mm.Subject = model.Subject;
                mm.Body = "Name: " + model.Name + "<br /><br />Email: " + model.Email + "<br />" + model.Body;
              
                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = smtpSection.Network.Host;
                    smtp.EnableSsl = smtpSection.Network.EnableSsl;
                    NetworkCredential networkCred = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCred;
                    smtp.Port = smtpSection.Network.Port;
                    smtp.Send(mm);
                    ViewBag.Message = "Your message has been sent. Thank you!.";
                }
            }

            return View();
        }
    }
}