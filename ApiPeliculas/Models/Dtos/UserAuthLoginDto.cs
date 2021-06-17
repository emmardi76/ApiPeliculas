using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Models.Dtos
{
    public class UserAuthLoginDto
    {
        [Required(ErrorMessage = "The user is obligatory")]
        public string User { get; set; }
        [Required(ErrorMessage = "The user is obligatory")]        
        public string Password { get; set; }
    }
}
