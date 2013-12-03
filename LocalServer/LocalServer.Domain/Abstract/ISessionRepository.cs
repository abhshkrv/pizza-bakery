using LocalServer.Domain.Entities;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface ISessionRepository
    {
        IQueryable<CRSession> Sessions { get; }
        void saveSession(CRSession session);
        void deleteSession(CRSession session);
     }
}
