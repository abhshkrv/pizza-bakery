using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class Session
    {
        
        public int sessionID { get; set; }
        public string cashRegister { get; set; }
        public string userID { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
