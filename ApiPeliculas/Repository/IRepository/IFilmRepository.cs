using ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IFilmRepository
    {
        ICollection<Film> GetFilm();
        ICollection<Film> GetFilmInCategory(int CatId);
        Film GetFilm(int FilmId);
        bool ExistFilm(string Name);
        IEnumerable<Film> SearchFilm(string Name);
        bool ExistFilm(int Id);
        bool CreateFilm(Film Film);
        bool UpdateFilm(Film Film);
        bool DeleteFilm(Film Film);
        bool Save();
    }
}
