using System.ComponentModel.DataAnnotations;

namespace invoice_backend.Entities;

public class User
{
    public int Id { get; init; }

    [Required]
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}