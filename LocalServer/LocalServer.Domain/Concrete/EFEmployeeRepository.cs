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
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Employee> Employees
        {
            get { return context.Employees; }
        }

        public void saveEmployee(Employee Employee)
        {
            if (Employee.employeeID == 0)
            {
                context.Employees.Add(Employee);
                context.SaveChanges();
            }
            else
            {
                context.Entry(Employee).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void deleteEmployee(Employee employee)
        {
            context.Employees.Remove(employee);
            context.SaveChanges();
        }
    }
}