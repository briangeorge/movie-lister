using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Arcadia.Challenge.Models
{
    public class MovieList
    {
        public int Id { get; set; } = 0;
        [Required, MaxLength(50)]
        public string UserId { get; set; } = string.Empty;
        [Required, MaxLength(50), MinLength(0)]
        public string Name { get; set; } = string.Empty;

        public int MovieCount { get; set; }
        public float AverageRating { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
    }
}