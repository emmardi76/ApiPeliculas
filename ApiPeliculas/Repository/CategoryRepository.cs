using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _database;

        public CategoryRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public bool CreateCategory(Category Category)
        {
            _database.Category.Add(Category);
            return Save();
        }

        public bool DeleteCategory(Category Category)
        {
            _database.Category.Remove(Category);
            return Save();
        }

        public bool ExistCategory(string Name)
        {
            bool value = _database.Category.Any(c => c.Name.ToLower().Trim() == Name.ToLower().Trim());
            return value;
        }

        public bool ExistCategory(int Id)
        {
            return _database.Category.Any(c => c.Id == Id);
        }

        public ICollection<Category> GetCategory()
        {
            return _database.Category.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int CategoryId)
        {
            return _database.Category.FirstOrDefault(c => c.Id == CategoryId);
        }

        public bool Save()
        {
            return _database.SaveChanges() >= 0 ?
                true : false;
        }

        public bool UpdateCategory(Category Category)
        {
            _database.Category.Update(Category);
            return Save();
        }
    }
}
