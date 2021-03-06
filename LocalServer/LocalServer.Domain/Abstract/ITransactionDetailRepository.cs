﻿using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface ITransactionDetailRepository
    {
        IQueryable<TransactionDetail> TransactionDetails { get; }
        void saveTransactionDetail(TransactionDetail transactionDetail);
        void quickSaveTransactionDetail(TransactionDetail transactionDetail);
        void saveContext();
        void deleteTransactionDetail(TransactionDetail transactionDetail);

        void quickDeleteTransactionDetail(TransactionDetail transactionDetail);
    
    }
}
