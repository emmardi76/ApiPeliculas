using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository
{
    public class FilmRepository : IFilmRepository
    {
        private readonly ApplicationDbContext _database;

        public FilmRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public bool CreateFilm(Film Film)
        {
            _database.Film.Add(Film);
            return Save();
        }

        public bool DeleteFilm(Film Film)
        {
            _database.Film.Remove(Film);
            return Save();
        }

        public bool ExistFilm(string Name)
        {
            bool value = _database.Film.Any(f => f.Name.ToLower().Trim() == Name.ToLower().Trim());

            return value;
        }

        public bool ExistFilm(int Id)
        {
            return _database.Film.Any(f => f.Id == Id);
        }

        public ICollection<Film> GetFilm()
        {
            return _database.Film.OrderBy( f => f.Name).ToList();
        }

        public Film GetFilm(int FilmId)
        {
            return _database.Film.FirstOrDefault(f => f.Id == FilmId);
        }

        public ICollection<Film> GetFilmInCategory(int CatId)
        {
            return _database.Film.Include(ca => ca.Category).Where(ca => ca.categoryId == CatId).ToList();
        }

        public bool Save()
        {
            return _database.SaveChanges() >= 0 ?
               true : false;
        }

        public IEnumerable<Film> SearchFilm(string Name)
        {
            IQueryable<Film> query = _database.Film;

            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(f => f.Name.Contains(Name) || f.Description.Contains(Name));

                return query.ToList();
            }
            else
            {
                return null;
            }            
        }

        public bool UpdateFilm(Film Film)
        {
            _database.Film.Update(Film);
            return Save();
        }
    }
}
