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
    public class EFPriceDisplayRepository : IPriceDisplayRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<PriceDisplay> PriceDisplays
        {
            get { return context.PriceDisplays; }
        }

        public void editPriceDisplay(PriceDisplay PriceDisplay)
        {
                context.Entry(PriceDisplay).State = EntityState.Modified;
                context.SaveChanges();
        }

        public void addPriceDisplay(PriceDisplay PriceDisplay)
        {
            context.PriceDisplays.Add(PriceDisplay);
            context.SaveChanges();

        }

        public void deletePriceDisplay(PriceDisplay PriceDisplay)
        {
            context.PriceDisplays.Remove(PriceDisplay);
            context.SaveChanges();
        }
    }
}