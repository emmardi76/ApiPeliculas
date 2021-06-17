using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ApiPeliculas.Models.Film;

namespace ApiPeliculas.Models.Dtos
{
    public class FilmCreateDto
    {
        [Required(ErrorMessage = "The name is obligatory")]
        public string Name { get; set; }
        public string RouteImaage { get; set; }

        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "The Description is obligatory")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The Duration is obligatory")]
        public string Duration { get; set; }
        public ClassificationType Classification { get; set; }

        public int categoryId { get; set; }        
    }
}
