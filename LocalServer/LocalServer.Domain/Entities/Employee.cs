using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class Employee
    {
        public int employeeID { get; set; }
        string hashID { get; set; }
        string role { get; set; }
        string userID { get; set; }
        string password { get; set; }
    }
}
