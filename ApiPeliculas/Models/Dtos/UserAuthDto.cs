using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Models.Dtos
{
    public class UserAuthDto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="The user is obligatory")]
        public string User { get; set; }
        [Required(ErrorMessage = "The user is obligatory")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Password length must be between 4 and 10 characters ")]
        public string Password { get; set; }
    }
}
