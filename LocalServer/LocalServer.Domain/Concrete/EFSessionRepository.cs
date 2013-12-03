using LocalServer.Domain.Abstract;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Concrete
{
    public class EFSessionRepository : ISessionRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<CRSession> Sessions
        {
            get { return context.Sessions; }
        }

        public void saveSession(CRSession Session)
        {
            if (Session.CRsessionID == 0)
            {
                context.Sessions.Add(Session);
                context.SaveChanges();
            }
            else
            {
                context.Entry(Session).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void deleteSession(CRSession Session)
        {
            context.Sessions.Remove(Session);
            context.SaveChanges();
        }
    }
}