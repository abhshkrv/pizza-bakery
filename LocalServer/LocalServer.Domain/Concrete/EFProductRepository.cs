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
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Product> Products
        {
            get { return context.Products; }
        }

        public void saveProduct(Product product)
        {
            if (product.productID == 0)
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void quickSaveProduct(Product product)
        {
            if (product.productID == 0)
            {
                context.Products.Add(product);
                
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
               
            }
        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteProduct(Product product)
        {
            context.Products.Remove(product);

            context.SaveChanges();
        }

        public void deleteTable()
        {
           // context.E("DELETE FROM YOURTABLE WHERE CustomerID = {0}", customerId);

        }
    }
}