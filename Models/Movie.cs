public class Movie
{
    public int Id { get; set; }
    public string ImdbId { get; set; }
    public int Year { get; set; }
    public string Title { get; set; }
    public int? Rating { get; set; }
    public string Genres { get; set; }
    public string Director { get; set; }
    public string Runtime { get; set; }
    public string Rated { get; set; }
    public bool DataPopulated { get; set; }
}