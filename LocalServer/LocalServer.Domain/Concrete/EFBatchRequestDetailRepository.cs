using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Concrete
{
    public class EFBatchRequestDetailRepository : IBatchRequestDetailRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<BatchRequestDetail> BatchRequestDetails
        {
            get { return context.BatchRequestDetails; }
        }

        public void saveBatchRequestDetail(BatchRequestDetail BatchRequestDetail)
        {
            context.BatchRequestDetails.Add(BatchRequestDetail);
            context.SaveChanges();
        }

        public void quickSaveBatchRequestDetail(BatchRequestDetail BatchRequestDetail)
        {
            context.BatchRequestDetails.Add(BatchRequestDetail);
           // context.SaveChanges();
        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteBatchRequestDetail(BatchRequestDetail BatchRequestDetail)
        {
            context.BatchRequestDetails.Remove(BatchRequestDetail);
            context.SaveChanges();
        }

        public void deleteTable()
        {
            

        }
    }
}
