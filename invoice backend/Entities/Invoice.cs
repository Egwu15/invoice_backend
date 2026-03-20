using System.ComponentModel.DataAnnotations;

namespace invoice_backend.Entities;

public class Invoice
{
    public int Id { get; init; }

    [Required]
    public User User { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(60)]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string ClientEmail { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(30)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string BillTo { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string SendTo { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? SenderAddress { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}