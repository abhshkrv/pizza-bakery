using LocalServer.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IPriceDisplayRepository
    {
        IQueryable<PriceDisplay> PriceDisplays { get; }
        void editPriceDisplay(PriceDisplay priceDisplay);
        void addPriceDisplay(PriceDisplay priceDisplay);
        void deletePriceDisplay(PriceDisplay priceDisplay);
     }
}
