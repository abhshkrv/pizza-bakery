using LocalServer.Domain.Entities;
using LocalServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocalServer.Domain.Abstract
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
        void saveCategory(Category category);
        void deleteCategory(Category category);
        void deleteTable();

        void saveContext();

        void quickSaveCategory(Entities.Category category);
    }
}
