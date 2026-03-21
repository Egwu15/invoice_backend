using invoice_backend.DTOs.Items;
using invoice_backend.Enums;

namespace invoice_backend.DTOs.Invoice;

public class UpdateInvoiceDto
{
    public string ClientEmail { get; set; } = "";
    public string Description { get; set; } = "";
    public string BillTo { get; set; } = "";

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    public string SendTo { get; set; } = "";
    public string SenderAddress { get; set; } = "";

    public string ClientName { get; set; } = "";

    public DateTime DueDate { get; set; }

    public List<CreateItemDto> Items { get; set; } = [];
}