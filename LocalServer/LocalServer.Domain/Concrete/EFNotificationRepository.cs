using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Concrete
{
    public class EFNotificationRepository : INotificationRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Notification> Notifications
        {
            get { return context.Notifications; }
        }

        public void saveNotification(Notification notification)
        {
            if (notification.notificationID == 0)
            {
                context.Notifications.Add(notification);
                context.SaveChanges();
            }
            else
            {
                context.Entry(notification).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void deleteNotification(Notification Notification)
        {
            context.Notifications.Remove(Notification);
            context.SaveChanges();
        }
    }
}