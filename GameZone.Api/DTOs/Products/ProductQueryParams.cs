using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;
public class ProductQueryParams
{
    [StringLength(100)]
    public string? Search { get; set; }

    public int? CategoryId { get; set; }

    [StringLength(120)]
    public string? CategorySlug { get; set; }

    public int? BrandId { get; set; }

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