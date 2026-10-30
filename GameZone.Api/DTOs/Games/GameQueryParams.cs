using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;

public class GameQueryParams
{
    [StringLength(100)]
    public string? Search { get; set; }

    public int? PlatformId { get; set; }

    [StringLength(60)]
    public string? PlatformSlug { get; set; }

    [StringLength(100)]
    public string? Genre { get; set; }

    public bool? IsDigital { get; set; }

    [Range(0, 1000000)]
    public decimal? MinPrice { get; set; }

    [Range(0, 1000000)]
    public decimal? MaxPrice { get; set; }

    public bool? InStock { get; set; }

    [StringLength(20)]
    public string? Sort { get; set; }

    [Range(1, 10000)]
    public int Page { get; set; } = 1;

    [Range(1, 50)]
    public int PageSize { get; set; } = 12;
}