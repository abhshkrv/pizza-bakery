using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using LocalServer.Domain.Entities;


namespace LocalServer.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<BatchRequest> BatchRequests { get; set; }
        public DbSet<BatchRequestDetail> BatchRequestDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<Member> Members { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
