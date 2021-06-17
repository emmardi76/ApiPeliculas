using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ApiPeliculas.Models.Film;

namespace ApiPeliculas.Models.Dtos
{
    public class FilmDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The name is obligatory")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "The RouteImaage is obligatory")]
        public string RouteImaage { get; set; }
        [Required(ErrorMessage = "The Description is obligatory")]
        public string Description { get; set; }
        public string Duration { get; set; }
        public ClassificationType Classification { get; set; }
                
        public int categoryId { get; set; }       
        public Category Category { get; set; }
    }
}
