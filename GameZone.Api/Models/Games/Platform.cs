using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Models;
public class Platform
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string Slug { get; set; } = string.Empty;

    public ICollection<Game> Games { get; set; } = new List<Game>();
}