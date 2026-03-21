namespace invoice_backend.DTOs.Invoice;

public class InvoiceStatsDto
{
    public int TotalInvoices { get; set; } = 0;
    public int TotalPending { get; set; } = 0;
    public decimal TotalRevenue { get; set; } = 0;
    public int TotalPaid { get; set; } = 0;
}