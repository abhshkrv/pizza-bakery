using LocalServer.Domain.Entities;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface IEmployeeRepository
    {
        IQueryable<Employee> Employees { get; }
        void saveEmployee(Employee Employee);
        void deleteEmployee(Employee Employee);
     }
}
