using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class Employee
    {
        public int employeeID { get; set; }
        public string hashID { get; set; }
        public string role { get; set; }
        public string userID { get; set; }
        public string password { get; set; }
    }
}
