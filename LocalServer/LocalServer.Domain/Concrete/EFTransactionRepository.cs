using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Concrete
{
    public class EFTransactionRepository : ITransactionRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Transaction> Transactions
        {
            get { return context.Transactions; }
        }

        public void saveTransaction(Transaction Transaction)
        {
            if (context.Entry(Transaction).State == EntityState.Detached)
            {
                context.Transactions.Add(Transaction);
            }

           // context.Entry(Transaction).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void deleteTransaction(Transaction Transaction)
        {
            context.Transactions.Remove(Transaction);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
