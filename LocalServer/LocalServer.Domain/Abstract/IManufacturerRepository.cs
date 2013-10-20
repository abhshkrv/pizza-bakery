using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IManufacturerRepository
    {
        IQueryable<Manufacturer> Manufacturers { get; }
        void saveManufacturer(Manufacturer manufacturer);
        void deleteManufacturer(Manufacturer manufacturer);

        void deleteTable();
    }
}
