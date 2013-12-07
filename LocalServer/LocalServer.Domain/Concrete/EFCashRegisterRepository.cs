using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Concrete
{
    public class EFCashRegisterRepository : ICashRegisterRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<CashRegister> CashRegisters
        {
            get { return context.CashRegisters; }
        }

        public void saveCashRegister(CashRegister CashRegister)
        {
                context.CashRegisters.Add(CashRegister);
                context.SaveChanges();
        }

        public void deleteCashRegister(CashRegister CashRegister)
        {
            context.CashRegisters.Remove(CashRegister);
            context.SaveChanges();
        }
    }
}