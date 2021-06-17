using ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User> GetUser();
        User GetUser(int UserId);
        bool ExistUser(string User);
        User RegisterUser(User User, string password);
        User LoginUser(string User, string password);       
        bool Save();
    }
}
