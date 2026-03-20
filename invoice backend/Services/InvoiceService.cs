using invoice_backend.Data;
using invoice_backend.DTOs.Invoice;
using invoice_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace invoice_backend.Services;

public class InvoiceService(ApplicationDbContext context)
{
    public async Task<InvoiceSummaryDto> CreateInvoice(CreateInvoiceDto dto)
    {
        var uuId = await GetInvoiceNumber();

        var invoiceItems = dto.Items.Select(item => new InvoiceItem
            { Name = item.Name, Amount = item.Amount, Quantity = item.Quantity }).ToList();


        var invoice = new Invoice
        {
            ClientEmail = dto.ClientEmail,
            BillTo = dto.BillTo,
            ClientName = dto.ClientName,
            DueDate = dto.DueDate,
            SenderAddress = dto.SenderAddress,
            Description = dto.Description,
            InvoiceNumber = uuId,
            SendTo = dto.SendTo,
            Items = invoiceItems
        };

        await context.Invoices.AddAsync(invoice);
        await context.SaveChangesAsync();


        return new InvoiceSummaryDto
        {
            InvoiceNumber = invoice.InvoiceNumber,
            BillTo = invoice.BillTo,
            SendTo = invoice.SendTo
        };
    }

    private async Task<string> GetInvoiceNumber()
    {
        var idDoesNotExist = true;
        var random = new Random();
        var invoicePrefix = "INV-" + DateTime.UtcNow.ToString("yy-MM-dd") + "-";
        var newUid = random.GetHexString(4, true);
        while (idDoesNotExist)
        {
            var invoice =
                await context.Invoices.FirstOrDefaultAsync(invoice => invoice.InvoiceNumber == invoicePrefix + newUid);
            if (invoice is null) idDoesNotExist = false;
            else newUid = random.GetHexString(4, true);
        }

        return invoicePrefix + newUid.ToUpper();
    }
}