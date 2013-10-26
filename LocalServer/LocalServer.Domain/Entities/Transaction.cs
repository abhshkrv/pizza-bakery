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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int transactionID { get; set; }
        public DateTime date { get; set; }
        public int cashierID { get; set; }
    }
}
