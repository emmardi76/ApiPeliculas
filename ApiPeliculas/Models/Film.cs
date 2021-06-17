using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Models
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public class Film
    {
        [Key]

        public int Id { get; set; }
        public string Name { get; set; }
        public string RouteImaage { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public enum ClassificationType { Seven, Thirteen, Sixteen, Eighteen }
        public ClassificationType Classification { get; set; }
        public DateTime CreationDate { get; set; }

        //create a relationship with the category table
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public Category Category { get; set; }

    }
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
}
