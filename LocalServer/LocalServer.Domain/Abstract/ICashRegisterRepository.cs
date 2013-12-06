using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface ICashRegisterRepository
    {
        IQueryable<CashRegister> CashRegisters { get; }
        void saveCashRegister(CashRegister cashRegister);
        void deleteCashRegister(CashRegister CashRegister);
     }
}
