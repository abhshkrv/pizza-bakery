using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalServer.WebUI.Controllers
{
    public class NotificationController : Controller
    {
        INotificationRepository _notifrepo;
        public NotificationController(INotificationRepository nop)
        {
            _notifrepo = nop;
        }
        //
        // GET: /Notification/

        public ActionResult Index()
        {
            var notifs = _notifrepo.Notifications;
            return View(notifs);

        }

        public string send(string message)
        {
            try
            {
                Notification n = new Notification();
                n.content = message;
                _notifrepo.saveNotification(n);
                return "success";
            }
            catch {
                return "Fail";
            }
        }

    }
}
