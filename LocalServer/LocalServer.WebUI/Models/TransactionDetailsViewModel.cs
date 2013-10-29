using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalServer.WebUI.Models
{
    public class TransactionDetailsListViewModel
    {
        public Transaction transaction { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}