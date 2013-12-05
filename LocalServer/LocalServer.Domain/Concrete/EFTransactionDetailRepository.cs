using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
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

        public void saveTransactionDetail(TransactionDetail transactionDetail)
        {
            if (context.Entry(transactionDetail).State == EntityState.Detached)
            {
                context.TransactionDetails.Add(transactionDetail);
            }

            // context.Entry(Transaction).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void quickSaveTransactionDetail(TransactionDetail transactionDetail)
        {
            context.TransactionDetails.Add(transactionDetail);
        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteTransactionDetail(TransactionDetail TransactionDetail)
        {
            context.TransactionDetails.Remove(TransactionDetail);
            context.SaveChanges();
        }

        public void quickDeleteTransactionDetail(TransactionDetail TransactionDetail)
        {
            context.TransactionDetails.Remove(TransactionDetail);
            //context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
