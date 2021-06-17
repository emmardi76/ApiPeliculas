using ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategory();
        Category GetCategory(int CategoryId);
        bool ExistCategory(string Name);
        bool ExistCategory(int Id);
        bool CreateCategory(Category Category);
        bool UpdateCategory(Category Category);
        bool DeleteCategory(Category Category);
        bool Save();
    }
}
