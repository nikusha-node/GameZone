namespace GameZone.DTOs.Genre;

public class GenreResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int GameCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
