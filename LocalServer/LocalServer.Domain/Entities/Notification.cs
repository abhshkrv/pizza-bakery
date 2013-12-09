using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class Notification
    {

        public int notificationID { get; set; }
        public string content { get; set; }
    }
}
