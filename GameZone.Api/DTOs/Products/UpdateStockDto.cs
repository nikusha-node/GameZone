using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;

public class UpdateStockDto
{
    [Range(0, 1000000)]
    public int StockQuantity { get; set; }
}