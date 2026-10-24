using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Models;
public class Game
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Genre { get; set; }

    [MaxLength(150)]
    public string? Developer { get; set; }

    [MaxLength(150)]
    public string? Publisher { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    [MaxLength(20)]
    public string? AgeRating { get; set; } 

    public bool IsDigital { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;
}