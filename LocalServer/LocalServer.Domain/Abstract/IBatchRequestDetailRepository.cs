using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IBatchRequestDetailRepository
    {
        IQueryable<BatchRequestDetail> BatchRequestDetails { get; }
        void saveBatchRequestDetail(BatchRequestDetail batchRequestDetail);
        void quickSaveBatchRequestDetail(BatchRequestDetail batchRequestDetail);
        void saveContext();
        void deleteBatchRequestDetail(BatchRequestDetail batchRequestDetail);
    }
}
