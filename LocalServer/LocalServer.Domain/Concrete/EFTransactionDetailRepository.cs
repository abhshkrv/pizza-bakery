using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Concrete
{
    public class EFTransactionDetailRepository : ITransactionDetailRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<TransactionDetail> TransactionDetails
        {
            get { return context.TransactionDetails; }
        }

        public void saveTransactionDetail(TransactionDetail TransactionDetail)
        {

        }

        public void deleteTransactionDetail(TransactionDetail TransactionDetail)
        {
            context.TransactionDetails.Remove(TransactionDetail);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
