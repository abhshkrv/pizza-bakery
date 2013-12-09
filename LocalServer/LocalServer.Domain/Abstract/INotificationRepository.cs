using LocalServer.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface INotificationRepository
    {
        IQueryable<Notification> Notifications { get; }
       // void editNotification(Notification Notification);
        void saveNotification(Notification Notification);
        void deleteNotification(Notification Notification);
     }
}
