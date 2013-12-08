﻿using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Concrete
{
    public class EFBatchRequestRepository : IBatchRequestRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<BatchRequest> BatchRequests
        {
            get { return context.BatchRequests; }
        }

        public void saveBatchRequest(BatchRequest batchRequest)
        {
            if (batchRequest.batchRequestID == 0)
            {
                context.BatchRequests.Add(batchRequest);
                context.SaveChanges();
            }
            else
            {
                context.Entry(batchRequest).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void deleteBatchRequest(BatchRequest batchRequest)
        {
            context.BatchRequests.Remove(batchRequest);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
