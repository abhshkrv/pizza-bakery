using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalServer.WebUI.Models
{
    public class UserTransaction
    {
        public IEnumerable<TransactionDetail>TransactionDetail { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
    }
}