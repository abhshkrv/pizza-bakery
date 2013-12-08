using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class BatchDispatch
    {
        public int batchDispatchID { get; set; }
        public DateTime timeStamp { get; set; }
        public byte status { get; set; } //{sent,attended,verified}
    }
}
