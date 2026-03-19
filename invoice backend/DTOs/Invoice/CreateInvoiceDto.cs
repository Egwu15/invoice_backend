using System.ComponentModel.DataAnnotations;
using invoice_backend.DTOs.Items;

namespace invoice_backend.DTOs.Invoice;

public class CreateInvoiceDto
{
    [Required]
    [MaxLength(60)]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string ClientEmail { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(200)]
    public string BillTo { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string SendTo { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? SenderAddress { get; set; }

    public ICollection<CreateItemDto> Items { get; set; } = new List<CreateItemDto>();

    public DateTime? DueDate { get; set; }
}