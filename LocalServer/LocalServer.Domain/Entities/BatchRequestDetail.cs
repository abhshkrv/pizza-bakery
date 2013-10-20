using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class BatchRequestDetail
    {
        [Key, Column(Order = 0)]
        public int batchRequestID { get; set; }
        [Key, Column(Order = 1)]
        public string barcode { get; set; }
        public int quantity { get; set; }
    }
}
