namespace invoice_backend.DTOs.Invoice;

public class InvoiceSummaryDto
{
    public int Id { get; init; }


    public string ClientName { get; set; } = string.Empty;


    public string ClientEmail { get; set; } = string.Empty;


    public string? Description { get; set; }


    public string InvoiceNumber { get; set; } = string.Empty;


    public string BillTo { get; set; } = string.Empty;


    public string SendTo { get; set; } = string.Empty;

    public string? SenderAddress { get; set; }

    public DateTime CreatedAt { get; set; }


    public DateTime? DueDate { get; set; }
}