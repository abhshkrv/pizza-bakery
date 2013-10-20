using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class Transaction
    {
        [Key, Column(Order = 0)]
        public string transactionID { get; set; }
        public DateTime date { get; set; }
        [Key, Column(Order = 1)]
        public int cashierID { get; set; }
    }
}
