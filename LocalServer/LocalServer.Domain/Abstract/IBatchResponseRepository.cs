using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IBatchResponseRepository
    {
        IQueryable<BatchRequest> BatchRequests { get; }
        void saveBatchResponse(BatchRequest batchRequest);
        void deleteBatchResponse(BatchRequest batchRequest);
    }
}
