using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Concrete
{
    public class EFBatchDispatchDetailRepository : IBatchDispatchDetailRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<BatchDispatchDetail> BatchDispatchDetails
        {
            get { return context.BatchDispatchDetails; }
        }

        public void saveBatchDispatchDetail(BatchDispatchDetail BatchDispatchDetail)
        {
            context.BatchDispatchDetails.Add(BatchDispatchDetail);
            context.SaveChanges();
        }

        public void quickSaveBatchDispatchDetail(BatchDispatchDetail BatchDispatchDetail)
        {
            context.BatchDispatchDetails.Add(BatchDispatchDetail);
           // context.SaveChanges();
        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteBatchDispatchDetail(BatchDispatchDetail BatchDispatchDetail)
        {
            context.BatchDispatchDetails.Remove(BatchDispatchDetail);
            context.SaveChanges();
        }

        public void deleteTable()
        {
            

        }
    }
}
