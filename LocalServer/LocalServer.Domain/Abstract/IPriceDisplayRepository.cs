using LocalServer.Domain.Entities;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IPriceDisplayRepository
    {
        IQueryable<PriceDisplay> PriceDisplays { get; }
        void savePriceDisplay(PriceDisplay priceDisplay);
        void deletePriceDisplay(PriceDisplay priceDisplay);
     }
}
