using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IBatchRequestRepository
    {
        IQueryable<BatchRequest> BatchRequests { get; }
        void saveBatchRequest(BatchRequest batchRequest);
        void deleteBatchRequest(BatchRequest batchRequest);
    }
}
