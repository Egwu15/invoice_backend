using System.ComponentModel.DataAnnotations;

namespace invoice_backend.DTOs.Items;

public class CreateItemDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; } = 0;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;
}