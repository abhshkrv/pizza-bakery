﻿using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IBatchDispatchRepository
    {
        IQueryable<BatchDispatch> BatchDispatchs { get; }
        void saveBatchDispatch(BatchDispatch batchDispatch);
        void deleteBatchDispatch(BatchDispatch batchDispatch);
    }
}
