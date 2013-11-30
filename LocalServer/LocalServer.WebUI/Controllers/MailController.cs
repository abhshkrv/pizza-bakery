
using ActionMailer.Net.Mvc;
using LocalServer.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalServer.WebUI.Controllers
{
    public class MailController : ActionMailer.Net.Mvc.MailerBase
    {
        public EmailResult SampleEmail(UserTransaction model,string subject)
        {
            To.Add(model.email);
            From = "no-reply@pizza.com";
           // Subject = "Transaction details for transaction ID:11223344 dated : 10/11/2013";
            Subject = subject;
            return Email("SampleEmail",model);
        }
    }
}
