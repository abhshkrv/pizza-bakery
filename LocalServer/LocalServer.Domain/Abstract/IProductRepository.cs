using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        void saveProduct(Product product);
        void deleteProduct(Product product);
        void deleteTable();

        void quickSaveProduct(Product product);

        void saveContext();
    }
}
