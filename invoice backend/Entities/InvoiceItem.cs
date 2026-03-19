using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace invoice_backend.Entities;

public class InvoiceItem
{
    public int Id { set; get; }


    [Required]
    [MaxLength(50)]
    public required string Name { set; get; }

    [Range(1, double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { set; get; } = 0;

    [Range(1, int.MaxValue)]
    public int Quantity { set; get; } = 1;

    public int InvoiceId { set; get; }

    [ForeignKey("InvoiceId")]
    public Invoice? Invoice { get; set; }
}