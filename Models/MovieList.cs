using System.ComponentModel.DataAnnotations;

public class MovieList
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string UserId { get; set; }
    [Required, MaxLength(50), MinLength(0)]
    public string Name { get; set; }

    public int MovieCount { get; set; }
    public float AverageRating { get; set; }
}